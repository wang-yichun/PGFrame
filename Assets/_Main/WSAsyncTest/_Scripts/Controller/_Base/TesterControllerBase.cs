using UnityEngine;
using System;
using System.Collections;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class TesterControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			TesterViewModel vm = (TesterViewModel)viewModel;
			vm.RC_SwitchCounter.Subscribe (_ => {
				SwitchCounter ((TesterViewModel)viewModel);
			}).AddTo (viewModel.baseAttachDisposables);
		}

		public override void Detach (ViewModelBase viewModel)
		{
			base.Detach (viewModel);
		}

		
		/*  */
		public virtual void SwitchCounter (TesterViewModel viewModel)
		{
		}
	}
}