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
		Numbers = new ReactiveCollection<int> ();
		MyDictionary = new ReactiveDictionary<string, string> ();

		RC_AddNum = new ReactiveCommand<AddNumCommand> ();
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

	[JsonProperty]
	public ReactiveCollection<int> Numbers;

	[JsonProperty]
	public ReactiveDictionary<string, string> MyDictionary;

	public ReactiveCommand<AddNumCommand> RC_AddNum;

}

public class AddNumCommand : ViewModelCommandBase
{
	public int value;
}