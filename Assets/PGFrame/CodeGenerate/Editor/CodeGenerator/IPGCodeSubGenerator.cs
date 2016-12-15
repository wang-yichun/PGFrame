using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGFrame
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	/// <summary>
	/// 定义代码生成器的接口
	/// </summary>
	public interface IPGCodeSubGenerator
	{
		bool CanGenerate (JObject jo);

		void GenerateCode (JObject jo, IList<string> filesGenerated);
	}
}