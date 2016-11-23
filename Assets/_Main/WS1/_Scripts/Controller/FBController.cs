using UnityEngine;
using System.Collections;
using PogoTools;

public class FBController : FBControllerBase<FBController>
{
	public FBController ()
	{
	}

	public override void FBTestCMD (FBViewModel viewModel)
	{
		base.FBTestCMD (viewModel);
		Debug.Log ("FBTestCMD: " + viewModel.VMID);
	}
}
