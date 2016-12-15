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
		void DesignList_Enum ()
		{
			if (SelectedJsonElement == null || SelectedJsonElement.jo ["Common"] ["Desc"] == null)
				return;
	
			JObject jo_common = SelectedJsonElement.jo ["Common"] as JObject;

			if (GUILayout.Button ("<<")) {
				SelectedJsonElement = null;
				EnumMembersList = null;
				NeedRefresh = true;

				AutoSelected.SelectedJsonFileName = string.Empty;
				AutoSelected.Save ();

				return;
			}

			GUILayout.Label (string.Format ("Workspace:{0}, Enum:{1}", SelectedJsonElement.Workspace, SelectedJsonElement.Name), EditorStyles.boldLabel);

			jo_common ["Desc"] = GUILayout.TextArea (jo_common ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle2 ());

			DesignList_Enum_Member ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Reset")) {
				SelectedJsonElement.Load ();
				ResetEnumMembersList ();
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

		ReorderableList EnumMembersList;

		void ResetEnumMembersList ()
		{
			JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
			EnumMembersList = new ReorderableList (ja_member, typeof(JToken));
			EnumMembersList.drawHeaderCallback += (Rect rect) => {
				GUI.Label (rect, "Items in Enum");
			};
			float[] split = new float[]{ 0f, .2f, .6f, 1f };
			float[] split_c = new float[]{ 0f, .3f, .6f, .9f, .95f, 1f };

			EnumMembersList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
				JObject jo_member = ja_member [index] as JObject;

				Rect r = new Rect (rect);
				r.y += 2;
				r.height -= 4;
				int split_idx = 0;
				r.x = (rect.width - 25f) * split [split_idx] + 25f;
				r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]) - 2f;

				string default_title = string.Format ("Item.{0}", index);
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
				jo_member ["Int"] = EditorGUI.IntField (r, jo_member ["Int"].Value<int> ());

				if (ShowDesc) {
					r.y = rect.y + singleRowHeight - 6f;
					r.x = (rect.width - 25f) * split [1] + 25f;
					r.width = (rect.width - 25f) * (split [3] - split [1]) - 2f;
					jo_member ["Desc"] = GUI.TextField (r, jo_member ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
				}
			};

			EnumMembersList.showDefaultBackground = false;

			EnumMembersList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) => {
				if (Event.current.type == EventType.Repaint) {
					Color color = GUI.backgroundColor;
					if (ShowDesc) {
						if (isActive) {
							GUI.backgroundColor = Color.yellow;
						}
						if (isFocused) {
							GUI.backgroundColor = Color.cyan;
						}
						GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight_Enum (index) - 4f), false, isActive, isFocused, false);
						GUI.backgroundColor = color;
					} else {
						if (isFocused) {
							GUI.backgroundColor = Color.cyan;
							GUI.skin.box.Draw (new Rect (rect.x + 2f, rect.y, rect.width - 4f, CalcHeight_Enum (index) - 4f), false, isActive, isFocused, false);
							GUI.backgroundColor = color;
						}
					}
				}
			};

			EnumMembersList.elementHeightCallback += (int index) => {
				return CalcHeight_Enum (index);
			};

			EnumMembersList.onAddCallback += (ReorderableList list) => {
				GenericEnumMenuOnAddCallback ();
			};
//		EnumMembersList.onRemoveCallback += (ReorderableList list) => {
//			// PR_TODO:
//		};
		}

		void GenericEnumMenuOnAddCallback ()
		{
			JArray ja_members = SelectedJsonElement.jo ["Member"] as JArray;

			string default_name = "DefaultEnum";
			string default_name_with_suffix = default_name;
			int suffix_number = 0;
			while (ja_members.FirstOrDefault (_ => (_ as JObject) ["Name"].Value<string> () == default_name_with_suffix) != null) {
				suffix_number++;
				default_name_with_suffix = default_name + suffix_number;
			}


			int default_int = 0;
			if (ja_members.FirstOrDefault (_ => (_ as JObject) ["Int"].Value<int> () < 0) != null) {
				default_int = -1;
			} else {
				default_int = 0;
				while (ja_members.FirstOrDefault (_ => (_ as JObject) ["Int"].Value<int> () == default_int) != null) {
					default_int++;
				}
			}

			JObject jo_member = new JObject ();
			jo_member.Add ("Name", default_name_with_suffix);
			jo_member.Add ("Int", default_int);
			jo_member.Add ("Desc", string.Empty);

			ja_members.Add (jo_member);
		}

		float CalcHeight_Enum (int index)
		{
			JArray ja_member = SelectedJsonElement.jo ["Member"] as JArray;
			JObject jo_member = ja_member [index] as JObject;

			float show_desc_height_amplify = 1f;
			if (ShowDesc) {
				show_desc_height_amplify = 2f;
			}

			return singleRowHeight * show_desc_height_amplify;
		}

		Vector2 EnumMemberScrollPos;

		void DesignList_Enum_Member ()
		{
			if (SelectedJsonElement == null)
				return;

			ShowDesc = GUILayout.Toggle (ShowDesc, "显示描述注释");

			if (EnumMembersList == null)
				ResetEnumMembersList ();
		
			EnumMemberScrollPos = GUILayout.BeginScrollView (EnumMemberScrollPos);
			EnumMembersList.DoLayoutList ();
			GUILayout.EndScrollView ();
		}

	}
}