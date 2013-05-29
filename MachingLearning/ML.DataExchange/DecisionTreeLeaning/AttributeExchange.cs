using System;
using System.Collections.Generic;
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
    }
}
