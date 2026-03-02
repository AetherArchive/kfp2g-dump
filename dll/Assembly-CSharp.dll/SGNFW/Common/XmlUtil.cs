using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SGNFW.Common
{
	// Token: 0x02000260 RID: 608
	public static class XmlUtil
	{
		// Token: 0x060025F2 RID: 9714 RVA: 0x001A0C40 File Offset: 0x0019EE40
		public static void Export<Type>(string xmlFilePath, Type exportObject)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(xmlFilePath, false, new UTF8Encoding(false)))
				{
					xmlSerializer.Serialize(streamWriter, exportObject, xmlSerializerNamespaces);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x001A0CBC File Offset: 0x0019EEBC
		public static string ToXmlString<Type>(Type exportObject)
		{
			string text = "";
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			try
			{
				using (StringWriter stringWriter = new XmlUtil.StringWriterUTF8())
				{
					xmlSerializer.Serialize(stringWriter, exportObject, xmlSerializerNamespaces);
					text = stringWriter.ToString();
				}
			}
			catch (Exception)
			{
			}
			return text;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x001A0D40 File Offset: 0x0019EF40
		public static void ExportArray<Type>(string xmlFilePath, Type[] exportArray)
		{
			XmlUtil.Export<Type[]>(xmlFilePath, exportArray);
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x001A0D49 File Offset: 0x0019EF49
		public static void ExportList<Type>(string xmlFilePath, List<Type> exportList)
		{
			XmlUtil.Export<List<Type>>(xmlFilePath, exportList);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x001A0D54 File Offset: 0x0019EF54
		public static Type Import<Type>(string xmlFilePath)
		{
			Type type = default(Type);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
			try
			{
				using (StreamReader streamReader = new StreamReader(xmlFilePath, new UTF8Encoding(false)))
				{
					type = (Type)((object)xmlSerializer.Deserialize(streamReader));
				}
			}
			catch (Exception)
			{
			}
			return type;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x001A0DC4 File Offset: 0x0019EFC4
		public static Type[] ImportArray<Type>(string xmlFilePath)
		{
			return XmlUtil.Import<Type[]>(xmlFilePath);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x001A0DCC File Offset: 0x0019EFCC
		public static List<Type> ImportList<Type>(string xmlFilePath)
		{
			return XmlUtil.Import<List<Type>>(xmlFilePath);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x001A0DD4 File Offset: 0x0019EFD4
		public static Type ImportFromString<Type>(string xmlText)
		{
			Type type = default(Type);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
			try
			{
				using (StringReader stringReader = new StringReader(xmlText))
				{
					type = (Type)((object)xmlSerializer.Deserialize(stringReader));
				}
			}
			catch (Exception)
			{
			}
			return type;
		}

		// Token: 0x0200109E RID: 4254
		private class StringWriterUTF8 : StringWriter
		{
			// Token: 0x17000BF0 RID: 3056
			// (get) Token: 0x06005367 RID: 21351 RVA: 0x00249F00 File Offset: 0x00248100
			public override Encoding Encoding
			{
				get
				{
					return Encoding.UTF8;
				}
			}
		}
	}
}
