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
using Newtonsoft.Json.Linq;
using UnityEditorInternal;

public class PGFrameWindow : EditorWindow
{
	public static readonly string lt = "PGFrame";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);
	public static readonly Color lcr = new Color32 (255, 162, 162, 255);

	[MenuItem ("PogoRock/PGFrame... %`")]
	static void Init ()
	{
		PGFrameWindow window = (PGFrameWindow)EditorWindow.GetWindow (typeof(PGFrameWindow));
		window.titleContent = new GUIContent ("PGFrame", Resources.Load<Texture2D> ("pgf_icon"));
		window.Show ();
	}

	void OnGUI ()
	{
		if (NeedRefresh)
			RefreshFiles ();

		GUILayout.BeginVertical ();
		GUILayout.Label ("PGFrame", EditorStyles.boldLabel);
		if (GUILayout.Button ("刷新")) {
			RefreshFiles ();
		}
		if (GUILayout.Button ("添加 Workspace")) {
			JsonWorkspaceManager manager = new JsonWorkspaceManager (Path.Combine (Application.dataPath, JsonRoot));
			PRDebug.TagLog (lt, lcr, manager.CreateWorkspace ("WS3"));
			AssetDatabase.Refresh ();
		}
		DesignList ();

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("发布代码")) {
//			for (int i = 0; i < xElements.Length; i++) {
//				XLSXElement xe = xElements [i];
//				Converter.SetElement (xe);
//				Converter.Convert (Generator, false);
//			}
			AssetDatabase.Refresh ();
		}

		if (GUILayout.Button ("删除代码")) {
//			for (int i = 0; i < xElements.Length; i++) {
//				XLSXElement xe = xElements [i];
//				Converter.SetElement (xe);
//				Converter.Convert (Generator, true);
//			}
			AssetDatabase.Refresh ();
		}
		GUILayout.EndVertical ();
	}

	XLSXElement[] xElements;
	JSONElement[] jElements;

	PGCodeGenerator Generator;
	XLSXJsonConverter Converter;

	public static readonly string JsonRoot = "PGFrameDesign/JsonData";

	public DirectoryInfo SelectedWorkspace;
	public JSONElement SelectedWorkspaceCommon;
	public DirectoryInfo[] WorkspaceDirectoryInfos;

	public bool NeedRefresh = true;

	void RefreshFiles ()
	{
		NeedRefresh = false;
		string JsonRootFull = Path.Combine (Application.dataPath, JsonRoot);
		if (SelectedWorkspace == null) {
			DirectoryInfo di = new DirectoryInfo (JsonRootFull);
			WorkspaceDirectoryInfos = di.GetDirectories ();
			PRDebug.TagLog (lt, lc, JsonConvert.SerializeObject (WorkspaceDirectoryInfos.Select (_ => _.Name).ToList ()));
		} else {
			string JsonWSFull = Path.Combine (JsonRootFull, SelectedWorkspace.Name);
			DirectoryInfo di = new DirectoryInfo (JsonWSFull);
			FileInfo[] fis = di.GetFiles ("*.json");

			List<JSONElement> je = new List<JSONElement> (fis.Length - 1);
			for (int i = 0; i < fis.Length; i++) {
				FileInfo fi = fis [i];

				JSONElement e = new JSONElement ();
				e.FileInfo = fi;
				if (fi.Name == "_Common.json") {
					SelectedWorkspaceCommon = e;
				} else {
					je.Add (e);
				}
			}
			jElements = je.ToArray ();
		}
	}

	Vector2 scrollViewPos;

	ReorderableList WSJsonFilesList;

	void ResetReorderableList ()
	{
		JArray ja_elements = SelectedWorkspaceCommon.jo ["ElementFiles"] as JArray;
		WSJsonFilesList = new ReorderableList (ja_elements, typeof(JToken));
		WSJsonFilesList.drawHeaderCallback += (Rect rect) => {
			GUI.Label (rect, "Title");
		};
		float[] split = new float[]{ 0f, 1f };
		WSJsonFilesList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
			
			Rect r = new Rect (rect);
			r.y -= 1;
			r.height -= 2;
			int split_idx = 0;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]);
			if (GUI.Button (r, (ja_elements [index] as JObject) ["File"].Value<string> ())) {
				// PR_TODO:
			}
		};

		WSJsonFilesList.onAddCallback += (ReorderableList list) => {

			if (ElementName == null) {
				ElementName = string.Empty;
			} else {
				if (ElementName != string.Empty) { // todo
					string jsonName = string.Format (
						                  "{0}.{1}.{2}.json", 
						                  SelectedWorkspaceCommon.Workspace, 
						                  "Element",
						                  ElementName
					                  );

					if (jElements.FirstOrDefault (_ => _.Name == ElementName) == null) {

						JObject jo = new JObject ();
						jo.Add ("DocType", "Element");
						jo.Add ("File", jsonName);
						ja_elements.Add (jo);
						SaveCommonFile ();

						PRDebug.TagLog (lt, lc, "成功创建了 Element: " + ElementName);
						ElementName = null;
					} else {
						PRDebug.TagLog (lt, lc, "该工作空间中已经含有名字: " + ElementName);
					}
				} else {
					PRDebug.TagLog (lt, lc, "没有填入名字,取消创建");
					ElementName = null;
					SaveCommonFile ();
				}
			}
		};

		WSJsonFilesList.onRemoveCallback += (ReorderableList list) => {
			JObject jo = ja_elements [list.index] as JObject;

			if (EditorUtility.DisplayDialog ("警告!", string.Format ("确定删除框架中的{0}文件?", jo ["File"].Value<string> ()), "Yes", "No")) {
				ja_elements.RemoveAt (list.index);
				SaveCommonFile ();
				ElementName = null;
			}
		};

		WSJsonFilesList.onReorderCallback += (ReorderableList list) => {
			SaveCommonFile ();
		};
	}

	string ElementName = null;

	void SaveCommonFile ()
	{
		SelectedWorkspaceCommon.Save ();
	}

	void DesignList ()
	{
		if (WorkspaceDirectoryInfos == null) {
			NeedRefresh = true;
			return;
		}

		string JsonRootFull = Path.Combine (Application.dataPath, JsonRoot);
		if (SelectedWorkspace == null) {
			GUILayout.Label ("Root", EditorStyles.boldLabel);
			scrollViewPos = GUILayout.BeginScrollView (scrollViewPos);
			for (int i = 0; i < WorkspaceDirectoryInfos.Length; i++) {
				DirectoryInfo wdi = WorkspaceDirectoryInfos [i];
				if (GUILayout.Button (wdi.Name)) {
					SelectedWorkspace = wdi;
					NeedRefresh = true;
				}
			}
			GUILayout.EndScrollView ();
		} else {
			GUILayout.Label ("Workspace:" + SelectedWorkspace.Name, EditorStyles.boldLabel);
			if (GUILayout.Button ("<<")) {
				SelectedWorkspace = null;
				NeedRefresh = true;
				WSJsonFilesList = null;
				SelectedWorkspaceCommon = null;
			}
			if (SelectedWorkspaceCommon != null) {
//				PRDebug.TagLog ("PGFrameWindow.Debug", Color.yellow, JsonConvert.SerializeObject (SelectedWorkspaceCommon));
				if (WSJsonFilesList == null)
					ResetReorderableList ();
				GUILayout.BeginVertical ();
				WSJsonFilesList.DoLayoutList ();
				if (ElementName != null) {
					ElementName = GUILayout.TextField (ElementName);
				}
				GUILayout.EndVertical ();
//				JArray ja_elements = SelectedWorkspaceCommon ["ElementFiles"] as JArray;

//				for (int i = 0; i < ja_elements.Count; i++) {
//					JObject jo = ja_elements [i] as JObject;
//					GUILayout.BeginHorizontal ();
//					GUILayout.Label (jo ["File"].Value<string> ());
//					GUILayout.EndHorizontal ();
//				}

			}
		}
	}

	/*
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
					Converter.Convert (null, false);
					AssetDatabase.Refresh ();
				}
				if (GUILayout.Button ("Generate", GUILayout.MaxWidth (60))) {
					Converter.SetElement (xe);
					Converter.Convert (Generator, false);
					AssetDatabase.Refresh ();
				}
				if (GUILayout.Button ("Delete", GUILayout.MaxWidth (60))) {
					Converter.SetElement (xe);
					Converter.Convert (Generator, true);
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
							Converter.Convert (null, false);
							AssetDatabase.Refresh ();
						}
						if (GUILayout.Button ("Generate", GUILayout.MaxWidth (60))) {
							Converter.SetDataTable (xe, dt);
							Converter.Convert (Generator, false);
							AssetDatabase.Refresh ();
						}
						if (GUILayout.Button ("Delete", GUILayout.MaxWidth (60))) {
							Converter.SetDataTable (xe, dt);
							Converter.Convert (Generator, true);
							AssetDatabase.Refresh ();
						}
						GUILayout.EndHorizontal ();
					}
					EditorGUI.indentLevel--;
				}
			}
		}

		GUILayout.EndScrollView ();


*/
}
