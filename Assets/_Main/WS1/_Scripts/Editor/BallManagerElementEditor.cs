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

	public class BallManagerElementEditor : ElementEditorBase<BallManagerViewModel>
	{
		public override BallManagerViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - BallManager")) {

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
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "BallManagerViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

		string vmk;

		vmk = "MyBallType";
		VM.MyBallType = (WS2.BallType)EditorGUILayout.EnumPopup (vmk, VM.MyBallType);

		vmk = "MyBalls";
		EditorGUILayout.BeginHorizontal ();
		string MyBallsJson = JsonConvert.SerializeObject (VM.MyBalls);
		string tempMyBallsJson = EditorGUILayout.DelayedTextField (vmk, MyBallsJson);
		if (tempMyBallsJson != MyBallsJson) {
			if (string.IsNullOrEmpty (tempMyBallsJson)) {
				VM.MyBalls = null;
			} else {
				VM.MyBalls = JsonConvert.DeserializeObject<ReactiveCollection<WS2.BallViewModel>> (tempMyBallsJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<WS2.BallViewModel> (this, VM.MyBalls)
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

		public override void VMCopyToJson ()
		{
		}

		#endregion
	}

}