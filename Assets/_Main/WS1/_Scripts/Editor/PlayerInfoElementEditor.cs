using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class PlayerInfoElementEditor : Editor, IElementEditor
{
	public PlayerInfoViewModel VM { get; set; }

	bool ToggleSettings = true;
	bool ToggleVMJsonOn = false;
	bool ToggleView = true;
	bool ToggleViewModel = true;

	public Dictionary<string, string> CommandParams { get; set; }

	public override void OnInspectorGUI ()
	{
		ViewBase V = target as ViewBase;

		EditorGUILayout.BeginVertical ();
		if (ToggleSettings = EditorGUILayout.Foldout (ToggleSettings, "Settings")) {
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Log VMJson")) {
				Debug.Log (V.ViewModelInitValueJson);
			}
			if (GUILayout.Button ("Clear VM & InitValueJson")) {
				VM = null;
				V = target as ViewBase;
				V.ViewModelInitValueJson = string.Empty;
			}
			EditorGUILayout.EndHorizontal ();
			V.AutoCreateViewModel = EditorGUILayout.ToggleLeft ("Auto Create ViewModel", V.AutoCreateViewModel);
			if (ToggleVMJsonOn = EditorGUILayout.ToggleLeft (string.Format ("Show VM Json (Length:{0})", V.VMJsonSize), ToggleVMJsonOn)) {
				EditorGUILayout.TextArea (JsonConvert.SerializeObject ((PlayerInfoViewModelBase)VM, Formatting.Indented));
			}
			EditorGUILayout.Space ();
		}

		if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
			base.OnInspectorGUI ();
			EditorGUILayout.Space ();
		}

		if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - PlayerInfo")) {

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
		EditorGUILayout.PrefixLabel ("Name & ID");
		EditorGUILayout.TextField (string.Format ("{0} ({1})", "PlayerInfoViewModel", VM.VMID.ToString ().Substring (0, 8)));
		EditorGUILayout.EndHorizontal ();

		

		string vmk;

		vmk = "Name";
		string tempName = EditorGUILayout.DelayedTextField (vmk, VM.Name);
		if (tempName != VM.Name) {
			VM.Name = tempName;
		}

		vmk = "Score";
		int tempScore = EditorGUILayout.DelayedIntField (vmk, VM.Score);
		if (tempScore != VM.Score) {
			VM.Score = tempScore;
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
