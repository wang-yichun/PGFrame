using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PGFrame
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class ElementEditorBase<TViewModel> : Editor, IElementEditor
	where TViewModel: ViewModelBase
	{
		public virtual TViewModel VM { get; set; }

		protected bool ToggleSettings = true;
		protected bool ToggleVMJsonOn = false;
		protected bool ToggleView = true;
		protected bool ToggleViewModel = true;

		public Dictionary<string, string> CommandParams { get; set; }

		public override void OnInspectorGUI ()
		{
			ViewBase V = target as ViewBase;
			if (ToggleSettings = EditorGUILayout.Foldout (ToggleSettings, "Settings")) {
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Log VMJson")) {
					Debug.Log (V.ViewModelInitValueJson);
				}
				if (GUILayout.Button ("Log VM's Hosts")) {
					Debug.Log (JsonConvert.SerializeObject (V.GetViewModel ().HostViews.Select (kvp => {
						return new {key = kvp.Key, object_name = kvp.Value.gameObject.name, instance_id = kvp.Value.gameObject.GetInstanceID ()};
					}), Formatting.Indented));
				}
				if (GUILayout.Button ("Clear VM & InitValueJson")) {
					VM = null;
					V = target as ViewBase;
					V.ViewModelInitValueJson = string.Empty;
				}

				EditorGUILayout.EndHorizontal ();
				if (EditorApplication.isPlaying == false) {
					V.AutoCreateViewModel = EditorGUILayout.ToggleLeft ("Auto Create ViewModel", V.AutoCreateViewModel);
					V.UseEmptyViewModel = EditorGUILayout.ToggleLeft ("Use Empty ViewModel", V.UseEmptyViewModel);
				}
				V.ViewBaseKey = EditorGUILayout.DelayedTextField ("View Base Key", V.ViewBaseKey);
				if (ToggleVMJsonOn = EditorGUILayout.ToggleLeft (string.Format ("Show VM Json (Length:{0})", V.VMJsonSize), ToggleVMJsonOn)) {
					EditorGUILayout.TextArea (JsonConvert.SerializeObject ((TViewModel)VM, Formatting.Indented));
				}
				EditorGUILayout.Space ();
			}
		}

		#region IElementEditor implementation

		public virtual void VMCopyToJson ()
		{
		}

		#endregion
	}

}