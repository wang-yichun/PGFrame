using UnityEngine;
using System;

namespace PGFrame
{
	using UniRx;

	public class ViewBase : MonoBehaviour, IDisposable
	{
	
		[SerializeField, HideInInspector]
		public bool AutoCreateViewModel = false;

		[SerializeField, HideInInspector]
		public bool UseEmptyViewModel = true;

		[SerializeField, HideInInspector]
		public string ViewBaseKey = "";

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

		void OnDestroy ()
		{
			Dispose ();
		}

		public virtual void CreateViewModel ()
		{
		}

		public virtual ViewModelBase GetViewModel ()
		{
			return null;
		}

		public virtual void SetViewModel (ViewModelBase viewModel)
		{
		}

		public virtual void Initialize (ViewModelBase viewModel)
		{
			baseBindDisposables = new CompositeDisposable ();
			ViewModelBase VM = GetViewModel ();
			if (VM != null) {
				BeforeBind ();
				Bind ();
				AfterBind ();
			}
		}

		public void Dispose ()
		{
			ViewModelBase VM = GetViewModel ();
			if (VM != null) {
				Unbind ();
				VM.Detach ();
				VM.Dispose ();
				SetViewModel (null);
			}
			baseBindDisposables = null;
		}

		public CompositeDisposable baseBindDisposables;

		public virtual void BeforeBind ()
		{
		}

		public virtual void Bind ()
		{
		}

		public virtual void AfterBind ()
		{
		}

		public virtual void Unbind ()
		{
			if (baseBindDisposables != null) {
				baseBindDisposables.Dispose ();
				baseBindDisposables = null;
			}
		}
	}

}