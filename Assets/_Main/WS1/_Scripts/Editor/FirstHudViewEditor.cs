using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(FirstHudView))]
public class FirstHudViewElementViewEditor : FirstElementEditor
{
	public FirstHudView V { get; set; }

	void OnEnable ()
	{
		V = (FirstHudView)target;

		if (EditorApplication.isPlaying == false) {
			V.CreateViewModel ();
		}
		VM = V.VM;

		CommandParams = new Dictionary<string, string> ();
	}

	public override void VMCopyToJson ()
	{
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((FirstViewModelBase)VM);
	}
}