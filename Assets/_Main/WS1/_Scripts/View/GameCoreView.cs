using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

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

			// MyInfoHud
			GameObject MyInfoHud_GO = GameObject.Find ("MyInfoHud");
			PlayerInfoHudView MyInfoHud_View = MyInfoHud_GO.GetComponent<PlayerInfoHudView> ();
			MyInfoHud_View.Initialize (GameCore.MyInfo);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
			Debug.Log (string.Format ("GameCoreView in {0} AfterBind.", gameObject.name));

		}
	}

}