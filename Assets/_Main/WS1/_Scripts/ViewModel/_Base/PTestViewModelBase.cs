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
	public class PTestViewModelBase : ViewModelBase
	{
		public PTestViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			
		RP_DefaultProperty1 = new ReactiveProperty<string> ();
		RP_DefaultProperty2 = new ReactiveProperty<string> ();
		RP_DefaultProperty3 = new ReactiveProperty<int> ();
		RP_DefaultProperty4 = new ReactiveProperty<float> ();
		DefaultCollection1 = new ReactiveCollection<string> ();
		DefaultCollection2 = new ReactiveCollection<int> ();
		DefaultDictionary1 = new ReactiveDictionary<string, string> ();
		DefaultDictionary2 = new ReactiveDictionary<int, string> ();
		RC_DefaultCommand1 = new ReactiveCommand<DefaultCommand1Command> ();
		RC_DefaultCommand2 = new ReactiveCommand ();
		RC_DefaultCommand3 = new ReactiveCommand ();
		RC_DefaultCommand4 = new ReactiveCommand ();
		RC_DefaultCommand5 = new ReactiveCommand ();
		RC_DefaultCommand6 = new ReactiveCommand ();
		RC_DefaultCommand7 = new ReactiveCommand ();
		RC_DefaultCommand8 = new ReactiveCommand<DefaultCommand8Command> ();
		RC_DefaultCommand9 = new ReactiveCommand ();
		RC_DefaultCommand10 = new ReactiveCommand ();
		RC_DefaultCommand11 = new ReactiveCommand ();
		RC_DefaultCommand12 = new ReactiveCommand ();
		RC_DefaultCommand13 = new ReactiveCommand ();
		RC_DefaultCommand14 = new ReactiveCommand ();
		RC_DefaultCommand15 = new ReactiveCommand ();
		RC_DefaultCommand16 = new ReactiveCommand ();
		RC_DefaultCommand17 = new ReactiveCommand ();
		RC_DefaultCommand18 = new ReactiveCommand ();
		RC_DefaultCommand19 = new ReactiveCommand ();
		RC_DefaultCommand20 = new ReactiveCommand ();
		RP_CurrentFB = new ReactiveProperty<FBViewModel> ();
		}

		public override void Attach ()
		{
			base.Attach ();
			PTestController.Instance.Attach (this);
		}

		

	/*  */
	public ReactiveProperty<string> RP_DefaultProperty1;

	[JsonProperty]
	public string DefaultProperty1 {
		get {
			return RP_DefaultProperty1.Value;
		}
		set {
			RP_DefaultProperty1.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<string> RP_DefaultProperty2;

	[JsonProperty]
	public string DefaultProperty2 {
		get {
			return RP_DefaultProperty2.Value;
		}
		set {
			RP_DefaultProperty2.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<int> RP_DefaultProperty3;

	[JsonProperty]
	public int DefaultProperty3 {
		get {
			return RP_DefaultProperty3.Value;
		}
		set {
			RP_DefaultProperty3.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<float> RP_DefaultProperty4;

	[JsonProperty]
	public float DefaultProperty4 {
		get {
			return RP_DefaultProperty4.Value;
		}
		set {
			RP_DefaultProperty4.Value = value;
		}
	}

	/*  */
	[JsonProperty] public ReactiveCollection<string> DefaultCollection1;

	/*  */
	[JsonProperty] public ReactiveCollection<int> DefaultCollection2;

	/*  */
	[JsonProperty] public ReactiveDictionary<string, string> DefaultDictionary1;

	/*  */
	[JsonProperty] public ReactiveDictionary<int, string> DefaultDictionary2;

	/*  */
	public ReactiveCommand<DefaultCommand1Command> RC_DefaultCommand1;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand2;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand3;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand4;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand5;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand6;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand7;
	

	/*  */
	public ReactiveCommand<DefaultCommand8Command> RC_DefaultCommand8;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand9;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand10;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand11;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand12;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand13;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand14;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand15;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand16;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand17;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand18;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand19;
	

	/*  */
	public ReactiveCommand RC_DefaultCommand20;
	

	/*  */
	public ReactiveProperty<FBViewModel> RP_CurrentFB;

	
	public FBViewModel CurrentFB {
		get {
			return RP_CurrentFB.Value;
		}
		set {
			RP_CurrentFB.Value = value;
		}
	}
	}

	
public class DefaultCommand1Command : ViewModelCommandBase
{

	/*  */
	public string Param0;
	
	/*  */
	public int Param1;
	
}

public class DefaultCommand2Command : ViewModelCommandBase
{

}

public class DefaultCommand3Command : ViewModelCommandBase
{

}

public class DefaultCommand4Command : ViewModelCommandBase
{

}

public class DefaultCommand5Command : ViewModelCommandBase
{

}

public class DefaultCommand6Command : ViewModelCommandBase
{

}

public class DefaultCommand7Command : ViewModelCommandBase
{

}

public class DefaultCommand8Command : ViewModelCommandBase
{

	/*  */
	public float Param0;
	
}

public class DefaultCommand9Command : ViewModelCommandBase
{

}

public class DefaultCommand10Command : ViewModelCommandBase
{

}

public class DefaultCommand11Command : ViewModelCommandBase
{

}

public class DefaultCommand12Command : ViewModelCommandBase
{

}

public class DefaultCommand13Command : ViewModelCommandBase
{

}

public class DefaultCommand14Command : ViewModelCommandBase
{

}

public class DefaultCommand15Command : ViewModelCommandBase
{

}

public class DefaultCommand16Command : ViewModelCommandBase
{

}

public class DefaultCommand17Command : ViewModelCommandBase
{

}

public class DefaultCommand18Command : ViewModelCommandBase
{

}

public class DefaultCommand19Command : ViewModelCommandBase
{

}

public class DefaultCommand20Command : ViewModelCommandBase
{

}


}