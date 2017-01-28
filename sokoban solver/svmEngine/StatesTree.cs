 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace inferenceEngine.svmEngine
{
    
    /// <summary>
    /// the tree of the states on the valid moves graph(space)
    /// </summary>
    /// <typeparam name="T">the type that each node of the tree contains as its value</typeparam>
    public class StatesTree<T>
    {

        internal Node<T> root;
        internal Dictionary<T, T> nodes;


        public StatesTree()
        {
            nodes = new Dictionary<T,T>();
            Node<T>.tree = this;
        }

        public bool Contains(T key)
        {
            return nodes.ContainsKey(key);
        }

        public void setRoot(Node<T> root)
        {
            this.root = root;
            this.nodes.Add(root.Value, root.Value);
        }

    }


    public class Node<T>
    {

        internal static StatesTree<T> tree;
        internal T Value;

        internal Node<T> Parent;
        List<Node<T>> Clildren;

        public Node(T value)
        {
            this.Value = value;
            Clildren = new List<Node<T>>();
        }
       
        
        /// <summary>
        /// adds a child node to this one ,and adds it to the
        /// tree dictionary
        /// so that it can be recognized by Contains()
        /// </summary>
        /// <param name="ch"></param>
        void addChild(Node<T> ch)
        {
            //Node<T> ch=new Node(v);
            ch.Parent = this;
            this.Clildren.Add(ch);
            Node<T>.tree.nodes.Add(ch.Value, ch.Value);
        }

       
        public void addChildren(List<Node<T>> chldrn)
        {
            foreach (Node<T> item in chldrn)
            {
                this.addChild(item);
            }
            
        }

        public List<Node<T>> getChildren()
        {
            return this.Clildren;
        }

        
    }
}