using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class BulletElementEditor : ElementEditorBase<BulletViewModel>
	{
		public override BulletViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			ViewBase V = target as ViewBase;

			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - Bullet")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "BulletViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

		string vmk;

		vmk = "BType";
		VM.BType = (BulletType)EditorGUILayout.EnumPopup (vmk, VM.BType);

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