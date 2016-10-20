using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

[JsonObjectAttribute (MemberSerialization.OptIn)]
public class FirstViewModelBase : ViewModelBase
{
	public FirstViewModelBase ()
	{
	}

	public override void Initialize ()
	{
		RP_LabelTextNum = new ReactiveProperty<int> ();
		RC_Numbers = new ReactiveCollection<int> ();
		RCMD_AddNum = new ReactiveCommand<AddNumCommand> ();
	}

	public override void Attach ()
	{
		FirstController.Instance.Attach (this);
	}

	public ReactiveProperty<int> RP_LabelTextNum;

	[JsonProperty]
	public int LabelTextNum {
		get {
			return RP_LabelTextNum.Value;
		}
		set {
			RP_LabelTextNum.Value = value;
		}
	}

	public ReactiveCollection<int> RC_Numbers;

	[JsonProperty]
	public List<int> Numbers {
		get {
			if (RC_Numbers != null) {
				return RC_Numbers.ToList<int> ();
			} else {
				return null;
			}
		}
		set {
			if (value == null) {
				RC_Numbers = null;
			} else {
				RC_Numbers = new ReactiveCollection<int> (value);
			}
		}
	}

	public ReactiveCommand<AddNumCommand> RCMD_AddNum;

}

public class AddNumCommand : ViewModelCommandBase
{
	public int value;
}