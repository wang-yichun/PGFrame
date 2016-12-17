using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
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

			
		vm.RC_AddSomeBullet.Subscribe (_ => {
			AddSomeBullet ((GameCoreViewModel)viewModel);
		});
		vm.RC_RemoveSomeBullet.Subscribe (_ => {
			RemoveSomeBullet ((GameCoreViewModel)viewModel);
		});
		}

		
	/*  */
	public virtual void AddSomeBullet (GameCoreViewModel viewModel)
	{
	}
	/*  */
	public virtual void RemoveSomeBullet (GameCoreViewModel viewModel)
	{
	}
	}
}