using UnityEngine;
using System.Collections;
using PogoTools;

public class FirstController : FirstControllerBase<FirstController>
{
	public FirstController ()
	{
	}
	
	
	/*  */
	public virtual void DefaultCommand (FirstViewModel viewModel)
	{
		base.DefaultCommand (viewModel);
	}
}
