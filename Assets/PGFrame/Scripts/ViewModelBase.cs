using UnityEngine;
using System;
using System.Collections;

public abstract class ViewModelBase
{
	public Guid VMID;

	public ViewModelBase ()
	{
		VMID = Guid.NewGuid ();

		Initialize ();

		Attach ();
	}

	public abstract void Initialize ();

	public abstract void Attach ();
}
