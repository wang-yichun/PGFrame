using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS2
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class BallElementEditor : ElementEditorBase<BallViewModel>
	{
		public override BallViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			ViewBase V = target as ViewBase;

			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - Ball")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "BallViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

		string vmk;

		vmk = "Type";
		VM.Type = (BallType)EditorGUILayout.EnumPopup (vmk, VM.Type);

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