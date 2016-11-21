using UnityEngine;
using System.Collections;
using UniRx;

public class SecondControllerBase<T> : ControllerBase<T>
	where T: Singleton<T>, new()
{

	public override void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("SecondControllerBase.Attach");

		SecondViewModel vm = (SecondViewModel)viewModel;

		
		vm.RC_AddNum2.Subscribe<AddNum2Command> (command => {
			command.Sender = viewModel;
			AddNum2 ((SecondViewModel)viewModel, command);
		});
		vm.RC_ButtonClick.Subscribe (_ => {
			ButtonClick ((SecondViewModel)viewModel);
		});
	}

	
	/* 增加一个数 */
	public virtual void AddNum2 (SecondViewModel viewModel, AddNum2Command command)
	{
	}
	/*  */
	public virtual void ButtonClick (SecondViewModel viewModel)
	{
	}
}
