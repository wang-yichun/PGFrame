using UnityEngine;
using System;
using System.Collections;

namespace WS1 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class BallManagerControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			UnityEngine.Debug.Log ("BallManagerControllerBase.Attach");

			BallManagerViewModel vm = (BallManagerViewModel)viewModel;

			
		}

		
	}
}