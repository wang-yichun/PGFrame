using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class PlayerInfoElementEditor : ElementEditorBase<PlayerInfoViewModel>
	{
		public override PlayerInfoViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			ViewBase V = target as ViewBase;

			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
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

}