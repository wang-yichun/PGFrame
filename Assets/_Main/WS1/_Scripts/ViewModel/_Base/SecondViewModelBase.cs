using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

////////////////////////////////////////////////////////////////////////////////
// 一个 element 的定义 
////////////////////////////////////////////////////////////////////////////////
[JsonObjectAttribute (MemberSerialization.OptIn)]
public class SecondViewModelBase : ViewModelBase
{
	public SecondViewModelBase ()
	{
	}

	public override void Initialize ()
	{
		
		RP_LabelTextNum = new ReactiveProperty<int> ();
		Numbers = new ReactiveCollection<int> ();
		MyDictionary = new ReactiveDictionary<string, string> ();
		RP_DefaultProperty1 = new ReactiveProperty<object> ();
	}

	public override void Attach ()
	{
		SecondController.Instance.Attach (this);
	}

	

	/* Label上的文字 */
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

	/* 一个整形数组 */
	[JsonProperty] public ReactiveCollection<int> Numbers;

	/* 我的字典 */
	[JsonProperty] public ReactiveDictionary<string, string> MyDictionary;

	/*  */
	public ReactiveProperty<object> RP_DefaultProperty1;

	[JsonProperty]
	public object DefaultProperty1 {
		get {
			return RP_DefaultProperty1.Value;
		}
		set {
			RP_DefaultProperty1.Value = value;
		}
	}
}

