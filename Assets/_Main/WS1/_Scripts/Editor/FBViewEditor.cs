using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(FBView))]
public class FBViewElementViewEditor : FBElementEditor
{
	public FBView V { get; set; }

	void OnEnable ()
	{
		V = (FBView)target;

		if (EditorApplication.isPlaying == false) {
			V.CreateViewModel ();
		}
		VM = V.VM;

		CommandParams = new Dictionary<string, string> ();
	}

	public override void VMCopyToJson ()
	{
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((FBViewModelBase)VM);
	}
}