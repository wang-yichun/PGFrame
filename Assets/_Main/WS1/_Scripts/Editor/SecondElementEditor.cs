using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

public class SecondElementEditor : Editor, IElementEditor
{
	public SecondViewModel VM { get; set; }

	bool ToggleDefault = true;
	bool ToggleViewModel = true;

	public Dictionary<string, string> CommandParams { get; set; }

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginVertical ();
		if (ToggleDefault = EditorGUILayout.Foldout (ToggleDefault, "Default")) {
			EditorGUI.indentLevel++;
			base.OnInspectorGUI ();
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();
		}
		if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel")) {

			if (VM != null) {
				InspectorGUI_ViewModel ();
			} else {
				EditorGUILayout.HelpBox ("没有绑定 ViewModel", MessageType.Warning);
			}

			EditorGUILayout.Space ();
		}

		EditorGUILayout.EndVertical ();
	}

	public void InspectorGUI_ViewModel ()
	{
		EditorGUI.indentLevel++;
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("VMID");
		EditorGUILayout.SelectableLabel (VM.VMID.ToString ());
		EditorGUILayout.EndHorizontal ();

		string vmk;

		vmk = "IntValue";
		int tempIntValue = EditorGUILayout.DelayedIntField (vmk, VM.IntValue);
		if (tempIntValue != VM.IntValue) {
			VM.IntValue = tempIntValue;
		}
		vmk = "LongValue";
		int tempLongValue = EditorGUILayout.DelayedIntField (vmk, (int)VM.LongValue);
		if ((long)tempLongValue != VM.LongValue) {
			VM.LongValue = (long)tempLongValue;
		}
		vmk = "FloatValue";
		float tempFloatValue = EditorGUILayout.DelayedFloatField (vmk, VM.FloatValue);
		if (tempFloatValue != VM.FloatValue) {
			VM.FloatValue = tempFloatValue;
		}
		vmk = "DoubleValue";
		double tempDoubleValue = EditorGUILayout.DelayedDoubleField (vmk, VM.DoubleValue);
		if (tempDoubleValue != VM.DoubleValue) {
			VM.DoubleValue = tempDoubleValue;
		}

		EditorGUILayout.EndVertical ();
		EditorGUI.indentLevel--;

		if (EditorApplication.isPlaying == false) {
			if (GUI.changed) {
				VMCopyToJson ();
			}
		}
	}

	#region IElementEditor implementation

	public virtual void VMCopyToJson ()
	{
	}

	#endregion
}
