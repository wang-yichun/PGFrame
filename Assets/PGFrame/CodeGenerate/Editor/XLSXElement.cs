using UnityEngine;
using System.Collections;
using System.IO;
using System.Data;
using Excel;

public class XLSXElement
{
	public bool toggleOpen;

	private FileInfo fileInfo;

	public FileInfo FileInfo {
		get {
			return fileInfo;
		}
		set {
			fileInfo = value;
			Read ();
		}
	}

	public DataSet ds;

	private void Read ()
	{
		using (FileStream stream = fileInfo.Open (FileMode.Open, FileAccess.Read)) {
			IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader (stream);

			ds = excelReader.AsDataSet ();

			excelReader.Close ();
		}

	}
}
