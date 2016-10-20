using UnityEngine;
using System.Collections;

public class FirstView : FirstViewBase
{
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

	public override void OnExecuted_AddNum (AddNumCommand command)
	{
		base.OnExecuted_AddNum (command);
		Debug.Log ("OnExecuted_AddNum: " + command.value);

	}
}
