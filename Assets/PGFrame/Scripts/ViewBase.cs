using UnityEngine;

public class ViewBase : MonoBehaviour
{
	[SerializeField, HideInInspector]
	public bool AutoCreateViewModel = false;

	[SerializeField, HideInInspector]
	private string viewModelInitValueJson;

	public string ViewModelInitValueJson {
		get {
			return viewModelInitValueJson;
		}
		set {
			viewModelInitValueJson = value;
			VMJsonSize = viewModelInitValueJson.Length;
		}
	}

	[SerializeField, HideInInspector]
	public int VMJsonSize;

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
