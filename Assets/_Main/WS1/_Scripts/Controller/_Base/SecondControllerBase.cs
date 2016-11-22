using UnityEngine;
using System.Collections;
using UniRx;

public class SecondControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("SecondControllerBase.Attach");

		SecondViewModel vm = (SecondViewModel)viewModel;

		
	}

	
}
