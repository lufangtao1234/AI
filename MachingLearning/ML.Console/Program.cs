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
            var t = AttributeExchange.GetAttributes(@"D:\1.xml");
            //foreach (var item in t)
            //{
            //    System.Console.WriteLine(item.GetAttributeName());
            //    foreach (var i in item.GetAttributeValues())
            //    {
            //        System.Console.WriteLine(i);
            //    }
            //}

            var table = TrainingSetExchange.GetTrainingSet(@"D:\2.txt", t, "Goal");
            //foreach (DataRow item in table.Rows)
            //    for (int i = 0; i < table.Columns.Count; i++)
            //        System.Console.WriteLine(item[table.Columns[i]]);

            ID3 id3 = new ID3();
            Tree root = id3.GenerateDecisionTree(table, t.ToArray(), "Goal", "yes", "no");
            System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "high", "no", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "high", "no", "excellent" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "high", "no", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "no", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "low", "yes", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "low", "yes", "excellent" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "low", "yes", "excellent" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "medium", "no", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "low", "yes", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "yes", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "youth", "medium", "yes", "excellent" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "medium", "no", "excellent" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "middle", "high", "yes", "fair" }));
            System.Console.WriteLine(ID3.Decison(root, new string[] { "senior", "medium", "no", "excellent" }));
            System.Console.ReadKey();
        }
    }
}
