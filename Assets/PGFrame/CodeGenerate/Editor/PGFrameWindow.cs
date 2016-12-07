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
	Rect buttonRect2;

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

	public JSONElement[] jElements;

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

	DocType? NeedShowPopupWindowDocType;

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

			GenericMenu menu = new GenericMenu ();
			foreach (DocType dt in Enum.GetValues (typeof(DocType))) {  
				menu.AddItem (new GUIContent (dt.ToString ()), false, (object userData) => {
					NeedShowPopupWindowDocType = (DocType)userData;
				}, dt);
			}
			menu.ShowAsContext ();
		};

		WSJsonFilesList.onRemoveCallback += (ReorderableList list) => {
			JObject jo = ja_elements [list.index] as JObject;
			string jsonName = jo ["File"].Value<string> ();
				
			if (EditorUtility.DisplayDialog ("警告!", string.Format ("确定删除框架中的{0}文件?", jo ["File"].Value<string> ()), "Yes", "No")) {
				ja_elements.RemoveAt (list.index);
				SelectedWorkspaceCommon.jo ["ElementFiles"] = ja_elements;
				SaveCommonFile ();

				DeleteElementJsonFile (jsonName, SelectedWorkspaceCommon.Workspace);
				NeedRefresh = true;
			}
		};
	}

	void DeleteElementJsonFile (string jsonFullName, string workspace)
	{
		string targetFileFullPath = Path.Combine (Application.dataPath, JsonRoot);
		targetFileFullPath = Path.Combine (targetFileFullPath, string.Format ("{0}/{1}", workspace, jsonFullName));

		File.Delete (targetFileFullPath);
		AssetDatabase.Refresh ();

		PRDebug.TagLog (lt, lc, targetFileFullPath + " (Deleted)");
	}

	public void SaveCommonFile ()
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

				GUILayout.EndScrollView ();
				
				TryShowPopupWindowDoc ();

			}

		}
	}

	void TryShowPopupWindowDoc ()
	{
		if (Event.current.type == EventType.Repaint && NeedShowPopupWindowDocType != null) {
			DocType selected = NeedShowPopupWindowDocType.Value;

			string tip = string.Format ("请输入 {0} 的名字:", selected.ToString ());

			NeedShowPopupWindowDocType = null;

			PopupWindow.Show (buttonRect, new TextFieldPopupDialog (tip, (string value) => {

				if (string.IsNullOrEmpty (value) == false) {
					JsonFileCreater cjf = null;
					switch (selected) {
					case DocType.Element:
						cjf = new ElementJsonFileCreater (this, value);
						break;
					case DocType.SimpleClass:
						cjf = new SimpleClassJsonFileCreater (this, value);
						break;
					case DocType.Enum:
						break;
					default:
						throw new ArgumentOutOfRangeException ();
					}
					if (cjf != null) {
						cjf.Create ();
						WSJsonFilesList = null;
					}
				} else {
					PRDebug.TagLog (lt, lcr, "请输入名字!");
				}
			}));

			if (Event.current.type == EventType.Repaint)
				buttonRect = GUILayoutUtility.GetLastRect ();
		}
	}
		
	void SaveJson ()
	{
		SelectedJsonElement.Save ();
	}
}
