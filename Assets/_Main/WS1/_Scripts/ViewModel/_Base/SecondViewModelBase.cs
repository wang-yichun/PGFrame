using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	 一个 element 的定义 
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class SecondViewModelBase : ViewModelBase
	{
		public SecondViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			
		RP_IntValue = new ReactiveProperty<int> ();
		RP_LongValue = new ReactiveProperty<long> ();
		RP_FloatValue = new ReactiveProperty<float> ();
		RP_DoubleValue = new ReactiveProperty<double> ();
		IntList = new ReactiveCollection<int> ();
		LongList = new ReactiveCollection<long> ();
		FloatList = new ReactiveCollection<float> ();
		DoubleList = new ReactiveCollection<double> ();
		StringList = new ReactiveCollection<string> ();
		IntDictionary = new ReactiveDictionary<string, int> ();
		StringDictionary = new ReactiveDictionary<int, string> ();
		RC_StringCommand = new ReactiveCommand<StringCommandCommand> ();
		RC_IntCommand = new ReactiveCommand<IntCommandCommand> ();
		RC_SimpleCommand = new ReactiveCommand ();
		}

		public override void Attach ()
		{
			base.Attach ();
			SecondController.Instance.Attach (this);
		}

		

	/* Label上的文字 */
	public ReactiveProperty<int> RP_IntValue;

	[JsonProperty]
	public int IntValue {
		get {
			return RP_IntValue.Value;
		}
		set {
			RP_IntValue.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<long> RP_LongValue;

	[JsonProperty]
	public long LongValue {
		get {
			return RP_LongValue.Value;
		}
		set {
			RP_LongValue.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<float> RP_FloatValue;

	[JsonProperty]
	public float FloatValue {
		get {
			return RP_FloatValue.Value;
		}
		set {
			RP_FloatValue.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<double> RP_DoubleValue;

	[JsonProperty]
	public double DoubleValue {
		get {
			return RP_DoubleValue.Value;
		}
		set {
			RP_DoubleValue.Value = value;
		}
	}

	/* 一个整形数组 */
	[JsonProperty] public ReactiveCollection<int> IntList;

	/*  */
	[JsonProperty] public ReactiveCollection<long> LongList;

	/*  */
	[JsonProperty] public ReactiveCollection<float> FloatList;

	/*  */
	[JsonProperty] public ReactiveCollection<double> DoubleList;

	/*  */
	[JsonProperty] public ReactiveCollection<string> StringList;

	/* 我的字典 */
	[JsonProperty] public ReactiveDictionary<string, int> IntDictionary;

	/*  */
	[JsonProperty] public ReactiveDictionary<int, string> StringDictionary;

	/*  */
	public ReactiveCommand<StringCommandCommand> RC_StringCommand;
	

	/*  */
	public ReactiveCommand<IntCommandCommand> RC_IntCommand;
	

	/*  */
	public ReactiveCommand RC_SimpleCommand;
	
	}

	
public class StringCommandCommand : ViewModelCommandBase
{

	/*  */
	public string Param0;
	
}

public class IntCommandCommand : ViewModelCommandBase
{

	/*  */
	public int Param0;
	
}

public class SimpleCommandCommand : ViewModelCommandBase
{

}


}