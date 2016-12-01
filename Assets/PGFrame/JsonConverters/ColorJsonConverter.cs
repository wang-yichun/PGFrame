using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ColorJsonConverter : JsonConverter
{
	#region implemented abstract members of JsonConverter

	public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
	{
		UnityEngine.Color color = (UnityEngine.Color)value;
		JObject jo = new JObject ();
		jo.Add ("r", color.r);
		jo.Add ("g", color.g);
		jo.Add ("b", color.b);
		jo.Add ("a", color.a);
		serializer.Serialize (writer, jo);
	}

	public override object ReadJson (JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jo = serializer.Deserialize<JObject> (reader);
		UnityEngine.Color color = new Color (jo ["r"].Value<float> (), jo ["g"].Value<float> (), jo ["b"].Value<float> (), jo ["a"].Value<float> ());
		return color;
	}

	public override bool CanConvert (System.Type objectType)
	{
		return objectType == typeof(UnityEngine.Color);
	}

	#endregion
	
}
