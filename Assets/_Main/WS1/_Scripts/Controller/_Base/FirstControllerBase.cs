using UnityEngine;
using System.Collections;
using UniRx;

public class FirstControllerBase<T> : ControllerBase<T>
	where T:new()
{
	public virtual void AddNum (FirstViewModel viewModel, AddNumCommand command)
	{
	}

	public virtual void ButtonClick (FirstViewModel viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase ButtonClick");
	}

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase.Attach");

		FirstViewModel vm = (FirstViewModel)viewModel;

		vm.RC_AddNum.Subscribe<AddNumCommand> (command => {
			command.Sender = viewModel;
			AddNum ((FirstViewModel)viewModel, command);
		});

		vm.RC_ButtonClick.Subscribe (_ => {
			ButtonClick ((FirstViewModel)viewModel);
		});
	}
}
