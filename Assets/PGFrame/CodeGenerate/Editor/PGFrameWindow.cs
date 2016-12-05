using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using PogoTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using System;

public partial class PGFrameWindow : EditorWindow
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

	Rect buttonRect;

	void OnGUI ()
	{
		if (Event.current.type == EventType.Layout && NeedRefresh)
			RefreshFiles ();

		if (Generator == null) {
			Generator = new PGCodeGenerator ();
			Generator.Init ();
		}

		GUILayout.BeginVertical ();
		GUILayout.Label ("PGFrame", EditorStyles.boldLabel);
		if (GUILayout.Button ("刷新")) {
			RefreshFiles ();
		}

		if (GUILayout.Button ("添加 Workspace")) {

			
			PopupWindow.Show (buttonRect, new TextFieldPopupDialog ("请输入 Workspace 的名字:", (string value) => {
				JsonWorkspaceManager manager = new JsonWorkspaceManager (Path.Combine (Application.dataPath, JsonRoot));
				manager.CreateWorkspace (value);
				AssetDatabase.Refresh ();
				NeedRefresh = true;
			}));

			if (Event.current.type == EventType.Repaint)
				buttonRect = GUILayoutUtility.GetLastRect ();
		}

		ApplySelected ();

		if (NeedRefresh == false) {
			if (SelectedJsonElement == null) {
				DesignList ();
			} else {
				switch ((DocType)Enum.Parse (typeof(DocType), SelectedJsonElement.DocType)) {
				case DocType.Element:
					DesignList_Element ();
					break;
				case DocType.SimpleClass:
					DesignList_SimpleClass ();
					break;
				case DocType.Enum:
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
		} else {
			this.Repaint ();
		}

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
	//	XLSXJsonConverter Converter;

	public static readonly string JsonRoot = "PGFrameDesign/JsonData";

	public DirectoryInfo SelectedWorkspace;
	public JSONElement SelectedWorkspaceCommon;
	public DirectoryInfo[] WorkspaceDirectoryInfos;
	public JSONElement SelectedJsonElement;

	bool ShowDesc = false;

	public bool NeedRefresh = true;

	void RefreshFiles ()
	{
		NeedRefresh = false;
		string JsonRootFull = Path.Combine (Application.dataPath, JsonRoot);
		if (SelectedWorkspace == null) {
			DirectoryInfo di = new DirectoryInfo (JsonRootFull);
			WorkspaceDirectoryInfos = di.GetDirectories ();
//			PRDebug.TagLog (lt, lc, JsonConvert.SerializeObject (WorkspaceDirectoryInfos.Select (_ => _.Name).ToList ()));
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
			GUI.Label (rect, "ElementFiles");
		};
		float[] split = new float[]{ 0f, 1f };
		WSJsonFilesList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
			
			Rect r = new Rect (rect);
			r.y -= 1;
			r.height -= 2;
			int split_idx = 0;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]);
			JObject jo_element = ja_elements [index] as JObject;

			string jo_element_filename = jo_element ["File"].Value<string> ();

//			if (Event.current.type == EventType.Layout && AutoSelected.SelectedJsonFileName == jo_element_filename) {
//				SelectedJsonElement = jElements.SingleOrDefault (je => je.FileName == jo_element_filename);
//			}

			if (GUI.Button (r, jo_element_filename)) {
				SelectedJsonElement = jElements.Single (je => je.FileName == jo_element_filename);

				AutoSelected.SelectedJsonFileName = jo_element_filename;
				AutoSelected.Save ();
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
//						SaveCommonFile ();

						CreateElementJsonFile (jsonName, SelectedWorkspaceCommon.Workspace, ElementName);
						NeedRefresh = true;

						PRDebug.TagLog (lt, lc, "成功创建了 Element: " + ElementName);
						ElementName = null;

						SaveCommonFile ();
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
			string jsonName = jo ["File"].Value<string> ();
				
			if (EditorUtility.DisplayDialog ("警告!", string.Format ("确定删除框架中的{0}文件?", jo ["File"].Value<string> ()), "Yes", "No")) {
				ja_elements.RemoveAt (list.index);
				SaveCommonFile ();

				DeleteElementJsonFile (jsonName, SelectedWorkspaceCommon.Workspace);
				NeedRefresh = true;
				ElementName = null;
			}
		};
	}

	void CreateElementJsonFile (string jsonFullName, string workspace, string elementName)
	{
		string targetFileFullPath = Path.Combine (Application.dataPath, JsonRoot);
		targetFileFullPath = Path.Combine (targetFileFullPath, string.Format ("{0}/{1}", workspace, jsonFullName));

		JObject jo = new JObject ();
		jo.Add ("Workspace", workspace);
		jo.Add ("DocType", "Element");

		JObject jo_common = new JObject ();
		jo_common.Add ("Name", elementName);
		jo_common.Add ("Desc", string.Empty);

		jo.Add ("Common", jo_common);
		jo.Add ("Member", new JArray ());
		jo.Add ("Views", new JArray ());

		string json = JsonConvert.SerializeObject (jo, Formatting.Indented);
		File.WriteAllText (targetFileFullPath, json);

		AssetDatabase.Refresh ();

		PRDebug.TagLog (lt, lc, targetFileFullPath + " (Created)");
	}

	void DeleteElementJsonFile (string jsonFullName, string workspace)
	{
		string targetFileFullPath = Path.Combine (Application.dataPath, JsonRoot);
		targetFileFullPath = Path.Combine (targetFileFullPath, string.Format ("{0}/{1}", workspace, jsonFullName));

		File.Delete (targetFileFullPath);
		AssetDatabase.Refresh ();

		PRDebug.TagLog (lt, lc, targetFileFullPath + " (Deleted)");
	}

	string ElementName = null;

	void SaveCommonFile ()
	{
		SelectedWorkspaceCommon.Save ();
	}

	Vector2 JsonFilesScrollPos;

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
					AutoSelected.SelectedWorkspaceName = wdi.Name;
					AutoSelected.Save ();
					SelectedWorkspace = wdi;
					NeedRefresh = true;
				}
			}
			GUILayout.EndScrollView ();

		} else {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("<<")) {
				SelectedWorkspace = null;
				NeedRefresh = true;
				WSJsonFilesList = null;
				SelectedWorkspaceCommon = null;

				AutoSelected.SelectedWorkspaceName = string.Empty;
				AutoSelected.Save ();

				return;
			}
			if (GUILayout.Button ("Save")) {
				SaveCommonFile ();
			}
			GUILayout.EndHorizontal ();

			GUILayout.Label ("Workspace:" + SelectedWorkspace.Name, EditorStyles.boldLabel);

			if (SelectedWorkspaceCommon != null) {
//				PRDebug.TagLog ("PGFrameWindow.Debug", Color.yellow, JsonConvert.SerializeObject (SelectedWorkspaceCommon));
				if (WSJsonFilesList == null)
					ResetReorderableList ();

				JsonFilesScrollPos = GUILayout.BeginScrollView (JsonFilesScrollPos);
				WSJsonFilesList.DoLayoutList ();
				if (ElementName != null) {
					ElementName = GUILayout.TextField (ElementName);
				}
				GUILayout.EndScrollView ();

			}

		}
	}
}
