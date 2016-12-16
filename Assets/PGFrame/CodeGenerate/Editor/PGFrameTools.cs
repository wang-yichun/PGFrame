using System.Collections.Generic;
using System.Linq;

namespace PGFrame
{
	public static class PGFrameTools
	{
		public static DocType? GetDocTypeByWorkspaceAndType (string sw_name, string type_name)
		{
			DocType? dt = null;
			string[] ts = type_name.Split (new char[]{ '.' });
			string workspace = "";
			string single_name = "";
			if (ts.Length == 1) {
				workspace = sw_name;
				single_name = ts [0];
			} else if (ts.Length == 2) {
				workspace = ts [0];
				single_name = ts [1];
			}

			if (string.IsNullOrEmpty (workspace) == false && string.IsNullOrEmpty (single_name) == false) {
				if (PGFrameWindow.Current != null) {
					dt = PGFrameWindow.Current.CommonManager.GetTheDocTypeByName (workspace, single_name);
				}
			}

			return dt;
		}

		public static TypeType? GetTypeTypeByType (string type_name)
		{
			TypeType? result = null;
			foreach (TypeType key in TypeTypeName.dic.Keys) {
				string[] values = TypeTypeName.dic [key];
				if (values.Contains (type_name)) {
					result = key;
					break;
				}
			}
			return result;
		}

		public static string[] SplitWorkspaceAndTypeName (string sw_name, string type_name)
		{
			List<string> ts = type_name.Split (new char[]{ '.' }).ToList ();
			string[] result = new string[2];
			if (ts.Count == 0) {
				result [0] = "";
				result [1] = "";
			} else if (ts.Count == 1) {
				result [0] = sw_name;
				result [1] = ts [0];
			} else {
				result [1] = ts.Last ();
				ts.RemoveAt (ts.Count - 1);
				result [0] = string.Join (".", ts.ToArray ());
			}
			return result;
		}

		public static string ConvertToElementName (this string viewModelName)
		{
			if (viewModelName.Contains ("ViewModel")) {
				int idx_need_cut = viewModelName.LastIndexOf ("ViewModel");
				return viewModelName.Substring (0, idx_need_cut);
			}
			return null;
		}
	}
}
