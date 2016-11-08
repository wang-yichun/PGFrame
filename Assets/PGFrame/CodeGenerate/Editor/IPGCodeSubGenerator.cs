using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 定义代码生成器的接口
/// </summary>
public interface IPGCodeSubGenerator
{
	bool CanGenerate (string workspaceName, string elementName);

	void GenerateCode (string workspaceName, string elementName, IList<string> filesGenerated);
}
