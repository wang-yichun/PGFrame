using UnityEngine;
using System.Collections;
using PogoTools;

public class FirstController : FirstControllerBase<FirstController>
{
	public FirstController ()
	{
	}

	public override void AddNum (FirstViewModel viewModel, AddNumCommand command)
	{
		base.AddNum (viewModel, command);
		Debug.Log ("FirstController: AddNum Invoked  " + command.value);

		viewModel.MyDictionary ["default2"] = command.value.ToString ();
	}

	public override void ButtonClick (FirstViewModel viewModel)
	{
		base.ButtonClick (viewModel);

		PRDebug.TagLog ("VMID", viewModel.VMID);
	}
}
