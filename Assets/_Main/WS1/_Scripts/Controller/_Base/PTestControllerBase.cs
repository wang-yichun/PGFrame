using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class PTestControllerBase<T> : ControllerBase<T>
		where T: Singleton<T>, new()
	{

		public override void Attach (ViewModelBase viewModel)
		{
			UnityEngine.Debug.Log ("PTestControllerBase.Attach");

			PTestViewModel vm = (PTestViewModel)viewModel;

			
		vm.RC_DefaultCommand1.Subscribe<DefaultCommand1Command> (command => {
			command.Sender = viewModel;
			DefaultCommand1 ((PTestViewModel)viewModel, command);
		});
		vm.RC_DefaultCommand2.Subscribe (_ => {
			DefaultCommand2 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand3.Subscribe (_ => {
			DefaultCommand3 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand4.Subscribe (_ => {
			DefaultCommand4 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand5.Subscribe (_ => {
			DefaultCommand5 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand6.Subscribe (_ => {
			DefaultCommand6 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand7.Subscribe (_ => {
			DefaultCommand7 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand8.Subscribe<DefaultCommand8Command> (command => {
			command.Sender = viewModel;
			DefaultCommand8 ((PTestViewModel)viewModel, command);
		});
		vm.RC_DefaultCommand9.Subscribe (_ => {
			DefaultCommand9 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand10.Subscribe (_ => {
			DefaultCommand10 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand11.Subscribe (_ => {
			DefaultCommand11 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand12.Subscribe (_ => {
			DefaultCommand12 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand13.Subscribe (_ => {
			DefaultCommand13 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand14.Subscribe (_ => {
			DefaultCommand14 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand15.Subscribe (_ => {
			DefaultCommand15 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand16.Subscribe (_ => {
			DefaultCommand16 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand17.Subscribe (_ => {
			DefaultCommand17 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand18.Subscribe (_ => {
			DefaultCommand18 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand19.Subscribe (_ => {
			DefaultCommand19 ((PTestViewModel)viewModel);
		});
		vm.RC_DefaultCommand20.Subscribe (_ => {
			DefaultCommand20 ((PTestViewModel)viewModel);
		});
		}

		
	/*  */
	public virtual void DefaultCommand1 (PTestViewModel viewModel, DefaultCommand1Command command)
	{
	}
	/*  */
	public virtual void DefaultCommand2 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand3 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand4 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand5 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand6 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand7 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand8 (PTestViewModel viewModel, DefaultCommand8Command command)
	{
	}
	/*  */
	public virtual void DefaultCommand9 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand10 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand11 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand12 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand13 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand14 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand15 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand16 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand17 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand18 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand19 (PTestViewModel viewModel)
	{
	}
	/*  */
	public virtual void DefaultCommand20 (PTestViewModel viewModel)
	{
	}
	}
}