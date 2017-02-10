using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class TesterElementEditor : ElementEditorBase<TesterViewModel>
	{
		public override TesterViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - Tester")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "TesterViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

			string vmk;

			vmk = "CounterEnable";
			VM.CounterEnable = EditorGUILayout.Toggle (vmk, VM.CounterEnable);

			vmk = "CountValue";
			int tempCountValue = EditorGUILayout.DelayedIntField (vmk, VM.CountValue);
			if (tempCountValue != VM.CountValue) {
				VM.CountValue = tempCountValue;
			}

			vmk = "SwitchCounter";
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel (vmk);
			if (GUILayout.Button ("Invoke")) {
				VM.RC_SwitchCounter.Execute ();
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