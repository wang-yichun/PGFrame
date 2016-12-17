using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class SecondControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			SecondViewModel vm = (SecondViewModel)viewModel;
			vm.RC_StringCommand.Subscribe<StringCommandCommand> (command => {
				command.Sender = viewModel;
				StringCommand ((SecondViewModel)viewModel, command);
			}).AddTo (viewModel.baseAttachDisposables);
			vm.RC_IntCommand.Subscribe<IntCommandCommand> (command => {
				command.Sender = viewModel;
				IntCommand ((SecondViewModel)viewModel, command);
			}).AddTo (viewModel.baseAttachDisposables);
			vm.RC_SimpleCommand.Subscribe (_ => {
				SimpleCommand ((SecondViewModel)viewModel);
			}).AddTo (viewModel.baseAttachDisposables);
		}

		public override void Detach (ViewModelBase viewModel)
		{
			base.Detach (viewModel);
		}

		
		/*  */
		public virtual void StringCommand (SecondViewModel viewModel, StringCommandCommand command)
		{
		}
		/*  */
		public virtual void IntCommand (SecondViewModel viewModel, IntCommandCommand command)
		{
		}
		/*  */
		public virtual void SimpleCommand (SecondViewModel viewModel)
		{
		}
	}
}