using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

////////////////////////////////////////////////////////////////////////////////
//  
////////////////////////////////////////////////////////////////////////////////
[JsonObjectAttribute (MemberSerialization.OptIn)]
public class FBViewModelBase : ViewModelBase
{
	public FBViewModelBase ()
	{
	}

	public override void Initialize ()
	{
		
		RP_Count = new ReactiveProperty<int> ();
	}

	public override void Attach ()
	{
		FBController.Instance.Attach (this);
	}

	

	/*  */
	public ReactiveProperty<int> RP_Count;

	[JsonProperty]
	public int Count {
		get {
			return RP_Count.Value;
		}
		set {
			RP_Count.Value = value;
		}
	}
}

