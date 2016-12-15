using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace PGFrame
{
	using PogoTools;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public partial class PGFrameWindow : EditorWindow
	{
		void DesignList_SimpleClass ()
		{
			if (SelectedJsonElement == null || SelectedJsonElement.jo ["Common"] ["Desc"] == null)
				return;
	
			JObject jo_common = SelectedJsonElement.jo ["Common"] as JObject;

			if (GUILayout.Button ("<<")) {
				SelectedJsonElement = null;
				SimpleClassMembersList = null;
				NeedRefresh = true;

				AutoSelected.SelectedJsonFileName = string.Empty;
				AutoSelected.Save ();

				return;
			}

			GUILayout.Label (string.Format ("Workspace:{0}, SimpleClass:{1}", SelectedJsonElement.Workspace, SelectedJsonElement.Name), EditorStyles.boldLabel);

			jo_common ["Desc"] = GUILayout.TextArea (jo_common ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle2 ());

			if (jo_common ["Type"] == null) {
				jo_common.Add ("Type", "");
			}

			GUILayout.BeginHorizontal ();
			jo_common ["Type"] = EditorGUILayout.TextField ("Base", jo_common ["Type"].Value<string> ());
			GUILayout.EndHorizontal ();

			DesignList_SimpleClass_Member ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Reset")) {
				SelectedJsonElement.Load ();
				ResetSimpleClassMembersList ();
			}
			if (GUILayout.Button ("Save")) {
				SaveJson ();
			}
			if (GUILayout.Button ("Save&Generate")) {
				SaveJson ();

				Generator.GenerateCode (SelectedJsonElement.jo);

				AssetDatabase.Refresh ();

				NeedRefresh = true;

				this.Repaint ();
			}
			GUILayout.EndHorizontal ();
		}

		ReorderableList SimpleClassMembersList;

		void ResetSimpleClassMembersList ()
		{
			ElementViewTools evtools = new ElementViewTools (SelectedJsonElement.jo);

			JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
			SimpleClassMembersList = new ReorderableList (ja_member, typeof(JToken));
			SimpleClassMembersList.drawHeaderCallback += (Rect rect) => {
				GUI.Label (rect, "Members in SimpleClass");
			};
			float[] split = new float[]{ 0f, .2f, .6f, 1f };
			float[] split_c = new float[]{ 0f, .3f, .6f, .9f, .95f, 1f };

			SimpleClassMembersList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
				JObject jo_member = ja_member [index] as JObject;

				Rect r = new Rect (rect);
				r.y += 2;
				r.height -= 4;
				int split_idx = 0;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

				string default_title = string.Format ("Member.{0}", index);
				string short_desc = jo_member ["Desc"].Value<string> ().Split (new char[]{ '#' }) [0];

				if (string.IsNullOrEmpty (short_desc)) {
					GUI.Label (r, default_title);
				} else {
					GUI.Label (r, short_desc, GUIStyleTemplate.GreenDescStyle ());
				}

				split_idx++;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

				string new_member_name = GUI.TextField (r, jo_member ["Name"].Value<string> ());
				if (new_member_name != jo_member ["Name"].Value<string> ()) {
					jo_member ["Name"] = new_member_name;
				}
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
			};

			SimpleClassMembersList.showDefaultBackground = false;

			SimpleClassMembersList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
				if (Event.current.type == EventType.Repaint) {
					Color color = GUI.backgroundColor;
					if (ShowDesc) {
						if (isActive) {
							GUI.backgroundColor = Color.yellow;
						}
						if (isFocused) {
							GUI.backgroundColor = Color.cyan;
						}
						GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight_SimpleClass (index) - 4f), false, isActive, isFocused, false);
						GUI.backgroundColor = color;
					} else {
						if (isFocused) {
							GUI.backgroundColor = Color.cyan;
							GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight_SimpleClass (index) - 4f), false, isActive, isFocused, false);
							GUI.backgroundColor = color;
						}
					}
				}
			};

			SimpleClassMembersList.elementHeightCallback += (int index) => {
				return CalcHeight_SimpleClass (index);
			};

			SimpleClassMembersList.onAddCallback += (ReorderableList list) => {
				GenericSimpleClassMenuOnAddCallback ();
			};
		}

		void GenericSimpleClassMenuOnAddCallback ()
		{
			JArray ja_members = SelectedJsonElement.jo ["Member"] as JArray;

			string default_name = "DefaultSimpleClass";
			string default_name_with_suffix = default_name;
			int suffix_number = 0;
			while (ja_members.FirstOrDefault (_ => (_ as JObject) ["Name"].Value<string> () == default_name_with_suffix) != null) {
				suffix_number++;
				default_name_with_suffix = default_name + suffix_number;
			}

			JObject jo_member = new JObject ();
			jo_member.Add ("Name", default_name_with_suffix);
			jo_member.Add ("Type", "object");
			jo_member.Add ("Desc", string.Empty);

			ja_members.Add (jo_member);
		}

		float CalcHeight_SimpleClass (int index)
		{
			JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
			JObject jo_member = ja_member [index] as JObject;

			float show_desc_height_amplify = 1f;
			if (ShowDesc) {
				show_desc_height_amplify = 2f;
			}

			return singleRowHeight * show_desc_height_amplify;
		}

		Vector2 SimpleClassMemberScrollPos;

		void DesignList_SimpleClass_Member ()
		{
			if (SelectedJsonElement == null)
				return;

			ShowDesc = GUILayout.Toggle (ShowDesc, "显示描述注释");

			if (SimpleClassMembersList == null)
				ResetSimpleClassMembersList ();
		
			SimpleClassMemberScrollPos = GUILayout.BeginScrollView (SimpleClassMemberScrollPos);
			SimpleClassMembersList.DoLayoutList ();
			GUILayout.EndScrollView ();
		}

	}
}