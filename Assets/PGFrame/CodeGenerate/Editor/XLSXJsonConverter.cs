using UnityEngine;
using System.Collections;
using System.Data;
using Excel;

public class XLSXJsonConverter
{
	public XLSXElement Element;

	public void SetElement (XLSXElement element)
	{
		this.Element = element;
	}

	public DataTable Table;

	public void SetDataTable (XLSXElement element, DataTable table)
	{
		this.Element = element;
		this.Table = table;
	}

	public void Convert ()
	{
		
	}
}
