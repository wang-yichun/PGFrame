using UnityEngine;
using UnityEditor;
using System.Collections;
using Newtonsoft.Json;
using UniRx;
using System;

public class ReactiveCollectionEditorPopupWindow<T> : PopupWindowContent
{

	public ReactiveCollectionEditorPopupWindow ()
	{
	}

	public ReactiveCollectionEditorPopupWindow (ReactiveCollection<T> rc)
	{
		this.rc = rc;
	}

	public ReactiveCollection<T> rc;

	public override Vector2 GetWindowSize ()
	{
		return new Vector2 (200, 150);
	}

	Vector2 scrollViewPos;

	public override void OnGUI (Rect rect)
	{
		scrollViewPos = EditorGUILayout.BeginScrollView (scrollViewPos);
		for (int i = 0; i < rc.Count; i++) {
			T e = rc [i];
			string tempJson = JsonConvert.SerializeObject (e);
			string tempJson2 = EditorGUILayout.DelayedTextField (tempJson);
			if (tempJson != tempJson2) {
				rc [i] = JsonConvert.DeserializeObject<T> (tempJson2);
			}
		}

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Close")) {
			this.editorWindow.Close ();
		}
		EditorGUILayout.EndScrollView ();
	}
}
