using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

////////////////////////////////////////////////////////////////////////////////
// 一个 element 的定义 
////////////////////////////////////////////////////////////////////////////////
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
		RC_DefaultCommand = new ReactiveCommand ();
		RP_MyNewProperty = new ReactiveProperty<object> ();
		DefaultCollection = new ReactiveCollection<object> ();
		RP_DefaultProperty = new ReactiveProperty<object> ();
		RC_AddNum = new ReactiveCommand<AddNumCommand> ();
	}

	public override void Attach ()
	{
		FirstController.Instance.Attach (this);
	}

	

	/* LabelText# LabelTextNum's Comment */
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

	/* This is a numbers list */
	[JsonProperty] public ReactiveCollection<int> Numbers;

	/* 我的字典# */
	[JsonProperty] public ReactiveDictionary<string, string> MyDictionary;

	/*  */
	public ReactiveCommand RC_DefaultCommand;
	

	/*  */
	public ReactiveProperty<object> RP_MyNewProperty;

	[JsonProperty]
	public object MyNewProperty {
		get {
			return RP_MyNewProperty.Value;
		}
		set {
			RP_MyNewProperty.Value = value;
		}
	}

	/*  */
	[JsonProperty] public ReactiveCollection<object> DefaultCollection;

	/*  */
	public ReactiveProperty<object> RP_DefaultProperty;

	[JsonProperty]
	public object DefaultProperty {
		get {
			return RP_DefaultProperty.Value;
		}
		set {
			RP_DefaultProperty.Value = value;
		}
	}

	/*  */
	public ReactiveCommand<AddNumCommand> RC_AddNum;
	
}


public class DefaultCommandCommand : ViewModelCommandBase
{

}

public class AddNumCommand : ViewModelCommandBase
{

	/*  */
	public int Param0;
	
	/*  */
	public int Param1;
	
}
