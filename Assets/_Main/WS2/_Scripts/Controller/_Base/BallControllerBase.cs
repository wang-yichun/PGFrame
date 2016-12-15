using UnityEngine;
using System;
using System.Collections;

namespace WS2 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class BallControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			UnityEngine.Debug.Log ("BallControllerBase.Attach");

			BallViewModel vm = (BallViewModel)viewModel;

			
		}

		
	}
}