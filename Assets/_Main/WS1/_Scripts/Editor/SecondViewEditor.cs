using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(SecondView))]
public class SecondViewElementViewEditor : SecondElementEditor
{
	public SecondView V { get; set; }

	void OnEnable ()
	{
		V = (SecondView)target;

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