using UnityEngine;
using System.Collections;
using UniRx;

public class FirstControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase.Attach");

		FirstViewModel vm = (FirstViewModel)viewModel;

		
		vm.RC_DefaultCommand.Subscribe (_ => {
			DefaultCommand ((FirstViewModel)viewModel);
		});
		vm.RC_AddNum.Subscribe<AddNumCommand> (command => {
			command.Sender = viewModel;
			AddNum ((FirstViewModel)viewModel, command);
		});
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
