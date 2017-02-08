using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PGFrame
{
	using PogoTools;

	public static class PGFrameOpenFileTools
	{
		public static void OpenContentMenu (JSONElement jsonElement)
		{
			string workspace = jsonElement.Workspace;
			string docType = jsonElement.DocType;
			string name = jsonElement.Name;

			Debug.LogFormat ("OpenContentMenu: {0} | {1} | {2}", workspace, docType, name);


			GenericMenu menu = new GenericMenu ();

			if (docType == "Element") {

				JArray ja_views = jsonElement.jo ["Views"] as JArray;
				for (int i = 0; i < ja_views.Count; i++) {
					JObject jo_view = ja_views [i] as JObject;
					string viewName = jo_view ["Name"].Value<string> ();

					menu.AddItem (new GUIContent (string.Format ("{0}.cs", viewName)), false, () => {
						string file = string.Format ("Assets/_Main/{0}/_Scripts/View/{1}.cs", workspace, viewName);
						UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
					});
				}

				menu.AddItem (new GUIContent (string.Format ("{0}Controller.cs", name)), false, () => {
					string file = string.Format ("Assets/_Main/{0}/_Scripts/Controller/{1}Controller.cs", workspace, name);
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
				});
				menu.AddItem (new GUIContent (string.Format ("{0}ViewModel.cs", name)), false, () => {
					string file = string.Format ("Assets/_Main/{0}/_Scripts/ViewModel/{1}ViewModel.cs", workspace, name);
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
				});

			} else if (docType == "SimpleClass") {
				menu.AddItem (new GUIContent (string.Format ("{0}.cs", name)), false, () => {
					string file = string.Format ("Assets/_Main/{0}/_Scripts/SimpleClass/{1}.cs", workspace, name);
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
				});
			} else if (docType == "Enum") {
				menu.AddItem (new GUIContent (string.Format ("{0}.cs", name)), false, () => {
					string file = string.Format ("Assets/_Main/{0}/_Scripts/Enum/{1}.cs", workspace, name);
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
				});
			}

			menu.AddSeparator ("");
			menu.AddItem (new GUIContent (string.Format ("{0}.{1}.{2}.json", workspace, docType, name)), false, () => {
				string file = string.Format ("Assets/PGFrameDesign/JsonData/{0}/{0}.{1}.{2}.json", workspace, docType, name);
				UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (file, 1);
			});

			menu.ShowAsContext ();
		}
	}
}