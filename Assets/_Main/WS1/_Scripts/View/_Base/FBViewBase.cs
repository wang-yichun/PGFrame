using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class FBViewBase : ViewBase , IFBView
{
	public FBViewModel VM;

	public FBViewModel FB {
		get {
			return VM;
		}
	}

	public override ViewModelBase GetViewModel ()
	{
		return VM;
	}

	public override void Initialize (ViewModelBase viewModel)
	{
		if (viewModel != null) {
			VM = (FBViewModel)viewModel;
		} else {
			if (AutoCreateViewModel) {
				if (VM == null) {
					CreateViewModel ();
				}
			}
		}

		base.Initialize (VM);
	}

	public override void CreateViewModel ()
	{
		if (string.IsNullOrEmpty (ViewModelInitValueJson) == false) {
			VM = JsonConvert.DeserializeObject<FBViewModel> (ViewModelInitValueJson);
		} else {
			VM = new FBViewModel ();
		}
	}

	public override void Bind ()
	{
		base.Bind ();
		
		VM.RP_Count.Subscribe (OnChanged_Count);
		VM.RC_FBTestCMD.Subscribe (OnExecuted_FBTestCMD);
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
	}

	

	public virtual void OnChanged_Count (int value)
	{
	}

	public virtual void OnExecuted_FBTestCMD (Unit unit)
	{
	}

	
}
