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

	public class PlayerInfoHudView : PlayerInfoHudViewBase
	{
		public override void Initialize (ViewModelBase viewModel)
		{
			base.Initialize (viewModel);
		}

		public override void Bind ()
		{
			base.Bind ();
			Debug.Log (string.Format ("PlayerInfoHudView in {0} Bind.", gameObject.name));
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
			Debug.Log (string.Format ("PlayerInfoHudView in {0} AfterBind.", gameObject.name));
		}

		public override void OnChanged_Name (string value)
		{
			base.OnChanged_Name (value);
			Debug.Log ("PlayerInfoHudView.OnChanged_Name: " + value);
		}

		public override void OnChanged_Score (int value)
		{
			base.OnChanged_Score (value);
			Debug.Log ("PlayerInfoHudView.OnChanged_Score: " + value);
		}
	}

}