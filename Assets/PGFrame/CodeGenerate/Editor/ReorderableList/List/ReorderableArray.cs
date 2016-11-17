using UnityEngine;
using System;
using System.Collections;

namespace Malee {

	[System.Serializable]
	public abstract class ReorderableArray<T> {

		[SerializeField]
		private T[] array = new T[0];

		[SerializeField]
		private int hashCode;

		public ReorderableArray()
			: this(0) {
		}

		public ReorderableArray(int length) {

			array = new T[length];
		}

		public T this[int index] {

			get { return array[index]; }
			set { array[index] = value; }
		}
		
		public int Length {
			
			get { return array.Length; }
		}

		public T[] Array {

			get { return array; }
		}

		public void Add(T value) {

			System.Array.Resize(ref array, array.Length + 1);

			array[array.Length - 1] = value;
		}

		public void Remove(T value) {

			int index = System.Array.IndexOf(array, value);

			if (index >= 0) {

				int length = array.Length;

				T[] newArray = new T[length - 1];

				System.Array.Copy(array, newArray, index);
				System.Array.Copy(array, index + 1, newArray, index, length - (index + 1));

				array = newArray;
			}
		}
	}
}
