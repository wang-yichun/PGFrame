using UniRx;

public class ControllerBase<T> : Singleton<T>
	where T: Singleton<T>, new()
{

	public virtual void Attach (ViewModelBase viewModel)
	{
		UnityEngine.Debug.Log ("ControllerBase.Attach");
	}
}
