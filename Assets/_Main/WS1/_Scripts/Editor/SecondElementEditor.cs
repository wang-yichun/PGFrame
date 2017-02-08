using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class SecondElementEditor : ElementEditorBase<SecondViewModel>
	{
		public override SecondViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - Second")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "SecondViewModel", VM.VMID.ToString ().Substring (0, 8)));
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

		vmk = "IntDictionary";
		EditorGUILayout.BeginHorizontal ();
		string IntDictionary = JsonConvert.SerializeObject (VM.IntDictionary);
		string tempIntDictionary = EditorGUILayout.DelayedTextField (vmk, IntDictionary);
		if (tempIntDictionary != IntDictionary) {
			if (string.IsNullOrEmpty (tempIntDictionary)) {
				VM.IntDictionary = null;
			} else {
				VM.IntDictionary = JsonConvert.DeserializeObject<ReactiveDictionary<string,int>> (tempIntDictionary);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<string,int> (this, VM.IntDictionary)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "StringDictionary";
		EditorGUILayout.BeginHorizontal ();
		string StringDictionary = JsonConvert.SerializeObject (VM.StringDictionary);
		string tempStringDictionary = EditorGUILayout.DelayedTextField (vmk, StringDictionary);
		if (tempStringDictionary != StringDictionary) {
			if (string.IsNullOrEmpty (tempStringDictionary)) {
				VM.StringDictionary = null;
			} else {
				VM.StringDictionary = JsonConvert.DeserializeObject<ReactiveDictionary<int,string>> (tempStringDictionary);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<int,string> (this, VM.StringDictionary)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "StringCommand";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Params")) {
			if (CommandParams.ContainsKey (vmk)) {
				CommandParams.Remove (vmk);
			} else {
				CommandParams [vmk] = JsonConvert.SerializeObject (new StringCommandCommand (), Formatting.Indented);
			}
		}
		if (GUILayout.Button ("Invoke")) {
			if (CommandParams.ContainsKey (vmk) == false) {
				VM.RC_StringCommand.Execute (new StringCommandCommand () { Sender = VM });
			} else {
				StringCommandCommand command = JsonConvert.DeserializeObject<StringCommandCommand> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_StringCommand.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "IntCommand";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Params")) {
			if (CommandParams.ContainsKey (vmk)) {
				CommandParams.Remove (vmk);
			} else {
				CommandParams [vmk] = JsonConvert.SerializeObject (new IntCommandCommand (), Formatting.Indented);
			}
		}
		if (GUILayout.Button ("Invoke")) {
			if (CommandParams.ContainsKey (vmk) == false) {
				VM.RC_IntCommand.Execute (new IntCommandCommand () { Sender = VM });
			} else {
				IntCommandCommand command = JsonConvert.DeserializeObject<IntCommandCommand> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_IntCommand.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "SimpleCommand";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_SimpleCommand.Execute ();
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

		public override void VMCopyToJson ()
		{
		}

		#endregion
	}

}