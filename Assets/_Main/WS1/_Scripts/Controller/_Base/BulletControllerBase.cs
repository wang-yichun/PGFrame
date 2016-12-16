using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class BulletControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			UnityEngine.Debug.Log ("BulletControllerBase.Attach");

			BulletViewModel vm = (BulletViewModel)viewModel;

			
		}

		
	}
}