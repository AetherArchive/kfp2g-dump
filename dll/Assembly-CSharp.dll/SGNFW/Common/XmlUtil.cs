using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SGNFW.Common
{
	public static class XmlUtil
	{
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

		public static void ExportArray<Type>(string xmlFilePath, Type[] exportArray)
		{
			XmlUtil.Export<Type[]>(xmlFilePath, exportArray);
		}

		public static void ExportList<Type>(string xmlFilePath, List<Type> exportList)
		{
			XmlUtil.Export<List<Type>>(xmlFilePath, exportList);
		}

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

		public static Type[] ImportArray<Type>(string xmlFilePath)
		{
			return XmlUtil.Import<Type[]>(xmlFilePath);
		}

		public static List<Type> ImportList<Type>(string xmlFilePath)
		{
			return XmlUtil.Import<List<Type>>(xmlFilePath);
		}

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

		private class StringWriterUTF8 : StringWriter
		{
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
