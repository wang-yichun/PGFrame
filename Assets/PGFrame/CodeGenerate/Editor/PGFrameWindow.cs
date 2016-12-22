using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;

namespace PGFrame
{
	using PogoTools;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public partial class PGFrameWindow : EditorWindow
	{
		public static readonly string lt = "PGFrame";
		public static readonly Color lc = new Color32 (0, 162, 255, 255);
		public static readonly Color lcr = new Color32 (255, 162, 162, 255);

		public static PGFrameWindow Current;

		public Texture2D pgf_window_title_icon;
		public Texture2D pgf_element_icon;
		public Texture2D pgf_simple_class_icon;
		public Texture2D pgf_enum_icon;
		public Texture2D pgf_fsm_icon;
		public Texture2D pgf_workspace_icon;

		public Dictionary<DocType, Texture2D> pgf_doctype_short_icons;
		public Dictionary<TypeType, Texture2D> pgf_typetype_short_icons;

		public void RefreshIcons ()
		{
			if (pgf_doctype_short_icons == null) {
				SetIcons ();
			}
		}

		public void SetIcons ()
		{
			pgf_window_title_icon = Resources.Load<Texture2D> ("pgf_window_title_icon");
			pgf_element_icon = Resources.Load<Texture2D> ("pgf_element_icon");
			pgf_simple_class_icon = Resources.Load<Texture2D> ("pgf_simple_class_icon");
			pgf_enum_icon = Resources.Load<Texture2D> ("pgf_enum_icon");
			pgf_fsm_icon = Resources.Load<Texture2D> ("pgf_fsm_icon");
			pgf_workspace_icon = Resources.Load<Texture2D> ("pgf_workspace_icon");

			pgf_doctype_short_icons = new Dictionary<DocType, Texture2D> ();
			pgf_doctype_short_icons.Add (DocType.Element, Resources.Load<Texture2D> ("pgf_element_short_icon"));
			pgf_doctype_short_icons.Add (DocType.SimpleClass, Resources.Load<Texture2D> ("pgf_simple_class_short_icon"));
			pgf_doctype_short_icons.Add (DocType.Enum, Resources.Load<Texture2D> ("pgf_enum_short_icon"));

			pgf_typetype_short_icons = new Dictionary<TypeType, Texture2D> ();
			pgf_typetype_short_icons.Add (TypeType.System, Resources.Load<Texture2D> ("pgf_system_short_icon"));
			pgf_typetype_short_icons.Add (TypeType.Unity, Resources.Load<Texture2D> ("pgf_unity_short_icon"));
			pgf_typetype_short_icons.Add (TypeType.Other, Resources.Load<Texture2D> ("pgf_other_short_icon"));
		}

		[MenuItem ("PogoRock/PGFrame/PGFrame... %_F1")]
		static void Init ()
		{
			PGFrameWindow window = (PGFrameWindow)EditorWindow.GetWindow (typeof(PGFrameWindow));
			window.SetIcons ();
			window.titleContent = new GUIContent (window.pgf_window_title_icon);
			window.Show ();
			Current = window;
		}

		[MenuItem ("PogoRock/PGFrame/Clear Editor Settings")]
		static void ClearEditorSettings ()
		{
			if (Current != null) {
				Current.SelectedJsonElement = null;

				Current.WSJsonFilesList = null;
				Current.ElementMembersList = null;
				Current.SimpleClassMembersList = null;
				Current.EnumMembersList = null;

				Current.SelectedWorkspace = null;
				Current.SelectedWorkspaceCommon = null;

				Current.NeedRefresh = true;
			}

			AutoSelected.Reset ();
		}

		Rect buttonRect;
		Rect buttonRect2;

		void OnGUI ()
		{
			if (Current == null) {
				Current = this;
				Current.NeedRefresh = true;
				Current.NeedRefreshCommon = true;
				RefreshIcons ();
			}

			if (Event.current.type == EventType.Layout && (NeedRefresh || NeedRefreshCommon))
				RefreshFiles ();

			if (Generator == null) {
				Generator = new PGCodeGenerator ();
				Generator.Init ();
			}

			GUILayout.BeginVertical ();
			GUILayout.Label ("PGFrame", EditorStyles.boldLabel);
			if (GUILayout.Button ("刷新")) {
				NeedRefresh = true;
				NeedRefreshCommon = true;
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
						DesignList_Enum ();
						break;
					case DocType.FSM:
						DesignList ();
						break;
					default:
						throw new ArgumentOutOfRangeException ();
					}
				}
			} else {
				this.Repaint ();
			}

			GUILayout.FlexibleSpace ();
			if (SelectedWorkspace != null && jElements != null) {
				if (GUILayout.Button (string.Format ("发布代码 ({0})", jElements.Length))) {
					for (int i = 0; i < jElements.Length; i++) {
						JSONElement xe = jElements [i];
						Generator.GenerateCode (xe.jo);
					}
					AssetDatabase.Refresh ();
				}
			}

			if (SelectedWorkspace != null && jElements != null) {
				if (GUILayout.Button (string.Format ("删除代码 ({0})", jElements.Length))) {
					if (EditorUtility.DisplayDialog ("警告!", string.Format ("确定删除整个Workspace:{0}的文件,包括非base层代码", SelectedWorkspace.Name), "确定删除", "取消操作")) {
						for (int i = 0; i < jElements.Length; i++) {
							JSONElement xe = jElements [i];
							Generator.DeleteCode (xe.jo);
						}
						AssetDatabase.Refresh ();
					}
				}
			}

			GUILayout.EndVertical ();
		}

		public JSONElement[] jElements;

		public PGCodeGenerator Generator;
		public PGFrameCommonManager CommonManager;

		public static readonly string JsonRoot = "PGFrameDesign/JsonData";

		public DirectoryInfo SelectedWorkspace;
		public JSONElement SelectedWorkspaceCommon;
		public DirectoryInfo[] WorkspaceDirectoryInfos;
		public JSONElement SelectedJsonElement;

		bool ShowDesc = false;

		public bool NeedRefresh = true;
		public bool NeedRefreshCommon = true;

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

			// Common Manager
			if (CommonManager == null) {
				CommonManager = new PGFrameCommonManager (this);
			}

			if (NeedRefreshCommon) {
				NeedRefreshCommon = false;
				CommonManager.Load ();
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

				DocType dt = (DocType)Enum.Parse (typeof(DocType), jo_element ["DocType"].Value<string> ());
				Texture2D icon = null;
				switch (dt) {
				case DocType.Element:
					icon = pgf_element_icon;
					break;
				case DocType.SimpleClass:
					icon = pgf_simple_class_icon;
					break;
				case DocType.Enum:
					icon = pgf_enum_icon;
					break;
				case DocType.FSM:
					icon = pgf_fsm_icon;
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}

				GUIContent content = new GUIContent (jo_element_filename, icon);
				if (GUI.Button (r, content, GUIStyleTemplate.ButtonStyleAlignmentLeft ())) {
					SelectedJsonElement = jElements.Single (je => je.FileName == jo_element_filename);

					if (dt == DocType.FSM) {
						FSMWindow.Init ();
						FSMWindow.Current.jElement = SelectedJsonElement;
					}

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
			if (WorkspaceDirectoryInfos == null || CommonManager.CommonObjectDic == null) {
				NeedRefresh = true;
				return;
			}

//			string JsonRootFull = Path.Combine (Application.dataPath, JsonRoot);
			if (SelectedWorkspace == null) {
				GUILayout.Label ("Root", EditorStyles.boldLabel);
				scrollViewPos = GUILayout.BeginScrollView (scrollViewPos);
				for (int i = 0; i < WorkspaceDirectoryInfos.Length; i++) {
					DirectoryInfo wdi = WorkspaceDirectoryInfos [i];
					string ws_name = wdi.Name;
					StringBuilder sb_content = new StringBuilder (wdi.Name);

					if (CommonManager != null) {
						int file_count = CommonManager.CommonObjectDic [ws_name].ElementFiles.Count;
						sb_content.AppendFormat (" ({0} file{1})", file_count, file_count > 1 ? "s" : "");
//					button_content += " (" + CommonManager.CommonObjectDic [ws_name].ElementFiles.Count + " file)";
					}

					GUIContent button_content = new GUIContent (sb_content.ToString (), pgf_workspace_icon);

					if (GUILayout.Button (button_content, GUIStyleTemplate.ButtonStyleAlignmentLeft ())) {
						AutoSelected.SelectedWorkspaceName = ws_name;
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
							cjf = new EnumJsonFileCreater (this, value);
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
}