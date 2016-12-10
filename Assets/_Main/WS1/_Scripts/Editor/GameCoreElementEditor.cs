using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class GameCoreElementEditor : Editor, IElementEditor
{
	public GameCoreViewModel VM { get; set; }

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
				EditorGUILayout.TextArea (JsonConvert.SerializeObject ((GameCoreViewModelBase)VM, Formatting.Indented));
			}
			EditorGUILayout.Space ();
		}

		if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
			base.OnInspectorGUI ();
			EditorGUILayout.Space ();
		}

		if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - GameCore")) {

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
		EditorGUILayout.TextField (string.Format ("{0} ({1})", "GameCoreViewModel", VM.VMID.ToString ().Substring (0, 8)));
		EditorGUILayout.EndHorizontal ();

		

		string vmk;

		vmk = "GameID";
		string tempGameID = EditorGUILayout.DelayedTextField (vmk, VM.GameID);
		if (tempGameID != VM.GameID) {
			VM.GameID = tempGameID;
		}

		vmk = "MyInfo";
		EditorGUILayout.DelayedTextField (vmk, VM.MyInfo != null ? VM.MyInfo.ToString () : "null (PlayerInfoViewModel)");

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
