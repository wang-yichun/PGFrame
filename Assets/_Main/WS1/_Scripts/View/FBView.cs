using UnityEngine;
using System.Collections;
using UniRx;
using Newtonsoft.Json;
using PogoTools;
using UnityEngine.UI;

public class FBView : FBViewBase
{
	public override void Initialize (ViewModelBase viewModel)
	{
		base.Initialize (viewModel);
	}

	public override void Bind ()
	{
		base.Bind ();
		Debug.Log (string.Format ("FBView in {0} Bind.", gameObject.name));
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
		Debug.Log (string.Format ("FBView in {0} AfterBind.", gameObject.name));
	}

	public override void OnChanged_Count (int value)
	{
		base.OnChanged_Count (value);
		Debug.Log (string.Format ("OnChanged_Count. {0}", value));
	}

	public override void OnExecuted_FBTestCMD (Unit unit)
	{
		base.OnExecuted_FBTestCMD (unit);
		Debug.Log (string.Format ("OnExecuted_FBTestCMD. {0}"));
	}
}
