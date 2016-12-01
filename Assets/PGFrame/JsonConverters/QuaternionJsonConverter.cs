using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuaternionJsonConverter : JsonConverter
{
	#region implemented abstract members of JsonConverter

	public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
	{
		UnityEngine.Quaternion quaternion = (UnityEngine.Quaternion)value;
		JObject jo = new JObject ();
		jo.Add ("x", quaternion.x);
		jo.Add ("y", quaternion.y);
		jo.Add ("z", quaternion.z);
		jo.Add ("w", quaternion.w);
		serializer.Serialize (writer, jo);
	}

	public override object ReadJson (JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jo = serializer.Deserialize<JObject> (reader);
//		UnityEngine.Color color = new Color (jo ["r"].Value<float> (), jo ["g"].Value<float> (), jo ["b"].Value<float> (), jo ["a"].Value<float> ());
		UnityEngine.Quaternion quaternion = new Quaternion (
			                                    jo ["x"].Value<float> (),
			                                    jo ["y"].Value<float> (),
			                                    jo ["z"].Value<float> (),
			                                    jo ["w"].Value<float> ()
		                                    );
		return quaternion;
	}

	public override bool CanConvert (System.Type objectType)
	{
		return objectType == typeof(UnityEngine.Color);
	}

	#endregion
	
}
