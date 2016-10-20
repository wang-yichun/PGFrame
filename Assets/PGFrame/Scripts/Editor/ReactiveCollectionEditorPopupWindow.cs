using UnityEngine;
using UnityEditor;
using System.Collections;
using Newtonsoft.Json;
using UniRx;
using System;
using PogoTools;

public class ReactiveCollectionEditorPopupWindow<T> : PopupWindowContent
{

	public ReactiveCollectionEditorPopupWindow ()
	{
	}

	public ReactiveCollectionEditorPopupWindow (Editor parent, ReactiveCollection<T> rc)
	{
		this.parent = parent;
		this.rc = rc;
		PE = default(T);
	}

	Editor parent;

	public ReactiveCollection<T> rc;

	public override Vector2 GetWindowSize ()
	{
		return new Vector2 (400, 800);
	}

	Vector2 scrollViewPos;

	T PE;

	public override void OnGUI (Rect rect)
	{
		scrollViewPos = EditorGUILayout.BeginScrollView (scrollViewPos);
		for (int i = 0; i < rc.Count; i++) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (i.ToString (), GUILayout.MaxWidth (30));
			T e = rc [i];
			string tempJson = JsonConvert.SerializeObject (e);
			string tempJson2 = EditorGUILayout.DelayedTextField (tempJson);
			if (tempJson != tempJson2) {
				rc [i] = JsonConvert.DeserializeObject<T> (tempJson2);
				(parent as IElementEditor).VMCopyToJson ();
			}
			if (GUILayout.Button ("-", GUILayout.MaxWidth (20))) {
				rc.RemoveAt (i);
				(parent as IElementEditor).VMCopyToJson ();
				break;
			}
			if (GUILayout.Button ("+", GUILayout.MaxWidth (20))) {
				rc.Insert (i, PE);
				(parent as IElementEditor).VMCopyToJson ();
				break;
			}
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("NEW", GUILayout.MaxWidth (30));
		string PEJson = JsonConvert.SerializeObject (PE);
		PEJson = EditorGUILayout.DelayedTextField (PEJson);
		PE = JsonConvert.DeserializeObject<T> (PEJson);
		if (GUILayout.Button ("+", GUILayout.MaxWidth (40))) {
			rc.Add (PE);
			(parent as IElementEditor).VMCopyToJson ();
		}
		EditorGUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Json")) {
			PRDebug.TagLog ("ReactiveDictionary", new Color (.2f, .2f, 1f), JsonConvert.SerializeObject (rc, Formatting.Indented));
		}
		if (GUILayout.Button ("Close")) {
			this.editorWindow.Close ();
		}
		EditorGUILayout.EndScrollView ();
	}
}
