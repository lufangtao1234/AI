using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Kernel.DecisionTreeLeaning
{
    /// <summary>
    /// C4.5决策树算法
    /// C4.5克服了ID3的2个缺点
    ///     1.信息增益选择属性时偏向于选择分支过多的属性，即取值多的属性
    ///     2.不能处理连续属性
    /// </summary>
    public class C45 : DecisionTree
    {
        /// <summary>
        /// 利用C45决策树算法生成决策树
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="attributes"></param>
        /// <param name="goal"></param>
        /// <param name="positiveExample"></param>
        /// <param name="negativeExample"></param>
        /// <returns></returns>
        public Tree GenerateDecisionTree(DataTable dataTable, Attribute[] attributes, string goal, string positiveExample, string negativeExample)
        {
            _Goal = goal;
            _PositiveExample = positiveExample;
            _NegativeExample = negativeExample;

            //根据训练值添加连续属性值
            _ProcessContinuousAttribute(dataTable, ref attributes);

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

            return null;
        }

        /// <summary>
        /// 根据训练数据集，添加连续属性值
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="attributes"></param>
        private void _ProcessContinuousAttribute(DataTable dataTable, ref Attribute[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
                return;

            for (int i = 0; i < attributes.Length; i++)
            {
                if (!attributes[i].GetIsContinuous())
                    continue;
                _OrderDataTableByAttribute(ref dataTable, attributes[i]);
                
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    
                }
            }
        }

        /// <summary>
        /// 根据连续属性值排序训练集
        /// </summary>
        /// <param name="dataTable">训练集</param>
        /// <param name="attribute">连续属性值</param>
        private void _OrderDataTableByAttribute(ref DataTable dataTable, Attribute attribute)
        {
            dataTable.AsEnumerable().OrderBy(ps => double.Parse((ps as DataRow)[attribute.GetAttributeName()].ToString()));
        }
    }
}
