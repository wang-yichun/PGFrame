using UnityEngine;
using System;
using System.Collections;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using PogoTools;

	public class TesterController : TesterControllerBase<TesterController>
	{
		public TesterController ()
		{
		}

		public override void SwitchCounter (TesterViewModel viewModel)
		{
			base.SwitchCounter (viewModel);
			viewModel.CounterEnable = !viewModel.CounterEnable;
		}
	}

}