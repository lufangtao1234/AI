using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Kernel.DecisionTreeLeaning
{
    /// <summary>
    /// 树
    /// </summary>
    public class Tree
    {
        // 父节点
        private Attribute _Node;
        //子结点
        private ArrayList _Children;

        /// <summary>
        /// 构造树
        /// </summary>
        /// <param name="attribute">属性值</param>
        public Tree(Attribute attribute)
        {
            if (attribute != null && attribute.GetAttributeValues() != null)
            {
                _Children = new ArrayList(attribute.GetAttributeValues().Count);
                for (int i = 0; i < attribute.GetAttributeValues().Count; i++)
                    _Children.Add(null);
            }
            _Node = attribute;
        }

        /// <summary>
        /// 增加节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public void AddNode(Tree node, string value)
        {
            int i = _Node.GetValueIndex(value);
            _Children[i] = node;
        }

        public Attribute GetAttribute()
        {
            return _Node;
        }

        public Tree GetChild(string valueName)
        {
            int i = _Node.GetValueIndex(valueName);
            return (Tree)_Children[i];
        }
    }
}
