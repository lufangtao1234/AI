using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Kernel.DecisionTreeLeaning
{
    /// <summary>
    /// 属性类
    /// </summary>
    public class Attribute
    {
        //属性名称
        private string _AttributeName;
        //属性值
        private ArrayList _AttributeValues;

        /// <summary>
        /// 构造属性
        /// </summary>
        /// <param name="name">属性名</param>
        public Attribute(string name)
        {
            _AttributeName = name;
            _AttributeValues = null;
        }

        /// <summary>
        /// 构造属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="values">属性值</param>
        public Attribute(string name, string[] values)
        {
            _AttributeName = name;
            _AttributeValues = new ArrayList(values);
        }

        /// <summary>
        /// 获得属性名称
        /// </summary>
        /// <returns></returns>
        public string GetAttributeName()
        {
            return _AttributeName;
        }

        /// <summary>
        /// 获得属性值
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAttributeValues()
        {
            return _AttributeValues;
        }

        /// <summary>
        /// 获得属性值在属性中的位置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetValueIndex(string name)
        {
            int index = -1;

            if (_AttributeValues != null)
            {
                for (int i = 0; i < _AttributeValues.Count; i++)
                {
                    if (name == _AttributeValues[i].ToString())
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }
    }
}
