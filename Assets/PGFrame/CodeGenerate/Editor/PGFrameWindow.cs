using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Excel;
using PogoTools;
using Newtonsoft.Json;
using System.Linq;

public class PGFrameWindow : EditorWindow
{
	public static readonly string lt = "PGFrame";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);

	[MenuItem ("PogoRock/PGFrame... %`")]
	static void Init ()
	{
		PGFrameWindow window = (PGFrameWindow)EditorWindow.GetWindow (typeof(PGFrameWindow));
		window.titleContent = new GUIContent ("PGFrame", Resources.Load<Texture2D> ("pgf_icon"));
		window.Show ();
	}

	void OnGUI ()
	{
		if (xElements == null)
			RefreshFiles ();

		GUILayout.BeginVertical ();
		GUILayout.Label ("PGFrame", EditorStyles.boldLabel);
		if (GUILayout.Button ("刷新")) {
			RefreshFiles ();
		}
		DesignList ();

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("发布代码")) {
//			Generator.GenerateCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}

		if (GUILayout.Button ("删除代码")) {
//			Generator.DeleteCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}
		GUILayout.EndVertical ();
	}

	XLSXElement[] xElements;

	PGCodeGenerator Generator;
	XLSXJsonConverter Converter;

	void RefreshFiles ()
	{
		Generator = new PGCodeGenerator ();
		Generator.Init ();

		Converter = new XLSXJsonConverter ();

		string[] fileNames = Directory.GetFiles (Path.Combine (Application.dataPath, "PGFrameDesign"), "*.xlsx").Where (_ => !_.Contains ("~$")).ToArray ();
		xElements = new XLSXElement[fileNames.Length];

		for (int i = 0; i < fileNames.Length; i++) {
			XLSXElement e = new XLSXElement ();
			e.FileInfo = new FileInfo (fileNames [i]);
			xElements [i] = e;
		}
	}

	Vector2 scrollViewPos;

	/// <summary>
	/// xlsx 文件/工作表列表部分
	/// </summary>
	void DesignList ()
	{
		scrollViewPos = GUILayout.BeginScrollView (scrollViewPos);

		if (xElements != null) {
			for (int i = 0; i < xElements.Length; i++) {
				XLSXElement xe = xElements [i];

				GUILayout.BeginHorizontal ();

				if (GUILayout.Button (xe.toggleOpen ? "-" : "+", GUILayout.MaxWidth (20))) {
					xe.toggleOpen = !xe.toggleOpen;
				}
				EditorGUILayout.LabelField (xe.FileInfo.Name);
				if (GUILayout.Button ("Open", GUILayout.MaxWidth (60))) {
					var xlsxAsset = AssetDatabase.LoadAssetAtPath<Object> ("Assets/PGFrameDesign/" + xe.FileInfo.Name);
					AssetDatabase.OpenAsset (xlsxAsset);
				}
				if (GUILayout.Button ("Json", GUILayout.MaxWidth (60))) {
					Converter.SetElement (xe);
					Converter.Convert (null);
					AssetDatabase.Refresh ();
				}
				GUILayout.EndHorizontal ();

				if (xe.toggleOpen) {
					EditorGUI.indentLevel++;
					for (int j = 0; j < xe.ds.Tables.Count; j++) {
						GUILayout.BeginHorizontal ();
						GUILayout.Label ("", GUILayout.MaxWidth (20));
						DataTable dt = xe.ds.Tables [j];
						EditorGUILayout.LabelField (dt.TableName);
						if (GUILayout.Button ("Json", GUILayout.MaxWidth (60))) {
							Converter.SetDataTable (xe, dt);
							Converter.Convert (null);
							AssetDatabase.Refresh ();
						}
						if (GUILayout.Button ("Generate", GUILayout.MaxWidth (60))) {
							Converter.SetDataTable (xe, dt);
							Converter.Convert (Generator);
							AssetDatabase.Refresh ();
						}
						GUILayout.EndHorizontal ();
					}
					EditorGUI.indentLevel--;
				}
			}
		}

		GUILayout.EndScrollView ();
	}
}
