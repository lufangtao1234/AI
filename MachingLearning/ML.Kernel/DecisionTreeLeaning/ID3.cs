using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Kernel.DecisionTreeLeaning
{
    /// <summary>
    /// ID3算法
    /// </summary>
    public class ID3 :DecisionTree
    {    
        /// <summary>
        /// 由ID3算法生成决策树
        /// </summary>
        /// <param name="dataTable">训练集</param>
        /// <param name="attributes">属性集合</param>
        /// <param name="goal">目标值</param>
        /// <param name="positiveExample">正例目标值</param>
        /// <param name="negativeExample">反例目标值</param>
        /// <returns></returns>
        public Tree GenerateDecisionTree(DataTable dataTable, Attribute[] attributes, string goal, string positiveExample, string negativeExample)
        {
            _Goal = goal;
            _PositiveExample = positiveExample;
            _NegativeExample = negativeExample;

            //检查是否目标为一类
            if (IsSameClass(dataTable) == true)
                return new Tree(new Attribute(dataTable.Rows[0][_Goal].ToString()));

            if (attributes.Length == 0)
            {
                int yes = 0;
                int no = 0;

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][_Goal].ToString() == _PositiveExample)
                        yes++;
                    if (dataTable.Rows[i][_Goal].ToString() == _NegativeExample)
                        no++;
                }

                if (yes > no)
                    return new Tree(new Attribute(_PositiveExample));
                else
                    return new Tree(new Attribute(_NegativeExample));
            }

            _TrainingSet = dataTable;
            _TotalTraningSetCount = dataTable.Rows.Count;
            _PositiveExampleCount = _ExampleCounter(dataTable, _PositiveExample);
            _NegativeExampleCount = _ExampleCounter(dataTable, _NegativeExample);

            _EntropyTotal = CaluteEntropy(_PositiveExampleCount, _NegativeExampleCount);

            Attribute bestSplittingAttribute = AttributeSelectionMethod(dataTable, attributes);
           
            Tree root = new Tree(bestSplittingAttribute);
            DataTable dtCopy = dataTable.Clone();
            if (bestSplittingAttribute == null)
                return root;
            ArrayList valueList = new ArrayList();
            valueList = bestSplittingAttribute.GetAttributeValues();
            for (int i = 0; i < valueList.Count; i++)
            {
                dtCopy.Rows.Clear();
                //选出属性值对应的元组数据
                DataRow[] rows = dataTable.Select(bestSplittingAttribute.GetAttributeName() + " = " + "'" + valueList[i].ToString() + "'");
                //将选出的数据拷贝到dt的副本里
                foreach (DataRow row in rows)
                    dtCopy.Rows.Add(row.ItemArray);
                ArrayList attributeListCopy = new ArrayList(attributes.Length - 1);
                for (int j = 0; j < attributes.Length; j++)
                    if (attributes[j].GetAttributeName() != bestSplittingAttribute.GetAttributeName())
                        attributeListCopy.Add(attributes[j]);

                if (dtCopy.Rows.Count == 0)
                {
                    int yes = 0;
                    int no = 0;
                    for (int k = 0; k < dataTable.Rows.Count; k++)
                    {
                        if (dataTable.Rows[k][_Goal].ToString() == _PositiveExample)
                            yes++;
                        if (dataTable.Rows[k][_Goal].ToString() == _NegativeExample)
                            no++;
                    }

                    if (yes > no)
                        return new Tree(new Attribute(_NegativeExample));
                    else
                        return new Tree(new Attribute(_PositiveExample));

                }
                else
                {
                    ID3 id3 = new ID3();
                    Tree child = id3.GenerateDecisionTree(dtCopy, (Attribute[])attributeListCopy.ToArray(typeof(Attribute)), _Goal, _PositiveExample, _NegativeExample);
                    root.AddNode(child, valueList[i].ToString());
                }
            }

            return root;
        }

        /// <summary>
        /// 选择信息增益最高的属性的方法
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private Attribute AttributeSelectionMethod(DataTable dataTable, Attribute[] attributes)
        {
            Attribute selectAttribute = null;
            double max = 0;

            foreach (Attribute attribute in attributes)
            {
                double gainAttribute = Gain(dataTable, attribute);
                if (gainAttribute > max)
                {
                    max = gainAttribute;
                    selectAttribute = attribute;
                }
            }

            return selectAttribute;

        }
        /// <summary>
        /// 计算信息增益
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public double Gain(DataTable dataTable, Attribute attribute)
        {
            double entropy = 0;
            double result = 0;
            string[] values = new string[attribute.GetAttributeValues().Count];

            ArrayList temp = attribute.GetAttributeValues();

            for (int i = 0; i < attribute.GetAttributeValues().Count; i++)
                values[i] = temp[i].ToString();

            for (int i = 0; i < values.Length; i++)
            {
                int yes = 0;
                int no = 0;

                CountByValue(dataTable, attribute, values[i], ref yes, ref no);

                entropy = CaluteEntropy(yes, no);
                result += -Convert.ToDouble(yes + no) / _TotalTraningSetCount * entropy;
            }
            return _EntropyTotal + result;
        }

        /// <summary>
        /// 统计正反例个数
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="postiveExampleCount"></param>
        /// <param name="negativeExampleCount"></param>
        public void CountByValue(DataTable dataTable, Attribute attribute, string value, ref int postiveExampleCount, ref int negativeExampleCount)
        {
            postiveExampleCount = 0;
            negativeExampleCount = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[attribute.GetAttributeName()].ToString() == value)
                {
                    if (row[_Goal].ToString() == _NegativeExample)
                        negativeExampleCount++;
                    else
                        postiveExampleCount++;
                }
            }
        }

        /// <summary>
        /// 利用决策树做判断
        /// </summary>
        /// <param name="root">决策树</param>
        /// <param name="searchStr">决策数组</param>
        /// <returns></returns>
        public static string Decison(Tree root, string[] searchStr)
        {

            if (root.GetAttribute().GetAttributeValues() != null)
            {
                for (int i = 0; i < root.GetAttribute().GetAttributeValues().Count; i++)
                {
                    for (int j = 0; j < searchStr.Length; j++)
                    {
                        if (root.GetAttribute().GetAttributeValues()[i].ToString().ToUpper() == searchStr[j].ToString().ToUpper())
                        {
                            Tree childNode = root.GetChild(root.GetAttribute().GetAttributeValues()[i].ToString());

                            if ((childNode.GetAttribute().GetAttributeName().ToString() == _NegativeExample) || (childNode.GetAttribute().GetAttributeName().ToString() == _PositiveExample))
                            {
                                if (childNode.GetAttribute().GetAttributeName() == _PositiveExample)
                                    return _PositiveExample;
                                else
                                    return _NegativeExample;
                            }
                            else
                                return Decison(childNode, searchStr);
                        }
                    }
                }
            }
            return _NegativeExample;
        }
    }
}
