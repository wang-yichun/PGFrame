using UnityEngine;
using System.Collections;
using PogoTools;

public class SecondController : SecondControllerBase<SecondController>
{
	public SecondController ()
	{
	}
	
	
	/* 增加一个数 */
	public virtual void AddNum2 (SecondViewModel viewModel, AddNum2Command command)
	{
		base.AddNum2 (viewModel, command);
	}
	/*  */
	public virtual void ButtonClick (SecondViewModel viewModel)
	{
		base.ButtonClick (viewModel);
	}
}
