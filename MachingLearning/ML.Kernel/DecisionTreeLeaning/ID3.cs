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
    public class ID3
    {
        //目标值
        private string _Goal;
        //正例
        private string _PositiveExample;
        //反例 
        private string _NegativeExample;
        //训练集
        private DataTable _TrainingSet;
        //训练集数据个数 
        private int _TotalTraningSetCount = 0;
        //正例个数
        private int _PositiveExampleCount = 0;
        //反例个数
        private int _NegativeExampleCount = 0;
        //训练集的总熵
        private double _EntropyTotal;

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
                        postiveExampleCount++;
                    else
                        negativeExampleCount++;
                }
            }
        }
        /// <summary>
        /// 计算熵
        /// </summary>
        /// <param name="positiveExampleCount">正例个数</param>
        /// <param name="negativeExampleCount">反例个数</param>
        /// <returns></returns>
        private double CaluteEntropy(int positiveExampleCount, int negativeExampleCount)
        {
            double entropy;
            int total = positiveExampleCount + negativeExampleCount;
            double positiveRatio = Convert.ToDouble(positiveExampleCount) / total;
            double negativeRatio = Convert.ToDouble(negativeExampleCount) / total;

            if (positiveRatio != 0)
                positiveRatio = -(positiveRatio) * System.Math.Log(positiveRatio, 2);
            if (negativeRatio != 0)
                negativeRatio = -(negativeRatio) * System.Math.Log(negativeRatio, 2);

            return entropy = positiveRatio + negativeRatio;
        }


        /// <summary>
        /// 统计个数
        /// </summary>
        /// <param name="dataTable">数据集</param>
        /// <param name="example">正例或反例</param>
        /// <returns></returns>
        private int _ExampleCounter(DataTable dataTable, string example)
        {
            int count = 0;

            foreach (DataRow row in dataTable.Rows)
                if (row[_Goal].ToString() == example)
                    count++;
            return count;
        }

        /// <summary>
        /// 检查训练集是否属于同一类
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public bool IsSameClass(DataTable dataTable)
        {
            bool flag = false;
            int sum = dataTable.Rows.Count;

            int positiveExampleCount = 0;
            int negativeExampleCount = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[_Goal].ToString() == _PositiveExample)
                    positiveExampleCount++;
                else
                    negativeExampleCount++;
            }

            if ((positiveExampleCount == sum) || (negativeExampleCount == sum))
                flag = true;
            return flag;
        }
    }
}
