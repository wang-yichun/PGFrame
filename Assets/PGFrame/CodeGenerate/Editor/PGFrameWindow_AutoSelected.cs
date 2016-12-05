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

public partial class PGFrameWindow : EditorWindow
{
	[InitializeOnLoad]
	public static class AutoSelected
	{
		public static string SelectedWorkspaceName;
		public static string SelectedJsonFileName;

		static AutoSelected ()
		{
			Load ();
		}

		public static void Save ()
		{
			EditorPrefs.SetString ("PGF_SelectedWorkspaceName", SelectedWorkspaceName);
			EditorPrefs.SetString ("PGF_SelectedJsonFileName", SelectedJsonFileName);
		}

		public static void Load ()
		{
			SelectedWorkspaceName = EditorPrefs.GetString ("PGF_SelectedWorkspaceName", string.Empty);
			SelectedJsonFileName = EditorPrefs.GetString ("PGF_SelectedJsonFileName", string.Empty);
		}
	}

	public void ApplySelected ()
	{
		if (WorkspaceDirectoryInfos != null) {
			for (int i = 0; i < WorkspaceDirectoryInfos.Length; i++) {
				DirectoryInfo wdi = WorkspaceDirectoryInfos [i];

				// 自动选择
				if (AutoSelected.SelectedWorkspaceName == wdi.Name) {
					if (SelectedWorkspace != wdi) {
						SelectedWorkspace = wdi;
						NeedRefresh = true;
					}
				}
			}
		}

		if (SelectedWorkspaceCommon != null) {
			JArray ja_elements = SelectedWorkspaceCommon.jo ["ElementFiles"] as JArray;
			for (int i = 0; i < ja_elements.Count; i++) {
				JObject jo_element = ja_elements [i] as JObject;
				string jo_element_filename = jo_element ["File"].Value<string> ();
				if (AutoSelected.SelectedJsonFileName == jo_element_filename) {

					var tempSelectedJsonElement = jElements.Single (je => je.FileName == jo_element_filename);
					if (SelectedJsonElement == null || SelectedJsonElement.Workspace != tempSelectedJsonElement.Workspace) {
						SelectedJsonElement = tempSelectedJsonElement;
						NeedRefresh = true;
					}
				}
			}
		}
	}
}
