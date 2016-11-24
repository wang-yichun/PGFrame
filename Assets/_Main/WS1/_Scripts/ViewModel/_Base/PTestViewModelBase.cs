using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

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
		
	}

	public override void Attach ()
	{
		base.Attach ();
		PTestController.Instance.Attach (this);
	}

	
}

