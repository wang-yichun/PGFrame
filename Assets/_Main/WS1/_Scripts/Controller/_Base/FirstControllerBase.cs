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

	public virtual void ButtonClick (FirstViewModel viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase ButtonClick");
	}

	public virtual void ButtonClickHandler (Unit unit)
	{
		this.ButtonClick (null);
	}

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("FirstControllerBase.Attach");

		FirstViewModel vm = (FirstViewModel)viewModel;

		vm.RC_AddNum.Subscribe<AddNumCommand> (AddNumHandler);
		vm.RC_ButtonClick.Subscribe (ButtonClickHandler);
	}
}
