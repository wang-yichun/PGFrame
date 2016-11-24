using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;

[CustomEditor (typeof(PTestView))]
public class PTestViewElementViewEditor : PTestElementEditor
{
	public PTestView V { get; set; }

	void OnEnable ()
	{
		V = (PTestView)target;

		if (EditorApplication.isPlaying == false) {
			V.CreateViewModel ();
		}
		VM = V.VM;

		CommandParams = new Dictionary<string, string> ();
	}

	public override void VMCopyToJson ()
	{
		V.ViewModelInitValueJson = JsonConvert.SerializeObject ((PTestViewModelBase)VM);
	}
}