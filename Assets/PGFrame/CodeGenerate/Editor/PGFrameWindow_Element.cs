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
	public JSONElement SelectedJsonElement;

	static string[] ToolbarHeaders = new string[] { "Member", "View" };
	int toolbar_index = 0;
	bool ShowDesc = false;

	void DesignList_Element ()
	{
		GUILayout.Label (string.Format ("Workspace:{0}, Element:{1}", SelectedJsonElement.Workspace, SelectedJsonElement.Name), EditorStyles.boldLabel);

		if (GUILayout.Button ("<<")) {
			SelectedJsonElement = null;
			ElementMembersList = null;
			NeedRefresh = true;
		}
		ShowDesc = GUILayout.Toggle (ShowDesc, "显示描述注释");
		toolbar_index = GUILayout.Toolbar (toolbar_index, ToolbarHeaders, new GUILayoutOption[]{ GUILayout.Height (25f) });

		switch (toolbar_index) {
		case 0:
			DesignList_Element_Member ();
			break;
		case 1:
			DesignList_Element_View ();
			break;
		default:
			break;
		}
	}

	ReorderableList ElementMembersList;

	void ResetElementMembersList ()
	{
		JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
		ElementMembersList = new ReorderableList (ja_member, typeof(JToken));
		ElementMembersList.drawHeaderCallback += (Rect rect) => {
			GUI.Label (rect, "Members in Element");
		};
		float[] split = new float[]{ 0f, .2f, .7f, 1f };
		float[] split_c = new float[]{ 0f, .3f, .7f, 1f };
		float singleRowHeight = 25f;
		float singleRowHeight_c = 20f;
		ElementMembersList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
			JObject jo_member = ja_member [index] as JObject;

			Rect r = new Rect (rect);
			r.height -= 4;
			int split_idx = 0;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

			GUI.Label (r, jo_member ["RxType"].Value<string> ());

			split_idx++;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

			jo_member ["Name"] = GUI.TextField (r, jo_member ["Name"].Value<string> ());


			if (jo_member ["RxType"].Value<string> () != "Command") {
				split_idx++;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;
				jo_member ["Type"] = GUI.TextField (r, jo_member ["Type"].Value<string> ());

				if (ShowDesc) {
					r.y = rect.y + singleRowHeight - 4f;
					r.x = (rect.width - 25f) * split [1] + 25f;
					r.width = (rect.width - 25f) * (split [3] - split [1]) - 2f;
					jo_member ["Desc"] = GUI.TextField (r, jo_member ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
				}
			} else {
				
				if (ShowDesc) {
					r.y = rect.y + singleRowHeight - 4f;
					r.x = (rect.width - 25f) * split [1] + 25f;
					r.width = (rect.width - 25f) * (split [3] - split [1]) - 2f;
					jo_member ["Desc"] = GUI.TextField (r, jo_member ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
				}

				float show_desc_height_amplify = 1f;
				if (ShowDesc) {
					show_desc_height_amplify = 2f;
				}
				JArray ja_command_params = jo_member ["Params"] as JArray;
				if (ja_command_params != null) {
					for (int i = 0; i < ja_command_params.Count; i++) {
						int split_c_idx = 0;
						r.y = rect.y + (singleRowHeight_c * (float)(i + 1)) * show_desc_height_amplify;
						JObject jo_command_param = ja_command_params [i] as JObject;

						split_c_idx = 0;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						GUI.Label (r, "  - Command Params");

						split_c_idx = 1;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						jo_command_param ["Name"] = GUI.TextField (r, jo_command_param ["Name"].Value<string> ());

						split_c_idx = 2;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						jo_command_param ["Type"] = GUI.TextField (r, jo_command_param ["Type"].Value<string> ());
					}
				}
			}
		};
		ElementMembersList.elementHeightCallback += (int index) => {
			JObject jo_member = ja_member [index] as JObject;

			float show_desc_height_amplify = 1f;
			if (ShowDesc) {
				show_desc_height_amplify = 2f;
			}

			if (jo_member ["RxType"].Value<string> () == "Command") {
				JArray jo_command_params = jo_member ["Params"] as JArray;
				if (jo_command_params == null)
					return singleRowHeight * show_desc_height_amplify;
				return ((jo_command_params.Count + 1) * singleRowHeight_c) * show_desc_height_amplify + 4f;
			}
			return singleRowHeight * show_desc_height_amplify;
		};
	}

	Vector2 ElementMemberScrollPos;

	void DesignList_Element_Member ()
	{
		if (SelectedJsonElement == null)
			return;
		
		if (ElementMembersList == null)
			ResetElementMembersList ();
		
		ElementMemberScrollPos = GUILayout.BeginScrollView (ElementMemberScrollPos);
		ElementMembersList.DoLayoutList ();
		GUILayout.EndScrollView ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Reset")) {
			SelectedJsonElement.Load ();
			ResetElementMembersList ();
		}
		if (GUILayout.Button ("Save")) {
			SaveElementJson ();
		}
		GUILayout.EndHorizontal ();
	}

	void DesignList_Element_View ()
	{
	}

	void SaveElementJson ()
	{
		SelectedJsonElement.Save ();
	}
}
