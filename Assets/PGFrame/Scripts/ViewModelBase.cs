using UnityEngine;
using System.Collections;

public abstract class ViewModelBase
{
	public ViewModelBase ()
	{
		Initialize ();

		Attach ();
	}

	public abstract void Initialize ();

	public abstract void Attach ();
}
