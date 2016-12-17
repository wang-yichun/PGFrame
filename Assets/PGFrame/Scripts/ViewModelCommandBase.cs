using UnityEngine;
using System.Collections;

namespace PGFrame
{

	using Newtonsoft.Json;

	public class ViewModelCommandBase
	{
		[JsonIgnore]
		public ViewModelBase Sender { get; set; }
	}

}