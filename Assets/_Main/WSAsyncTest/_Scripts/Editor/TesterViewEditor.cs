using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	[CustomEditor (typeof(TesterView))]
	public class TesterViewElementViewEditor : TesterElementEditor
	{
		public TesterView V { get; set; }

		void OnEnable ()
		{
			V = (TesterView)target;

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
			V.ViewModelInitValueJson = JsonConvert.SerializeObject ((TesterViewModelBase)VM, settings);
		}
	}

}