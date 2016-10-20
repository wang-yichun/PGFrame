using UnityEngine;

public class ViewBase : MonoBehaviour
{
	void Awake ()
	{
		Initialize (null);
	}

	public virtual ViewModelBase GetViewModel ()
	{
		return null;
	}

	public virtual void Initialize (ViewModelBase viewModel)
	{

		if (GetViewModel () != null) {
			Bind ();
			AfterBind ();
		}
	}

	public virtual void Bind ()
	{
	}

	public virtual void AfterBind ()
	{
	}
}
