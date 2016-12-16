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

	[CustomEditor (typeof(FirstView))]
	public class FirstViewElementViewEditor : FirstElementEditor
	{
		public FirstView V { get; set; }

		void OnEnable ()
		{
			V = (FirstView)target;

			if (EditorApplication.isPlaying == false) {
				V.CreateViewModel ();
			}
			VM = V.VM;

			CommandParams = new Dictionary<string, string> ();
		}

		public override void VMCopyToJson ()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings () {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
			V.ViewModelInitValueJson = JsonConvert.SerializeObject ((FirstViewModelBase)VM, settings);
		}
	}

}