using UnityEngine;
using System.Collections;
using UniRx;

public class FirstControllerBase<T> : ControllerBase<T>
	where T:new()
{
	public virtual void AddNum (FirstViewModel viewModel, AddNumCommand command)
	{
	}

	public virtual void AddNumHandler (AddNumCommand command)
	{
		this.AddNum ((FirstViewModel)command.Sender, command);
	}

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase.Attach");

		FirstViewModel vm = (FirstViewModel)viewModel;

		vm.RC_AddNum.Subscribe<AddNumCommand> (AddNumHandler);
	}
}
