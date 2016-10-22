using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PogoTools;
using Newtonsoft.Json;

public class PGFrameWindow : EditorWindow
{
	public static readonly string lt = "PGFrame";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);

	[MenuItem ("PogoRock/PGFrame...")]
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

		if (GUILayout.Button ("发布代码")) {
			Generator.GenerateCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}

		if (GUILayout.Button ("删除代码")) {
			Generator.DeleteCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}
		GUILayout.EndVertical ();
	}

	XLSXElement[] xElements;

	PGCodeGenerator Generator;

	void RefreshFiles ()
	{
		Generator = new PGCodeGenerator ();
		Generator.Init ();

		string[] fileNames = Directory.GetFiles (Path.Combine (Application.dataPath, "PGFrameDesign"), "*.xlsx");
		xElements = new XLSXElement[fileNames.Length];

		for (int i = 0; i < fileNames.Length; i++) {
			xElements [i] = new XLSXElement () {
				fileInfo = new FileInfo (fileNames [i])
			};
		}
	}

	void DesignList ()
	{
		GUILayout.BeginVertical ();

		if (xElements != null) {
			for (int i = 0; i < xElements.Length; i++) {
				XLSXElement xe = xElements [i];
				EditorGUILayout.LabelField (xe.fileInfo.Name);
			}
		}

		GUILayout.EndVertical ();
	}
}
