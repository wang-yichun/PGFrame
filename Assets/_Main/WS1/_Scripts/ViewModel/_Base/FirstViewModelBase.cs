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
		RP_LabelTextNum = new IntReactiveProperty ();
		RC_AddNum = new ReactiveCommand<AddNumCommand> ();
	}

	public override void Attach ()
	{
		FirstController.Instance.Attach (this);
	}

	//////////////////////////////////////////////////
	// LabelTextNum Description
	//////////////////////////////////////////////////
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

	public ReactiveCommand<AddNumCommand> RC_AddNum;

}

public class AddNumCommand : ViewModelCommandBase
{
	public int value;
}