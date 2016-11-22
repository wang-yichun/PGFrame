using UnityEngine;
using System.Collections;
using PogoTools;

public class SecondController : SecondControllerBase<SecondController>
{
	public SecondController ()
	{
	}

	public override void SimpleCommand (SecondViewModel viewModel)
	{
		base.SimpleCommand (viewModel);
		Debug.Log ("SimpleCommand");
	}

	public override void IntCommand (SecondViewModel viewModel, IntCommandCommand command)
	{
		base.IntCommand (viewModel, command);
		Debug.Log ("IntCommand: " + command.Param0);
	}
}
