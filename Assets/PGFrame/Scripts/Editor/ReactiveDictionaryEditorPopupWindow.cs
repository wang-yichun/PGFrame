using UnityEngine;
using UnityEditor;
using System.Collections;
using Newtonsoft.Json;
using UniRx;
using System;
using PogoTools;

public class ReactiveDictionaryEditorPopupWindow<T,U> : PopupWindowContent
{

	public ReactiveDictionaryEditorPopupWindow ()
	{
	}

	public ReactiveDictionaryEditorPopupWindow (IElementEditor parent, ReactiveDictionary<T,U> rd)
	{
		this.parent = parent;
		this.rd = rd;
		PK = default(T);
		PV = default(U);
	}

	IElementEditor parent;

	public ReactiveDictionary<T,U> rd;

	public override Vector2 GetWindowSize ()
	{
		return new Vector2 (600, 800);
	}

	Vector2 scrollViewPos;

	T PK;
	U PV;

	public override void OnGUI (Rect rect)
	{
		scrollViewPos = EditorGUILayout.BeginScrollView (scrollViewPos);
		foreach (T key in rd.Keys) {
			U value = rd [key];

			EditorGUILayout.BeginHorizontal ();

			string tempkJson = JsonConvert.SerializeObject (key);
			string tempkJson2 = EditorGUILayout.DelayedTextField (tempkJson);
			if (tempkJson != tempkJson2) {
				T newKey = JsonConvert.DeserializeObject<T> (tempkJson2);
				rd [newKey] = value;
				rd.Remove (key);
				parent.VMCopyToJson ();
			}

			string tempvJson = JsonConvert.SerializeObject (value);
			string tempvJson2 = EditorGUILayout.DelayedTextField (tempvJson);
			if (tempvJson != tempvJson2) {
				U newValue = JsonConvert.DeserializeObject<U> (tempvJson2);
				rd [key] = newValue;
				parent.VMCopyToJson ();
			}

			if (GUILayout.Button ("-", GUILayout.MaxWidth (20))) {
				rd.Remove (key);
				parent.VMCopyToJson ();
				break;
			}

			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.BeginHorizontal ();
		string PKJson = JsonConvert.SerializeObject (PK);
		PKJson = EditorGUILayout.DelayedTextField (PKJson);
		PK = JsonConvert.DeserializeObject<T> (PKJson);

		string PVJson = JsonConvert.SerializeObject (PV);
		PVJson = EditorGUILayout.DelayedTextField (PVJson);
		PV = JsonConvert.DeserializeObject<U> (PVJson);

		if (GUILayout.Button ("+", GUILayout.MaxWidth (20))) {
			rd.Add (PK, PV);
			parent.VMCopyToJson ();
		}
		EditorGUILayout.EndHorizontal ();
		

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Json")) {
			PRDebug.TagLog ("ReactiveDictionary", new Color (.2f, .2f, 1f), JsonConvert.SerializeObject (rd, Formatting.Indented));
		}
		if (GUILayout.Button ("Clear")) {
			rd.Clear ();
			this.editorWindow.Close ();
			parent.VMCopyToJson ();
			GUI.changed = true;
		}
		if (GUILayout.Button ("Close")) {
			this.editorWindow.Close ();
		}
		EditorGUILayout.EndScrollView ();
	}
}
