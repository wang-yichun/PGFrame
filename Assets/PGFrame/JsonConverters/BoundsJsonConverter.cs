using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BoundsJsonConverter : JsonConverter
{
	#region implemented abstract members of JsonConverter

	public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
	{
		UnityEngine.Bounds bounds = (UnityEngine.Bounds)value;
		JObject jo = new JObject ();
		jo.Add ("c", JToken.FromObject (new {x = bounds.center.x, y = bounds.center.y, z = bounds.center.z}));
		jo.Add ("e", JToken.FromObject (new {x = bounds.extents.x, y = bounds.extents.y, z = bounds.extents.z}));
		serializer.Serialize (writer, jo);
	}

	public override object ReadJson (JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jo = serializer.Deserialize<JObject> (reader);
		UnityEngine.Bounds bounds = new UnityEngine.Bounds (
			                            new Vector3 (
				                            jo ["c"] ["x"].Value<float> (),
				                            jo ["c"] ["y"].Value<float> (),
				                            jo ["c"] ["z"].Value<float> ()
			                            ), 
			                            new Vector3 (
				                            jo ["e"] ["x"].Value<float> (),
				                            jo ["e"] ["y"].Value<float> (),
				                            jo ["e"] ["z"].Value<float> ()
			                            )
		                            );
		return bounds;
	}

	public override bool CanConvert (System.Type objectType)
	{
		return objectType == typeof(UnityEngine.Bounds);
	}

	#endregion
	
}
