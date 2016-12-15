using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS2 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	  
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class BallViewModelBase : ViewModelBase
	{
		public BallViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			
		RP_Type = new ReactiveProperty<BallType> ();
		}

		public override void Attach ()
		{
			base.Attach ();
			BallController.Instance.Attach (this);
		}

		

	/*  */
	public ReactiveProperty<BallType> RP_Type;

	[JsonProperty]
	public BallType Type {
		get {
			return RP_Type.Value;
		}
		set {
			RP_Type.Value = value;
		}
	}
	}

	

}