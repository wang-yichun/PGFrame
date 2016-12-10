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
}
