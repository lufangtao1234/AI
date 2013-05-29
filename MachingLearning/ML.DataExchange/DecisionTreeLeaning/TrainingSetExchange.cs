using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.DataExchange.DecisionTreeLeaning
{
    /// <summary>
    /// 训练集数据转换
    /// </summary>
    public static class TrainingSetExchange
    {
        /// <summary>
        /// 获得训练数据集
        /// 训练集以英文,隔开
        /// </summary>
        /// <param name="filePath">训练数据文件路径</param>
        /// <param name="attributes">属性值</param>
        /// <param name="goal">目标值</param>
        /// <returns>训练集</returns>
        public static DataTable GetTrainingSet(string filePath, List<ML.Kernel.DecisionTreeLeaning.Attribute> attributes, string goal)
        {
            DataTable dataTable = new DataTable("TrainingSet");
            DataColumn column;
            DataRow row;

            foreach (var attribute in attributes)
            {
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = attribute.GetAttributeName();
                dataTable.Columns.Add(column);
            }
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = goal;
            dataTable.Columns.Add(column);

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var attributesValue = line.Split(',');
                row = dataTable.NewRow();
                List<string> rowsData = new List<string>();
                for (int i = 0; i < attributesValue.Length; i++)
                    rowsData.Add(attributesValue[i].Trim());
                dataTable.Rows.Add(rowsData.ToArray());
            }
            return dataTable;
        }

        /// <summary>
        /// 生成训练集
        /// </summary>
        /// <param name="trainingSets">训练集数据（对应到属性）</param>
        /// <param name="filePath">文件路径</param>
        public static void SetTrainingSet(List<List<string>> trainingSets, string filePath)
        {
            if (trainingSets == null || trainingSets.Count == 0)
                return;

            if (File.Exists(filePath))
                File.Delete(filePath);
            StreamWriter sw = File.CreateText(filePath);
            foreach (var trainingSet in trainingSets)
            {
                if (trainingSet == null || trainingSet.Count == 0)
                    continue;
                string content = "";
                for (int i = 0; i < trainingSet.Count; i++)
                {
                    if (i == trainingSet.Count - 1)
                        content += trainingSet[i];
                    else
                        content += trainingSet[i] + ",";
                }

                sw.WriteLine(content);
            }
            sw.Close();

        }
    }
}
