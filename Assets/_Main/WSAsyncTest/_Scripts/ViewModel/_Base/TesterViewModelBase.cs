using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	  
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class TesterViewModelBase : ViewModelBase
	{
		public TesterViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			
			RP_CounterEnable = new ReactiveProperty<bool> ();
			RP_CountValue = new ReactiveProperty<int> ();
			RC_SwitchCounter = new ReactiveCommand ();
			
			base.Initialize ();
		}

		public override void Attach ()
		{
			base.Attach ();
			TesterController.Instance.Attach (this);
		}

		

		/*  */
		public ReactiveProperty<bool> RP_CounterEnable;

		[JsonProperty]
		public bool CounterEnable {
			get {
				return RP_CounterEnable.Value;
			}
			set {
				RP_CounterEnable.Value = value;
			}
		}

		/*  */
		public ReactiveProperty<int> RP_CountValue;

		[JsonProperty]
		public int CountValue {
			get {
				return RP_CountValue.Value;
			}
			set {
				RP_CountValue.Value = value;
			}
		}

		/*  */
		public ReactiveCommand RC_SwitchCounter;
		
	}

	
	public class SwitchCounterCommand : ViewModelCommandBase
	{
	
	}
	

}