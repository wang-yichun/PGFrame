using UnityEngine;
using System;
using System.Collections;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using PogoTools;

	public class GameCoreController : GameCoreControllerBase<GameCoreController>
	{
		public GameCoreController ()
		{
		}
		
		
		/*  */
		public virtual void AddSomeBullet (GameCoreViewModel viewModel)
		{
			base.AddSomeBullet (viewModel);
		}
		/*  */
		public virtual void RemoveSomeBullet (GameCoreViewModel viewModel)
		{
			base.RemoveSomeBullet (viewModel);
		}
	}

}