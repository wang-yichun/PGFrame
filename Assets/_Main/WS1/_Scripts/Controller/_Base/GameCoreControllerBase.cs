using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class GameCoreControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("GameCoreControllerBase.Attach");

		GameCoreViewModel vm = (GameCoreViewModel)viewModel;

		
	}

	
}
