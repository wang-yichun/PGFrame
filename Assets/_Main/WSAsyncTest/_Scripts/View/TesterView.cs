using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;
	using PogoTools;

	public class TesterView : TesterViewBase
	{
		public Button counterSwitch;
		public Text counterText;

		public override void Initialize (ViewModelBase viewModel)
		{
			base.Initialize (viewModel);
		}

		public override void Bind ()
		{
			base.Bind ();
			Debug.Log (string.Format ("TesterView in {0} Bind.", gameObject.name));

			Tester.RC_SwitchCounter.BindTo (counterSwitch);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
			Debug.Log (string.Format ("TesterView in {0} AfterBind.", gameObject.name));
		}

		public override void OnChanged_CountValue (int value)
		{
			base.OnChanged_CountValue (value);
			counterText.text = value.ToString ();
		}
	}

}