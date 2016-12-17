using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class FBControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			FBViewModel vm = (FBViewModel)viewModel;

			
			vm.RC_FBTestCMD.Subscribe (_ => {
				FBTestCMD ((FBViewModel)viewModel);
			}).AddTo (viewModel.baseAttachDisposables);
		}

		public override void Detach (ViewModelBase viewModel)
		{
			base.Detach (viewModel);
		}

		
		/*  */
		public virtual void FBTestCMD (FBViewModel viewModel)
		{
		}
	}
}