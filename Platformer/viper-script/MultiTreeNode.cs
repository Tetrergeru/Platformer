using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace viper_script
{
    public class MultiTreeNode<T>
    {
        public int Count { get; private set; }

        public MultiTreeNode<T> Parent { get; private set; }

        private MultiTreeNode<T> _child;

        private MultiTreeNode<T> _sibling;

        public T Data { get; set; }

        public MultiTreeNode(T data, MultiTreeNode<T> parent = null)
        {
            Data = data;
            Parent = parent;
            _sibling = parent;
            _child = this;
        }

        public void AddChild(MultiTreeNode<T> child)
        {
            child.Parent = this;
            child._sibling = _child;
            _child = child;
            Count++;
        }

        public MultiTreeNode<T> PopChild()
        {
            var c = _child;
            if (c == this)
                return null;

            _child = _child._sibling;
            Count--;
            return c;
        }

        public IEnumerable<MultiTreeNode<T>> GetChildren()
        {
            var c = _child;
            while (c != this)
            {
                yield return c;
                c = c._sibling;
            }
        }

        public IEnumerable<T> Traverse()
        {
            yield return Data;
            foreach (var child in GetChildren())
            foreach (var data in child.Traverse())
                yield return data;
        }

        public MultiTreeNode<T> this[int key]
        {
            get
            {
                foreach(var x in GetChildren())
                    if (key == 0)
                        return x;
                    else
                        key--;
                throw new ArgumentException("Argument key should be lesser the child number");
            }
        }

        public void Print(int tab = 0)
        {
            foreach (var child in GetChildren().Reverse())
                child.Print(tab + 1);
            
            for (var i = 0; i < tab; i++)
                Console.Write("\t");
            Console.WriteLine(Data);
        }
    }
}