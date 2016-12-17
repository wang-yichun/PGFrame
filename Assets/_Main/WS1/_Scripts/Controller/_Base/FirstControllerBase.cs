using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class FirstControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			FirstViewModel vm = (FirstViewModel)viewModel;

			
			vm.RC_DefaultCommand.Subscribe (_ => {
				DefaultCommand ((FirstViewModel)viewModel);
			}).AddTo (viewModel.baseAttachDisposables);
			vm.RC_AddNum.Subscribe<AddNumCommand> (command => {
				command.Sender = viewModel;
				AddNum ((FirstViewModel)viewModel, command);
			}).AddTo (viewModel.baseAttachDisposables);
		}

		public override void Detach (ViewModelBase viewModel)
		{
			base.Detach (viewModel);
		}

		
		/*  */
		public virtual void DefaultCommand (FirstViewModel viewModel)
		{
		}
		/*  */
		public virtual void AddNum (FirstViewModel viewModel, AddNumCommand command)
		{
		}
	}
}