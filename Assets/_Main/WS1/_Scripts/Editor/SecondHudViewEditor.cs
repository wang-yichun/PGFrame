using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(SecondHudView))]
public class SecondHudViewElementViewEditor : SecondElementEditor
{
	public SecondHudView V { get; set; }

	void OnEnable ()
	{
		V = (SecondHudView)target;

		if (EditorApplication.isPlaying == false) {
			V.CreateViewModel ();
		}
		VM = V.VM;

		CommandParams = new Dictionary<string, string> ();
	}

	public override void VMCopyToJson ()
	{
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((SecondViewModelBase)VM);
	}
}