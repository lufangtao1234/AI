using ML.DataExchange.DecisionTreeLeaning;
using ML.Kernel.DecisionTreeLeaning;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("是否重新初始化决策树！y or n");
            var yes = System.Console.ReadLine();
            if (yes == "y")
            {
                System.Console.WriteLine("请输入属性个数：");
                int attributeNumber = int.Parse(System.Console.ReadLine());
                List<string> attributes = new List<string>();
                Dictionary<string, List<string>> attributesValue = new Dictionary<string, List<string>>();
                List<List<string>> trainSet = new List<List<string>>();
                for (int i = 0; i < attributeNumber; i++)
                {
                    System.Console.WriteLine("请输入第" + (i + 1) + "个属性的名称");
                    var attribute = System.Console.ReadLine().Trim();
                    attributes.Add(attribute);
                    attributesValue[attribute] = new List<string>();
                }
                for (int i = 0; i < attributeNumber; i++)
                {
                    System.Console.WriteLine("请输入" + attributes[i] + "属性的所有值，以,(英文)隔开");
                    var vaules = System.Console.ReadLine().Trim().Split(',');
                    if (vaules == null || vaules.Length == 0)
                        continue;
                    for (int j = 0; j < vaules.Length; j++)
                        attributesValue[attributes[i]].Add(vaules[j]);
                }
                AttributeExchange.SetAttributes(attributesValue, @"D:\3.xml");
                System.Console.WriteLine("请输入训练集，以,（英文）隔开！用Q退出输入！");
                for (int i = 0; i < attributeNumber; i++)
                    System.Console.Write(attributes[i] + ",     ");
                System.Console.WriteLine("Goal");
                while (true)
                {
                    var line = System.Console.ReadLine();
                    if (line == "Q")
                        break;
                    var lines = line.Split(',');
                    if (lines == null || lines.Length != attributeNumber + 1)
                        continue;
                    trainSet.Add(lines.ToList());
                }

                TrainingSetExchange.SetTrainingSet(trainSet, @"D:\2.txt");
            }
            var t = AttributeExchange.GetAttributes(@"D:\3.xml");
            var table = TrainingSetExchange.GetTrainingSet(@"D:\2.txt", t, "Goal");
            ID3 id3 = new ID3();
            Tree root = id3.GenerateDecisionTree(table, t.ToArray(), "Goal", "yes", "no");
            while (true)
            {
                System.Console.WriteLine("请输入测试样例！");

                var s = System.Console.ReadLine().Trim();
                var slines = s.Split(',');
                System.Console.WriteLine("测试结果为：" + ID3.Decison(root, slines));
            }
           

            //var t = AttributeExchange.GetAttributes(@"D:\1.xml");
            ////foreach (var item in t)
            ////{
            ////    System.Console.WriteLine(item.GetAttributeName());
            ////    foreach (var i in item.GetAttributeValues())
            ////    {
            ////        System.Console.WriteLine(i);
            ////    }
            ////}

            //var table = TrainingSetExchange.GetTrainingSet(@"D:\2.txt", t, "Goal");
            ////foreach (DataRow item in table.Rows)
            ////    for (int i = 0; i < table.Columns.Count; i++)
            ////        System.Console.WriteLine(item[table.Columns[i]]);

            //ID3 id3 = new ID3();
            //Tree root = id3.GenerateDecisionTree(table, t.ToArray(), "Goal", "yes", "no");
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "high", "no", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "high", "no", "excellent" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "high", "no", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "no", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "low", "yes", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "low", "yes", "excellent" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "low", "yes", "excellent" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "medium", "no", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "low", "yes", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "yes", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "medium", "yes", "excellent" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "medium", "no", "excellent" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "high", "yes", "fair" }));
            //System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "no", "excellent" }));
            //Dictionary<string, List<string>> a = new Dictionary<string, List<string>>();
            //a["1"] = new List<string> { "11", "12" };
            //a["2"] = new List<string> { "21", "22" };
            //AttributeExchange.SetAttributes(a, @"D:\3.xml");
        }
    }
}
