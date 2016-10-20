using UnityEngine;
using System.Collections;
using UniRx;
using Newtonsoft.Json;
using PogoTools;
using UnityEngine.UI;

public class FirstView : FirstViewBase
{
	public Text text;
	public Button button;

	public override void Initialize (ViewModelBase viewModel)
	{
		base.Initialize (viewModel);
	}

	public override void Bind ()
	{
		base.Bind ();
		Debug.Log (string.Format ("FirstView in {0} Bind.", gameObject.name));

		if (text != null)
			VM.RP_LabelTextNum.SubscribeToText (text);

		if (button != null)
			VM.RC_ButtonClick.BindTo (button);
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

	public override void OnReplace_Numbers (CollectionReplaceEvent<int> e)
	{
		base.OnReplace_Numbers (e);
		Debug.Log (string.Format ("OnReplace_Numbers: {0} -> {1} idx: {2}", e.OldValue, e.NewValue, e.Index));
	}

	public override void OnExecuted_AddNum (AddNumCommand command)
	{
		base.OnExecuted_AddNum (command);
		Debug.Log ("OnExecuted_AddNum: " + command.value);
	}
}
