using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[CustomEditor (typeof(FirstView))]
public class FirstElementEditor : Editor
{
	FirstView V;

	void OnEnable ()
	{
		V = (FirstView)target;

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

		string vmk;

		vmk = "LabelTextNum";
		int tempLabelTextNum = EditorGUILayout.DelayedIntField (vmk, V.VM.LabelTextNum);
		if (tempLabelTextNum != V.VM.LabelTextNum) {
			V.VM.LabelTextNum = tempLabelTextNum;
		}

		vmk = "AddNum";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Params")) {
			if (CommandParams.ContainsKey (vmk)) {
				CommandParams.Remove (vmk);
			} else {
				CommandParams [vmk] = JsonConvert.SerializeObject (new AddNumCommand (), Formatting.Indented);
			}
		}
		if (GUILayout.Button ("Invoke")) {
			if (CommandParams.ContainsKey (vmk) == false) {
				V.VM.RCMD_AddNum.Execute (new AddNumCommand () { Sender = V.VM });
			} else {
				AddNumCommand command = JsonConvert.DeserializeObject<AddNumCommand> (CommandParams [vmk]);
				command.Sender = V.VM;
				V.VM.RCMD_AddNum.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "Numbers";
		EditorGUILayout.BeginHorizontal ();
		string numbersJson = JsonConvert.SerializeObject (V.VM.Numbers);
		string tempNumbersJson = EditorGUILayout.DelayedTextField (vmk, numbersJson);
		if (tempNumbersJson != numbersJson) {
			if (string.IsNullOrEmpty (tempNumbersJson)) {
				V.VM.Numbers = null;
			} else {
				V.VM.Numbers = JsonConvert.DeserializeObject<List<int>> (tempNumbersJson);
			}
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
		EditorGUI.indentLevel--;

		if (EditorApplication.isPlaying == false) {
			if (GUI.changed) {
				V.ViewModelInitValueJson = JsonConvert.SerializeObject ((FirstViewModelBase)V.VM);
			}
		}
	}
}
