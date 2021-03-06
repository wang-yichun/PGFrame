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
		public override void AddSomeBullet (GameCoreViewModel viewModel)
		{
			base.AddSomeBullet (viewModel);
		}
		/*  */
		public override void RemoveSomeBullet (GameCoreViewModel viewModel)
		{
			base.RemoveSomeBullet (viewModel);
		}

		public override void GameStateChanged (GameCoreViewModel viewModel, GameCoreFSM.State newState, GameCoreFSM.State oldState)
		{
			base.GameStateChanged (viewModel, newState, oldState);
			PRDebug.TagLog ("GameCoreController", Color.blue, string.Format ("{0} -> {1}", oldState.ToString (), newState.ToString ()));
		}
	}

}