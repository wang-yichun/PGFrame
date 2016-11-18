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
using System.Linq.Expressions;
using System;

public partial class PGFrameWindow : EditorWindow
{
	public JSONElement SelectedJsonElement;

	static string[] ToolbarHeaders = new string[] { "Member", "View" };
	int toolbar_index = 0;
	bool ShowDesc = false;

	void DesignList_Element ()
	{
		if (SelectedJsonElement == null || SelectedJsonElement.jo ["Common"] ["Desc"] == null)
			return;
	
		JObject jo_common = SelectedJsonElement.jo ["Common"] as JObject;

		GUILayout.Label (string.Format ("Workspace:{0}, Element:{1}", SelectedJsonElement.Workspace, SelectedJsonElement.Name), EditorStyles.boldLabel);

		if (GUILayout.Button ("<<")) {
			SelectedJsonElement = null;
			ElementMembersList = null;
			NeedRefresh = true;
			return;
		}
		ShowDesc = GUILayout.Toggle (ShowDesc, "显示描述注释");


		jo_common ["Desc"] = GUILayout.TextArea (jo_common ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle2 ());

		if (jo_common ["Type"] == null) {
			jo_common.Add ("Type", "");
		}

		GUILayout.BeginHorizontal ();
		jo_common ["Type"] = EditorGUILayout.TextField ("Base", jo_common ["Type"].Value<string> ());
		GUILayout.EndHorizontal ();

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

	float singleRowHeight = 25f;
	float singleRowHeight_c = 20f;

	void ResetElementMembersList ()
	{
		ElementViewTools evtools = new ElementViewTools (SelectedJsonElement.jo);

		JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
		ElementMembersList = new ReorderableList (ja_member, typeof(JToken));
		ElementMembersList.drawHeaderCallback += (Rect rect) => {
			GUI.Label (rect, "Members in Element");
		};
		float[] split = new float[]{ 0f, .2f, .7f, 1f };
		float[] split_c = new float[]{ 0f, .3f, .7f, .9f, .95f, 1f };

		ElementMembersList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
			JObject jo_member = ja_member [index] as JObject;

			Rect r = new Rect (rect);
			r.y += 2;
			r.height -= 4;
			int split_idx = 0;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

			if (!ShowDesc && jo_member ["Desc"].Value<string> ().Contains ("#")) {
				GUI.Label (r, jo_member ["Desc"].Value<string> ().Split (new char[]{ '#' }) [0] + "(" + jo_member ["RxType"].Value<string> ().Substring (0, 3).ToUpper () + ")", GUIStyleTemplate.GreenDescStyle ());
			} else {
				GUI.Label (r, jo_member ["RxType"].Value<string> ());
			}

			split_idx++;
			r.x = (rect.width - 25f) * split [split_idx] + 25f;
			r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

			string new_member_name = GUI.TextField (r, jo_member ["Name"].Value<string> ());
			if (new_member_name != jo_member ["Name"].Value<string> ()) {
				evtools.ChangeName (jo_member ["Name"].Value<string> (), new_member_name);
				jo_member ["Name"] = new_member_name;
			}

			if (jo_member ["RxType"].Value<string> () != "Command") {
				split_idx++;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;
				jo_member ["Type"] = GUI.TextField (r, jo_member ["Type"].Value<string> ());

				if (ShowDesc) {
					r.y = rect.y + singleRowHeight - 6f;
					r.x = (rect.width - 25f) * split [1] + 25f;
					r.width = (rect.width - 25f) * (split [3] - split [1]) - 2f;
					jo_member ["Desc"] = GUI.TextField (r, jo_member ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
				}
			} else {

				split_idx++;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

				JArray ja_command_params = jo_member ["Params"] as JArray;

				if (GUI.Button (r, "+", GUIStyleTemplate.BlueCommandLink ())) {
					if (ja_command_params == null) {
						jo_member.Add ("Params", new JArray ());
						ja_command_params = jo_member ["Params"] as JArray;
					}
					JObject jo_command_param = new JObject ();
					jo_command_param.Add ("Name", string.Format ("Param{0}", ja_command_params.Count));
					jo_command_param.Add ("Type", "object");
					jo_command_param.Add ("Desc", "");
					ja_command_params.Add (jo_command_param);
				}

				if (ShowDesc) {
					r.y = rect.y + singleRowHeight - 6f;
					r.x = (rect.width - 25f) * split [1] + 25f;
					r.width = (rect.width - 25f) * (split [3] - split [1]) - 2f;
					jo_member ["Desc"] = GUI.TextField (r, jo_member ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
				}

				float show_desc_height_amplify = 1f;
				if (ShowDesc) {
					show_desc_height_amplify = 2f;
				}

				if (ja_command_params != null) {
					for (int i = 0; i < ja_command_params.Count; i++) {
						int split_c_idx = 0;
						r.y = rect.y + (singleRowHeight_c * (float)(i + 1)) * show_desc_height_amplify;
						JObject jo_command_param = ja_command_params [i] as JObject;

						split_c_idx = 0;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;

						if (!ShowDesc && jo_command_param ["Desc"].Value<string> ().Contains ("#")) {
							GUI.Label (r, "  - " + jo_command_param ["Desc"].Value<string> ().Split (new char[]{ '#' }) [0] + "(P" + i + ")", GUIStyleTemplate.GreenDescStyle ());
						} else {
							GUI.Label (r, "  - Command Params");
						}

						split_c_idx = 1;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						string new_name = GUI.TextField (r, jo_command_param ["Name"].Value<string> ());
						if (new_name != jo_command_param ["Name"].Value<string> ()) {
							jo_command_param ["Name"] = new_name;
						}

						split_c_idx = 2;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						jo_command_param ["Type"] = GUI.TextField (r, jo_command_param ["Type"].Value<string> ());

						split_c_idx = 3;
						r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
						r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
						if (GUI.Button (r, "-", GUIStyleTemplate.BlueCommandLink ())) {
							ja_command_params.RemoveAt (i);
							return;
						}

						if (i != 0) {
							split_c_idx = 4;
							r.x = (rect.width - 25f) * split_c [split_c_idx] + 25f;
							r.width = (rect.width - 25f) * (split_c [split_c_idx + 1] - split_c [split_c_idx]) - 2f;
							if (GUI.Button (r, "^", GUIStyleTemplate.BlueCommandLink ())) {
								JObject jo0 = ja_command_params [i] as JObject;
								JObject jo1 = ja_command_params [i - 1] as JObject;
								ja_command_params [i] = jo1;
								ja_command_params [i - 1] = jo0;
							}
						}

						if (ShowDesc) {
							r.y = rect.y + (singleRowHeight_c * (float)(i + 1)) * show_desc_height_amplify + singleRowHeight_c - 2f;
							r.x = (rect.width - 25f) * split_c [1] + 25f;
							r.width = (rect.width - 25f) * (split_c [3] - split_c [1]) - 2f;
							jo_command_param ["Desc"] = GUI.TextField (r, jo_command_param ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
						}
					}
				}
			}
		};
		ElementMembersList.elementHeightCallback += (int index) => {
			return CalcHeight (index);
		};
		ElementMembersList.showDefaultBackground = false;

		ElementMembersList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
			if (Event.current.type == EventType.Repaint) {
				Color color = GUI.backgroundColor;
				if (ShowDesc) {
					if (isActive) {
						GUI.backgroundColor = Color.yellow;
					}
					if (isFocused) {
						GUI.backgroundColor = Color.cyan;
					}
					GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight (index) - 4f), false, isActive, isFocused, false);
					GUI.backgroundColor = color;
				} else {
					if (isFocused) {
						GUI.backgroundColor = Color.cyan;
						GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight (index) - 4f), false, isActive, isFocused, false);
						GUI.backgroundColor = color;
					}
				}
			}
		};

		ElementMembersList.onAddCallback += (ReorderableList list) => {
			GenericMenu menu = new GenericMenu ();
			foreach (RxType rt in Enum.GetValues (typeof(RxType))) {  
				menu.AddItem (new GUIContent (rt.ToString ()), false, GenericMenuOnAddCallback, rt);
			}
			menu.ShowAsContext ();
		};
		ElementMembersList.onRemoveCallback += (ReorderableList list) => {
			JObject jo_member = ja_member [list.index] as JObject;
			string element_rxtype = jo_member ["RxType"].Value<string> ();
			string element_name = jo_member ["Name"].Value<string> ();
			if (EditorUtility.DisplayDialog ("警告!", string.Format ("确定删除Element中的一个{0}成员:{1}", element_rxtype, element_name), "Yes", "No")) {
				ja_member.RemoveAt (list.index);
				evtools.DeleteMember (element_name);
			}
		};
	}

	void GenericMenuOnAddCallback (object obj)
	{
		ElementViewTools evtools = new ElementViewTools (SelectedJsonElement.jo);

		JArray ja_members = SelectedJsonElement.jo ["Member"] as JArray;
		JArray ja_views = SelectedJsonElement.jo ["Views"] as JArray;

		RxType selected = (RxType)obj;

		string default_name = string.Format ("Default{0}", selected.ToString ());

		if (ja_members.FirstOrDefault (_ => (_ as JObject) ["Name"].Value<string> () == default_name) != null) {
			PRDebug.TagLog (lt, lcr, string.Format ("已经存在{0},不能创建", default_name));
			return;
		}

		JObject jo_member = new JObject ();
		jo_member.Add ("RxType", selected.ToString ());
		jo_member.Add ("Name", default_name);

		if (selected != RxType.Command) {
			if (selected == RxType.Dictionary) {
				jo_member.Add ("Type", "object,object");
			} else {
				jo_member.Add ("Type", "object");
			}
		}

		jo_member.Add ("Desc", "");
		ja_members.Add (jo_member);

		evtools.CreateDefaultMember (selected, default_name);
	}

	float CalcHeight (int index)
	{
		JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
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
