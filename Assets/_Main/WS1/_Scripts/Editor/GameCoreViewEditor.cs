using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

[CustomEditor (typeof(GameCoreView))]
public class GameCoreViewElementViewEditor : GameCoreElementEditor
{
	public GameCoreView V { get; set; }

	void OnEnable ()
	{
		V = (GameCoreView)target;

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
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((GameCoreViewModelBase)VM, settings);
	}
}