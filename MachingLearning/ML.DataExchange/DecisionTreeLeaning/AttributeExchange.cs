using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ML.DataExchange.DecisionTreeLeaning
{
    /// <summary>
    /// 属性数据转换
    /// </summary>
    public static class AttributeExchange
    {
        /// <summary>
        /// 通过XML获得属性值
        /// XML格式为
        /// <Attributes>
        ///     <Attribute Key="Income">
        ///     <Value>high</Value>
        ///     <Value>medium</Value>
        ///     <Value>low</Value>
        ///     </Attribute>
        /// </Attributes>
        /// </summary>
        /// <param name="xmlFilePath">attribute文件路径</param>
        /// <returns>attribute集合</returns>
        public static List<ML.Kernel.DecisionTreeLeaning.Attribute> GetAttributes(string xmlFilePath)
        {
            List<ML.Kernel.DecisionTreeLeaning.Attribute> attributes = new List<Kernel.DecisionTreeLeaning.Attribute>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("Attributes").ChildNodes;
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Name == "Attribute")
                {
                    string attriuteName = node.Attributes["Key"].Value;
                    var valuesChild = node.ChildNodes;
                    List<string> values = new List<string>();
                    foreach (XmlNode childNode in valuesChild)
                        if (childNode.Name == "Value")
                            values.Add(childNode.InnerText);
                    attributes.Add(new ML.Kernel.DecisionTreeLeaning.Attribute(attriuteName, values.ToArray()));
                }
            }
            return attributes;
        }
        /// <summary>
        /// 生成属性文件
        /// </summary>
        /// <param name="attributes">属性值</param>
        /// <param name="filePath">存储路径</param>
        public static void SetAttributes(Dictionary<string, List<string>> attributes, string filePath)
        {
            if (attributes == null || attributes.Keys.Count == 0)
                return;

            if (File.Exists(filePath))
                File.Delete(filePath);
            StreamWriter sw = File.CreateText(filePath);
            sw.WriteLine("<Attributes></Attributes>");
            sw.Close();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode root = xmlDoc.SelectSingleNode("Attributes");
            foreach (var key in attributes.Keys)
            {
                XmlElement xe1 = xmlDoc.CreateElement("Attribute");
                xe1.SetAttribute("Key", key);
                if (attributes[key] == null || attributes[key].Count == 0)
                    continue;
                foreach (var value in attributes[key])
                {
                    XmlElement xe11 = xmlDoc.CreateElement("Value");
                    xe11.InnerText = value;
                    xe1.AppendChild(xe11);
                }
                root.AppendChild(xe1);
            }
            xmlDoc.Save(filePath);
        }
    }
}
