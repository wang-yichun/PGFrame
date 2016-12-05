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
			SaveElementJson ();
		}
		if (GUILayout.Button ("Save&Generate")) {
			SaveElementJson ();

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
			// PR_TODO:
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

		SimpleClassMembersList.onAddCallback += (ReorderableList list) => {
			// PR_TODO:
		};
		SimpleClassMembersList.onRemoveCallback += (ReorderableList list) => {
			// PR_TODO:
		};
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
