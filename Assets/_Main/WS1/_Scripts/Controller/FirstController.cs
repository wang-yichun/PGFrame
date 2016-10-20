using UnityEngine;
using System.Collections;

public class FirstController : FirstControllerBase<FirstController>
{
	public FirstController ()
	{
	}

	public override void AddNum (FirstViewModel viewModel, AddNumCommand command)
	{
		base.AddNum (viewModel, command);
		Debug.Log ("FirstController: AddNum Invoked  " + command.value);

		viewModel.MyDictionary ["default"] = command.value.ToString ();
	}
}
