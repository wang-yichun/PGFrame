using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

public class FirstElementEditor : Editor, IElementEditor
{
	public FirstViewModel VM { get; set; }

	bool ToggleDefault = true;
	bool ToggleViewModel = true;

	public Dictionary<string, string> CommandParams { get; set; }

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginVertical ();
		if (ToggleDefault = EditorGUILayout.Foldout (ToggleDefault, "View")) {
			EditorGUI.indentLevel++;
			base.OnInspectorGUI ();
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();
		}
		if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - First")) {

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
		EditorGUILayout.TextField (string.Format ("{0} ({1})", "FirstViewModel", VM.VMID.ToString ().Substring (0, 8)));
		EditorGUILayout.EndHorizontal ();

		
		EditorGUILayout.BeginVertical ("box");
		FBElementEditor baseElementEditor = new FBElementEditor ();
		baseElementEditor.VM = VM as FBViewModel;
		baseElementEditor.InspectorGUI_ViewModel ();
		EditorGUILayout.EndVertical ();

		string vmk;

		vmk = "LabelTextNum";
		int tempLabelTextNum = EditorGUILayout.DelayedIntField (vmk, VM.LabelTextNum);
		if (tempLabelTextNum != VM.LabelTextNum) {
			VM.LabelTextNum = tempLabelTextNum;
		}

		vmk = "Numbers";
		EditorGUILayout.BeginHorizontal ();
		string NumbersJson = JsonConvert.SerializeObject (VM.Numbers);
		string tempNumbersJson = EditorGUILayout.DelayedTextField (vmk, NumbersJson);
		if (tempNumbersJson != NumbersJson) {
			if (string.IsNullOrEmpty (tempNumbersJson)) {
				VM.Numbers = null;
			} else {
				VM.Numbers = JsonConvert.DeserializeObject<ReactiveCollection<int>> (tempNumbersJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<int> (this, VM.Numbers)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "MyDictionary";
		EditorGUILayout.BeginHorizontal ();
		string MyDictionary = JsonConvert.SerializeObject (VM.MyDictionary);
		string tempMyDictionary = EditorGUILayout.DelayedTextField (vmk, MyDictionary);
		if (tempMyDictionary != MyDictionary) {
			if (string.IsNullOrEmpty (tempMyDictionary)) {
				VM.MyDictionary = null;
			} else {
				VM.MyDictionary = JsonConvert.DeserializeObject<ReactiveDictionary<string,string>> (tempMyDictionary);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<string,string> (this, VM.MyDictionary)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "MyNewProperty";
		EditorGUILayout.DelayedTextField (vmk, VM.MyNewProperty != null ? VM.MyNewProperty.ToString () : "null (object)");

		vmk = "DefaultCollection";
		EditorGUILayout.BeginHorizontal ();
		string DefaultCollectionJson = JsonConvert.SerializeObject (VM.DefaultCollection);
		string tempDefaultCollectionJson = EditorGUILayout.DelayedTextField (vmk, DefaultCollectionJson);
		if (tempDefaultCollectionJson != DefaultCollectionJson) {
			if (string.IsNullOrEmpty (tempDefaultCollectionJson)) {
				VM.DefaultCollection = null;
			} else {
				VM.DefaultCollection = JsonConvert.DeserializeObject<ReactiveCollection<object>> (tempDefaultCollectionJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<object> (this, VM.DefaultCollection)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultProperty";
		EditorGUILayout.DelayedTextField (vmk, VM.DefaultProperty != null ? VM.DefaultProperty.ToString () : "null (object)");

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
				VM.RC_AddNum.Execute (new AddNumCommand () { Sender = VM });
			} else {
				AddNumCommand command = JsonConvert.DeserializeObject<AddNumCommand> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_AddNum.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "CurrentVector2";
		VM.CurrentVector2 = EditorGUILayout.Vector2Field (vmk, VM.CurrentVector2);

		vmk = "CurrentVector3";
		VM.CurrentVector3 = EditorGUILayout.Vector3Field (vmk, VM.CurrentVector3);

		vmk = "CurrentVector4";
		VM.CurrentVector4 = EditorGUILayout.Vector4Field (vmk, VM.CurrentVector4);

		vmk = "CurrentQuaternion";
		Vector3 tempCurrentQuaternionVector3 = VM.CurrentQuaternion.eulerAngles;
		tempCurrentQuaternionVector3 = EditorGUILayout.Vector3Field (vmk, tempCurrentQuaternionVector3);
		VM.CurrentQuaternion = Quaternion.Euler (tempCurrentQuaternionVector3);

		vmk = "CurrentRect";
		VM.CurrentRect = EditorGUILayout.RectField (vmk, VM.CurrentRect);

		vmk = "CurrentBounds";
		VM.CurrentBounds = EditorGUILayout.BoundsField (vmk, VM.CurrentBounds);

		vmk = "CurrentColor";
		VM.CurrentColor = EditorGUILayout.ColorField (vmk, VM.CurrentColor);

		vmk = "CurrentDateTime";
		DateTime tempCurrentDateTime;
		if (DateTime.TryParse (EditorGUILayout.DelayedTextField (vmk, VM.CurrentDateTime.ToString ()), out tempCurrentDateTime)) {
			if (VM.CurrentDateTime != tempCurrentDateTime) {
				VM.CurrentDateTime = tempCurrentDateTime;
			}
		}

		vmk = "CurrentTimeSpan";
		TimeSpan tempCurrentTimeSpan;
		if (TimeSpan.TryParse (EditorGUILayout.DelayedTextField (vmk, VM.CurrentTimeSpan.ToString ()), out tempCurrentTimeSpan)) {
			if (VM.CurrentTimeSpan != tempCurrentTimeSpan) {
				VM.CurrentTimeSpan = tempCurrentTimeSpan;
			}
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
