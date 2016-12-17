using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using PogoTools;

	public class FirstController : FirstControllerBase<FirstController>
	{
		public FirstController ()
		{
		}
		
		
		/*  */
		public override void DefaultCommand (FirstViewModel viewModel)
		{
			base.DefaultCommand (viewModel);
		}
		/*  */
		public override void AddNum (FirstViewModel viewModel, AddNumCommand command)
		{
			base.AddNum (viewModel, command);
		}
	}

}