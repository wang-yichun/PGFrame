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

		vmk = "IntList";
		EditorGUILayout.BeginHorizontal ();
		string IntListJson = JsonConvert.SerializeObject (VM.IntList);
		string tempIntListJson = EditorGUILayout.DelayedTextField (vmk, IntListJson);
		if (tempIntListJson != IntListJson) {
			if (string.IsNullOrEmpty (tempIntListJson)) {
				VM.IntList = null;
			} else {
				VM.IntList = JsonConvert.DeserializeObject<ReactiveCollection<int>> (tempIntListJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<int> (this, VM.IntList)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "LongList";
		EditorGUILayout.BeginHorizontal ();
		string LongListJson = JsonConvert.SerializeObject (VM.LongList);
		string tempLongListJson = EditorGUILayout.DelayedTextField (vmk, LongListJson);
		if (tempLongListJson != LongListJson) {
			if (string.IsNullOrEmpty (tempLongListJson)) {
				VM.LongList = null;
			} else {
				VM.LongList = JsonConvert.DeserializeObject<ReactiveCollection<long>> (tempLongListJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<long> (this, VM.LongList)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "FloatList";
		EditorGUILayout.BeginHorizontal ();
		string FloatListJson = JsonConvert.SerializeObject (VM.FloatList);
		string tempFloatListJson = EditorGUILayout.DelayedTextField (vmk, FloatListJson);
		if (tempFloatListJson != FloatListJson) {
			if (string.IsNullOrEmpty (tempFloatListJson)) {
				VM.FloatList = null;
			} else {
				VM.FloatList = JsonConvert.DeserializeObject<ReactiveCollection<float>> (tempFloatListJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<float> (this, VM.FloatList)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DoubleList";
		EditorGUILayout.BeginHorizontal ();
		string DoubleListJson = JsonConvert.SerializeObject (VM.DoubleList);
		string tempDoubleListJson = EditorGUILayout.DelayedTextField (vmk, DoubleListJson);
		if (tempDoubleListJson != DoubleListJson) {
			if (string.IsNullOrEmpty (tempDoubleListJson)) {
				VM.DoubleList = null;
			} else {
				VM.DoubleList = JsonConvert.DeserializeObject<ReactiveCollection<double>> (tempDoubleListJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<double> (this, VM.DoubleList)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "StringList";
		EditorGUILayout.BeginHorizontal ();
		string StringListJson = JsonConvert.SerializeObject (VM.StringList);
		string tempStringListJson = EditorGUILayout.DelayedTextField (vmk, StringListJson);
		if (tempStringListJson != StringListJson) {
			if (string.IsNullOrEmpty (tempStringListJson)) {
				VM.StringList = null;
			} else {
				VM.StringList = JsonConvert.DeserializeObject<ReactiveCollection<string>> (tempStringListJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<string> (this, VM.StringList)
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
