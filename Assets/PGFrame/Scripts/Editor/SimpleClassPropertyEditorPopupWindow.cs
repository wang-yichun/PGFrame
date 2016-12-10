using UnityEngine;
using UnityEditor;
using System.Collections;
using Newtonsoft.Json;
using UniRx;
using System;
using PogoTools;
using Newtonsoft.Json.Linq;

public class SimpleClassPropertyEditorPopupWindow<T> : PopupWindowContent
{

	public SimpleClassPropertyEditorPopupWindow ()
	{
	}

	public SimpleClassPropertyEditorPopupWindow (Editor parent, ReactiveProperty<T> rp)
	{
		this.parent = (IElementEditor)parent;
		this.rp = rp;
		jsonStr = JsonConvert.SerializeObject (rp.Value, Formatting.Indented);
		jsonStr_ori = jsonStr;
		PE = default(T);
	}

	IElementEditor parent;

	public ReactiveProperty<T> rp;
	public string jsonStr_ori;
	public string jsonStr;

	public override Vector2 GetWindowSize ()
	{
		return new Vector2 (400, 800);
	}

	Vector2 scrollViewPos;

	T PE;

	public override void OnGUI (Rect rect)
	{
		scrollViewPos = EditorGUILayout.BeginScrollView (scrollViewPos);

		jsonStr = EditorGUILayout.TextArea (jsonStr);

		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("OK")) {
			try {
				rp.Value = JsonConvert.DeserializeObject<T> (jsonStr);
				this.editorWindow.Close ();
			} catch {
				if (EditorUtility.DisplayDialog ("警告!", "输入的 Json 编码有误,重置到最初?", "重置", "取消")) {
					rp.Value = JsonConvert.DeserializeObject<T> (jsonStr_ori);
				}
			}
			parent.VMCopyToJson ();
		}
		if (GUILayout.Button ("Set Null")) {
			rp.Value = default(T);
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
