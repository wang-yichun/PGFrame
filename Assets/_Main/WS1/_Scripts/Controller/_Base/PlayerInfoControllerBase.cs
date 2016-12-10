using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class PlayerInfoControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("PlayerInfoControllerBase.Attach");

		PlayerInfoViewModel vm = (PlayerInfoViewModel)viewModel;

		
	}

	
}
