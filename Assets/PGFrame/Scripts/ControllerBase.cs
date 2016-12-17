namespace PGFrame
{

	public class ControllerBase<T> : Singleton<T>
		where T: Singleton<T>, new()
	{

		public virtual void Attach (ViewModelBase viewModel)
		{
		}

		public virtual void Detach (ViewModelBase viewModel)
		{
			if (viewModel.baseAttachDisposables != null) {
				viewModel.baseAttachDisposables.Dispose ();
				viewModel.baseAttachDisposables = null;
			}
		}
	}

}