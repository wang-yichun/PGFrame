namespace PogoRock
{
	using UnityEngine;
	using System.Collections;
	using UnityEditor;
	using System.Diagnostics;

	using System.IO;
	using System.Text;

	public class PogoToolsMenuItem
	{

		[MenuItem ("PogoRock/Rebuild PogoTools")]
		private static void RebuildPogoTools ()
		{
			string app_path = Application.dataPath;
			string command = Path.Combine (new DirectoryInfo (app_path).Parent.FullName, "SubModules/PogoTools/command_autobuild.sh");
			processCommand (command);
		}

		private static void processCommand (string command)
		{
			Process proc = new System.Diagnostics.Process ();
			proc.StartInfo.FileName = "/bin/bash";
			proc.StartInfo.Arguments = command;
			proc.StartInfo.UseShellExecute = false; 
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.StartInfo.WorkingDirectory = new DirectoryInfo (command).Parent.FullName;
			proc.Start ();

			StringBuilder sb = new StringBuilder ();
			while (!proc.StandardError.EndOfStream) {
				sb.AppendLine (proc.StandardError.ReadLine ());
			}
			UnityEngine.Debug.Log (sb.ToString ());

			sb.Length = 0;
			while (!proc.StandardOutput.EndOfStream) {
				string line = proc.StandardOutput.ReadLine ();
				if (line.Trim ().StartsWith ("Done building project")) {
					sb.Append ("<color=#22ff22>");
					sb.Append (line);
					sb.AppendLine ("</color>");
				} else {
					sb.AppendLine (line);
				}
			}
			UnityEngine.Debug.Log (sb.ToString ());

			AssetDatabase.Refresh ();
		}
	}
}