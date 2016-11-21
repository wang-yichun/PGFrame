using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(SecondView))]
public class SecondElementEditor : Editor, IElementEditor
{
	SecondView V;

	void OnEnable ()
	{
		V = (SecondView)target;

		if (EditorApplication.isPlaying == false) {
			V.CreateViewModel ();
		}

		CommandParams = new Dictionary<string, string> ();
	}

	bool ToggleDefault = true;
	bool ToggleViewModel = true;

	Dictionary<string, string> CommandParams;

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

			if (V.VM != null) {
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
		EditorGUILayout.SelectableLabel (V.VM.VMID.ToString ());
		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.EndVertical ();
		EditorGUI.indentLevel--;

		if (EditorApplication.isPlaying == false) {
			if (GUI.changed) {
				VMCopyToJson ();
			}
		}
	}

	#region IElementEditor implementation

	public void VMCopyToJson ()
	{
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((SecondViewModelBase)V.VM);
	}

	#endregion
}
