using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Kernel.DecisionTreeLeaning
{
    /// <summary>
    /// 决策树基类
    /// </summary>
    public class DecisionTree
    {
        //目标值
        protected string _Goal;
        //正例
        protected static string _PositiveExample;
        //反例 
        protected static string _NegativeExample;
        //训练集
        protected DataTable _TrainingSet;
        //训练集数据个数 
        protected int _TotalTraningSetCount = 0;
        //正例个数
        protected int _PositiveExampleCount = 0;
        //反例个数
        protected int _NegativeExampleCount = 0;
        //训练集的总熵
        protected double _EntropyTotal;
        //属性值
        protected Attribute[] _Attributes;

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

        /// <summary>
        /// 统计个数
        /// </summary>
        /// <param name="dataTable">数据集</param>
        /// <param name="example">正例或反例</param>
        /// <returns></returns>
        protected int _ExampleCounter(DataTable dataTable, string example)
        {
            int count = 0;

            foreach (DataRow row in dataTable.Rows)
                if (row[_Goal].ToString() == example)
                    count++;
            return count;
        }

        /// <summary>
        /// 计算熵
        /// </summary>
        /// <param name="positiveExampleCount">正例个数</param>
        /// <param name="negativeExampleCount">反例个数</param>
        /// <returns></returns>
        protected double CaluteEntropy(int positiveExampleCount, int negativeExampleCount)
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

    }
}
