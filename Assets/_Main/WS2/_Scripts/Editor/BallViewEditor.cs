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

	[CustomEditor (typeof(BallView))]
	public class BallViewElementViewEditor : BallElementEditor
	{
		public BallView V { get; set; }

		void OnEnable ()
		{
			V = (BallView)target;

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
			V.ViewModelInitValueJson = JsonConvert.SerializeObject ((BallViewModelBase)VM, settings);
		}
	}

}