using UnityEngine;
using System.Collections;
using UniRx;
using Newtonsoft.Json;
using PogoTools;

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

	public override void OnChanged_LabelTextNum (int value)
	{
		base.OnChanged_LabelTextNum (value);
		Debug.Log ("OnChanged_LabelTextNum: " + value);
	}

	public override void OnAdd_Numbers (CollectionAddEvent<int> e)
	{
		base.OnAdd_Numbers (e);
		Debug.Log ("OnAdd_Numbers: " + e.Value + " idx: " + e.Index);
	}

	public override void OnRemove_Numbers (CollectionRemoveEvent<int> e)
	{
		base.OnRemove_Numbers (e);
		Debug.Log ("OnRemove_Numbers: " + e.Value + " idx: " + e.Index);
	}

	public override void OnExecuted_AddNum (AddNumCommand command)
	{
		base.OnExecuted_AddNum (command);
		Debug.Log ("OnExecuted_AddNum: " + command.value);
	}
}
