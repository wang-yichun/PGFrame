using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class ViewModelCommandBase
{
	[JsonIgnore]
	public ViewModelBase Sender { get; set; }
}
