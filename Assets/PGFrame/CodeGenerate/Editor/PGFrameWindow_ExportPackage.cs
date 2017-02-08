using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

namespace PGFrame
{
	using PogoTools;
	using UniRx;

	public partial class PGFrameWindow : EditorWindow
	{
		[MenuItem ("PogoRock/PGFrame/Export Package")]
		static void DoExportPackage ()
		{
			PRDebug.Log ("开始将资源重新打包");
			string[] contents = new string[] {
				Path.Combine ("Assets", "PGFrame"),
				Path.Combine ("Assets", "Plugins/UniRx"),
				Path.Combine ("Assets", "VariousAssets/JsonDotNet"),
				Path.Combine ("Assets", "VariousAssets/PogoRock")
			};
			Debug.Log (JsonConvert.SerializeObject (contents));
			Observable.NextFrame ().Subscribe (_ => {
				AssetDatabase.ExportPackage (contents, string.Format ("PGFrame_{0}.unitypackage", Application.version), ExportPackageOptions.Recurse);
				Debug.Log ("打包结束");
			});
		}
	}
}