using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	  
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class FBViewModelBase : ViewModelBase
	{
		public FBViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			
			RP_Count = new ReactiveProperty<int> ();
			RC_FBTestCMD = new ReactiveCommand ();
			
			base.Initialize ();
		}

		public override void Attach ()
		{
			base.Attach ();
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

		/*  */
		public ReactiveCommand RC_FBTestCMD;
		
	}

	
	public class FBTestCMDCommand : ViewModelCommandBase
	{
	
	}
	

}