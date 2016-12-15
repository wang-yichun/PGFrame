using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	  
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class PlayerInfoViewModelBase : ViewModelBase
	{
		public PlayerInfoViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			
		RP_Name = new ReactiveProperty<string> ();
		RP_Score = new ReactiveProperty<int> ();
		}

		public override void Attach ()
		{
			base.Attach ();
			PlayerInfoController.Instance.Attach (this);
		}

		

	/*  */
	public ReactiveProperty<string> RP_Name;

	[JsonProperty]
	public string Name {
		get {
			return RP_Name.Value;
		}
		set {
			RP_Name.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<int> RP_Score;

	[JsonProperty]
	public int Score {
		get {
			return RP_Score.Value;
		}
		set {
			RP_Score.Value = value;
		}
	}
	}

	

}