using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using PogoTools;

	public class SecondController : SecondControllerBase<SecondController>
	{
		public SecondController ()
		{
		}
		
		
		/*  */
		public override void StringCommand (SecondViewModel viewModel, StringCommandCommand command)
		{
			base.StringCommand (viewModel, command);
		}
		/*  */
		public override void IntCommand (SecondViewModel viewModel, IntCommandCommand command)
		{
			base.IntCommand (viewModel, command);
		}
		/*  */
		public override void SimpleCommand (SecondViewModel viewModel)
		{
			base.SimpleCommand (viewModel);
		}
	}

}