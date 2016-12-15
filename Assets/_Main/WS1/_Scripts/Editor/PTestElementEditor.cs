using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class PTestElementEditor : ElementEditorBase<PTestViewModel>
	{
		public override PTestViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			ViewBase V = target as ViewBase;

			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - PTest")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "PTestViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

		string vmk;

		vmk = "DefaultProperty1";
		string tempDefaultProperty1 = EditorGUILayout.DelayedTextField (vmk, VM.DefaultProperty1);
		if (tempDefaultProperty1 != VM.DefaultProperty1) {
			VM.DefaultProperty1 = tempDefaultProperty1;
		}

		vmk = "DefaultProperty2";
		string tempDefaultProperty2 = EditorGUILayout.DelayedTextField (vmk, VM.DefaultProperty2);
		if (tempDefaultProperty2 != VM.DefaultProperty2) {
			VM.DefaultProperty2 = tempDefaultProperty2;
		}

		vmk = "DefaultProperty3";
		int tempDefaultProperty3 = EditorGUILayout.DelayedIntField (vmk, VM.DefaultProperty3);
		if (tempDefaultProperty3 != VM.DefaultProperty3) {
			VM.DefaultProperty3 = tempDefaultProperty3;
		}

		vmk = "DefaultProperty4";
		float tempDefaultProperty4 = EditorGUILayout.DelayedFloatField (vmk, VM.DefaultProperty4);
		if (tempDefaultProperty4 != VM.DefaultProperty4) {
			VM.DefaultProperty4 = tempDefaultProperty4;
		}

		vmk = "DefaultCollection1";
		EditorGUILayout.BeginHorizontal ();
		string DefaultCollection1Json = JsonConvert.SerializeObject (VM.DefaultCollection1);
		string tempDefaultCollection1Json = EditorGUILayout.DelayedTextField (vmk, DefaultCollection1Json);
		if (tempDefaultCollection1Json != DefaultCollection1Json) {
			if (string.IsNullOrEmpty (tempDefaultCollection1Json)) {
				VM.DefaultCollection1 = null;
			} else {
				VM.DefaultCollection1 = JsonConvert.DeserializeObject<ReactiveCollection<string>> (tempDefaultCollection1Json);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<string> (this, VM.DefaultCollection1)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCollection2";
		EditorGUILayout.BeginHorizontal ();
		string DefaultCollection2Json = JsonConvert.SerializeObject (VM.DefaultCollection2);
		string tempDefaultCollection2Json = EditorGUILayout.DelayedTextField (vmk, DefaultCollection2Json);
		if (tempDefaultCollection2Json != DefaultCollection2Json) {
			if (string.IsNullOrEmpty (tempDefaultCollection2Json)) {
				VM.DefaultCollection2 = null;
			} else {
				VM.DefaultCollection2 = JsonConvert.DeserializeObject<ReactiveCollection<int>> (tempDefaultCollection2Json);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<int> (this, VM.DefaultCollection2)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultDictionary1";
		EditorGUILayout.BeginHorizontal ();
		string DefaultDictionary1 = JsonConvert.SerializeObject (VM.DefaultDictionary1);
		string tempDefaultDictionary1 = EditorGUILayout.DelayedTextField (vmk, DefaultDictionary1);
		if (tempDefaultDictionary1 != DefaultDictionary1) {
			if (string.IsNullOrEmpty (tempDefaultDictionary1)) {
				VM.DefaultDictionary1 = null;
			} else {
				VM.DefaultDictionary1 = JsonConvert.DeserializeObject<ReactiveDictionary<string,string>> (tempDefaultDictionary1);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<string,string> (this, VM.DefaultDictionary1)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultDictionary2";
		EditorGUILayout.BeginHorizontal ();
		string DefaultDictionary2 = JsonConvert.SerializeObject (VM.DefaultDictionary2);
		string tempDefaultDictionary2 = EditorGUILayout.DelayedTextField (vmk, DefaultDictionary2);
		if (tempDefaultDictionary2 != DefaultDictionary2) {
			if (string.IsNullOrEmpty (tempDefaultDictionary2)) {
				VM.DefaultDictionary2 = null;
			} else {
				VM.DefaultDictionary2 = JsonConvert.DeserializeObject<ReactiveDictionary<int,string>> (tempDefaultDictionary2);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<int,string> (this, VM.DefaultDictionary2)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand1";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Params")) {
			if (CommandParams.ContainsKey (vmk)) {
				CommandParams.Remove (vmk);
			} else {
				CommandParams [vmk] = JsonConvert.SerializeObject (new DefaultCommand1Command (), Formatting.Indented);
			}
		}
		if (GUILayout.Button ("Invoke")) {
			if (CommandParams.ContainsKey (vmk) == false) {
				VM.RC_DefaultCommand1.Execute (new DefaultCommand1Command () { Sender = VM });
			} else {
				DefaultCommand1Command command = JsonConvert.DeserializeObject<DefaultCommand1Command> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_DefaultCommand1.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "DefaultCommand2";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand2.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand3";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand3.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand4";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand4.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand5";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand5.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand6";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand6.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand7";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand7.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand8";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Params")) {
			if (CommandParams.ContainsKey (vmk)) {
				CommandParams.Remove (vmk);
			} else {
				CommandParams [vmk] = JsonConvert.SerializeObject (new DefaultCommand8Command (), Formatting.Indented);
			}
		}
		if (GUILayout.Button ("Invoke")) {
			if (CommandParams.ContainsKey (vmk) == false) {
				VM.RC_DefaultCommand8.Execute (new DefaultCommand8Command () { Sender = VM });
			} else {
				DefaultCommand8Command command = JsonConvert.DeserializeObject<DefaultCommand8Command> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_DefaultCommand8.Execute (command);
			}
		}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}

		vmk = "DefaultCommand9";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand9.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand10";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand10.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand11";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand11.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand12";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand12.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand13";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand13.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand14";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand14.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand15";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand15.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand16";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand16.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand17";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand17.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand18";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand18.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand19";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand19.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "DefaultCommand20";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_DefaultCommand20.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

			vmk = "CurrentFB";
			ViewBase CurrentFBView = (target as IPTestView).CurrentFBView as ViewBase;
			if (EditorApplication.isPlaying && VM.CurrentFB == null)
				CurrentFBView = null;
			ViewBase tempCurrentFBView = (ViewBase)EditorGUILayout.ObjectField (vmk, CurrentFBView, typeof(ViewBase), true);
			if (tempCurrentFBView == null) {
				(target as IPTestView).CurrentFBView = null;
				VM.CurrentFB = null;
			} else if (CurrentFBView != tempCurrentFBView) {
				var view = tempCurrentFBView as WS1.IFBView;
				if (view != null) {
					(target as IPTestView).CurrentFBView = tempCurrentFBView as WS1.IFBView;
					VM.CurrentFB = (WS1.FBViewModel)tempCurrentFBView.GetViewModel ();
				} else {
					Debug.Log ("类型不匹配, 需要一个: FB");
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

}