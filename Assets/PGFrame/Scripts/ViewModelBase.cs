using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PGFrame
{
	
	public abstract class ViewModelBase
	{
		public Guid VMID;

		public static readonly string DefaultViewBaseKey = "";

		public ViewModelBase ()
		{
			VMID = Guid.NewGuid ();
			HostViews = new Dictionary<string, ViewBase> ();

			Initialize ();

			Attach ();
		}

		public virtual void Initialize ()
		{
		}

		public virtual void Attach ()
		{
		}

		public Dictionary<string, ViewBase> HostViews;

		public T GetView<T> (string key = null)
		where T : ViewBase
		{
			if (key == null)
				key = DefaultViewBaseKey;
		
			ViewBase viewBase;
			if (HostViews.TryGetValue (key, out viewBase)) {
				T tViewBase = Convert.ChangeType (viewBase, typeof(T)) as T;
				return tViewBase;
			}
			return default(T);
		}

		public virtual bool AddHostView (string key, ViewBase hostView)
		{
			if (HostViews.ContainsKey (key)) {
				return false;
			} else {
				HostViews.Add (key, hostView);
			}
			return true;
		}

		public virtual bool RemoveHostView (string key)
		{
			if (HostViews.ContainsKey (key)) {
				return false;
			} else {
				HostViews.Remove (key);
			}
			return true;
		}
	}

}