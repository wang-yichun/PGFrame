using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class PlayerInfoViewBase : ViewBase , IPlayerInfoView
{
	public PlayerInfoViewModel VM;

	public PlayerInfoViewModel PlayerInfo {
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
			VM = (PlayerInfoViewModel)viewModel;
		} else {
			if (AutoCreateViewModel && VM == null) {
				CreateViewModel ();
			}
		}

		base.Initialize (VM);
	}

	public override void CreateViewModel ()
	{
		if (UseEmptyViewModel || string.IsNullOrEmpty (ViewModelInitValueJson) == false) {
			VM = JsonConvert.DeserializeObject<PlayerInfoViewModel> (ViewModelInitValueJson);
			ViewModelPropertyRef ();
		} else {
			VM = new PlayerInfoViewModel ();
		}
	}

	public void ViewModelPropertyRef ()
	{
		
	}

	public override void Bind ()
	{
		base.Bind ();
		
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
	}

	

	
}
