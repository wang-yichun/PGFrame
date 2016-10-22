using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPGCodeSubGenerator
{
	bool CanGenerate (string workspaceName, string elementName);

	void GenerateCode (string workspaceName, string elementName, IList<string> filesGenerated);
}
