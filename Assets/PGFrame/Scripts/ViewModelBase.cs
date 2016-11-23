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

	public virtual void Initialize ()
	{
	}

	public virtual void Attach ()
	{
	}
}
