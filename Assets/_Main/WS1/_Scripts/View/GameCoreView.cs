using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;
	using PogoTools;

	public class GameCoreView : GameCoreViewBase
	{
		public override void Initialize (ViewModelBase viewModel)
		{
			base.Initialize (viewModel);
		}

		public override void Bind ()
		{
			base.Bind ();
			Debug.Log (string.Format ("GameCoreView in {0} Bind.", gameObject.name));
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
			Debug.Log (string.Format ("GameCoreView in {0} AfterBind.", gameObject.name));
		}

		public override void OnPairChanged_GameState (Pair<GameCoreFSM.State> pair)
		{
			base.OnPairChanged_GameState (pair);

			PRDebug.TagLog ("GameCoreView", Color.blue, string.Format ("{0} -> {1}", pair.Previous.ToString (), pair.Current.ToString ()));
		}
	}

}