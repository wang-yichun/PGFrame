using UnityEngine;
using System.Collections;
using UniRx;
using Newtonsoft.Json;
using PogoTools;
using UnityEngine.UI;

public class FirstView : FirstViewBase
{
	public override void Initialize (ViewModelBase viewModel)
	{
		base.Initialize (viewModel);
	}

	public override void Bind ()
	{
		base.Bind ();
		Debug.Log (string.Format ("FirstView in {0} Bind.", gameObject.name));
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
		Debug.Log (string.Format ("FirstView in {0} AfterBind.", gameObject.name));
	}

	public override void OnExecuted_FBTestCMD (Unit unit)
	{
		base.OnExecuted_FBTestCMD (unit);
		Debug.Log (string.Format ("FirstView: OnExecuted_FBTestCMD."));
	}

	public SCA msca;
}
