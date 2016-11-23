using UnityEngine;

public class ViewBase : MonoBehaviour
{

	public bool AutoCreateViewModel = false;

	public string ViewModelInitValueJson;

	void Awake ()
	{
		Initialize (null);
	}

	public virtual void CreateViewModel ()
	{
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
