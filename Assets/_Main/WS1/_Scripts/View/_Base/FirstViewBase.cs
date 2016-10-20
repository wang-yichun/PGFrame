using UnityEngine;
using UniRx;
using Newtonsoft.Json;

public class FirstViewBase : ViewBase
{
	public FirstViewModel VM;

	public FirstViewModel First {
		get {
			return VM;
		}
	}

	public override ViewModelBase GetViewModel ()
	{
		return VM;
	}

	public bool AutoCreateViewModel = false;

	public string ViewModelInitValueJson;

	public override void Initialize (ViewModelBase viewModel)
	{
		if (viewModel != null) {
			VM = (FirstViewModel)viewModel;
		} else {
			if (AutoCreateViewModel) {
				if (VM == null) {
					if (string.IsNullOrEmpty (ViewModelInitValueJson) == false) {
						VM = JsonConvert.DeserializeObject<FirstViewModel> (ViewModelInitValueJson);
					} else {
						VM = new FirstViewModel ();
					}
				}
			}
		}

		base.Initialize (null);
	}

	public override void Bind ()
	{
		base.Bind ();

		VM.RP_LabelTextNum.Subscribe (OnChanged_LabelTextNum);
		VM.RC_AddNum.Subscribe<AddNumCommand> (OnExecuted_AddNum);
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
	}

	public virtual void OnChanged_LabelTextNum (int value)
	{
	}

	public virtual void OnExecuted_AddNum (AddNumCommand command)
	{
	}
}
