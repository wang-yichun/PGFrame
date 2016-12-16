using System.Collections.Generic;

namespace PGFrame
{
	public static class TypeTypeName
	{
		public static Dictionary<TypeType, string[]> dic = new Dictionary<TypeType, string[]> () { { TypeType.System, new string[] { 
					"object", "int", "long", "float", "double", "string", "Object", "DateTime", "TimeSpan"
				} 
			}, { TypeType.Unity, new string[] {
					"UnityEngine.Vector2",
					"UnityEngine.Vector3",
					"UnityEngine.Vector4",
					"UnityEngine.Quaternion",
					"UnityEngine.Rect",
					"UnityEngine.Bounds",
					"UnityEngine.Color",
					"UnityEngine.AnimationCurve"
				}
			}, { TypeType.Other, new string[] {
					"JArray",
					"JObject"
				}
			}
		};
	}
}