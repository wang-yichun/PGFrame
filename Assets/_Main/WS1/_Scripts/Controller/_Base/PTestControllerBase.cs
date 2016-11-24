using UnityEngine;
using System.Collections;
using UniRx;

public class PTestControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("PTestControllerBase.Attach");

		PTestViewModel vm = (PTestViewModel)viewModel;

		
	}

	
}
