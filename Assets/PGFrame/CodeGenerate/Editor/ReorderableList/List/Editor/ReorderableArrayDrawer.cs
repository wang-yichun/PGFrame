using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Malee.Editor {

	[CustomPropertyDrawer(typeof(ReorderableAttribute))]
	public class ReorderableArrayDrawer : PropertyDrawer {

		private Dictionary<int, ReorderableList> lists = new Dictionary<int, ReorderableList>();

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

			ReorderableList list = GetList(property);

			if (list != null) {

				return list.GetListHeight();
			}
			else {

				return EditorGUIUtility.singleLineHeight;
			}
		}		

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			ReorderableList list = GetList(property);

			if (list != null) {

				list.DoList(EditorGUI.IndentedRect(position), label);
			}
			else {

				GUI.Label(position, "Array must extend from ReorderableArray");
			}
		}

		//
		// -- PRIVATE --
		//

		private ReorderableList GetList(SerializedProperty property) {

			SerializedProperty array;
			SerializedProperty id;

			ReorderableList list = null;

			if (IsValid(property, out array, out id)) {
				
				ReorderableAttribute attrib = attribute as ReorderableAttribute;

				if (attrib != null) {

					if (!lists.TryGetValue(id.intValue, out list)) {

						list = new ReorderableList(array, attrib.add, attrib.remove, attrib.draggable, ReorderableList.ElementDisplayType.Auto, attrib.elementNameProperty, attrib.elementIconPath);
						lists.Add(list.id, list);

						id.intValue = list.id;
					}
					else {

						list.List = array;
					}
				}
			}

			return list;
		}

		private bool IsValid(SerializedProperty property, out SerializedProperty array, out SerializedProperty id) {

			array = property.FindPropertyRelative("array");
			id = property.FindPropertyRelative("hashCode");

			return array != null && id != null ? array.isArray && id.propertyType == SerializedPropertyType.Integer : false;
		}
	}
}