using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using PogoTools;

namespace WS1
{
	public class PlayerInfoView : PlayerInfoViewBase
	{
		public override void Initialize (ViewModelBase viewModel)
		{
			base.Initialize (viewModel);
		}

		public override void Bind ()
		{
			base.Bind ();
			Debug.Log (string.Format ("PlayerInfoView in {0} Bind.", gameObject.name));
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
			Debug.Log (string.Format ("PlayerInfoView in {0} AfterBind.", gameObject.name));
		}
	}
}