using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RectJsonConverter : JsonConverter
{
	#region implemented abstract members of JsonConverter

	public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
	{
		UnityEngine.Rect rect = (UnityEngine.Rect)value;
		JObject jo = new JObject ();
		jo.Add ("x", rect.x);
		jo.Add ("y", rect.y);
		jo.Add ("w", rect.width);
		jo.Add ("h", rect.height);
		serializer.Serialize (writer, jo);
	}

	public override object ReadJson (JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jo = serializer.Deserialize<JObject> (reader);
		UnityEngine.Rect rect = new Rect (jo ["x"].Value<float> (), jo ["y"].Value<float> (), jo ["w"].Value<float> (), jo ["h"].Value<float> ());
		return rect;
	}

	public override bool CanConvert (System.Type objectType)
	{
		return objectType == typeof(UnityEngine.Rect);
	}

	#endregion
	
}
