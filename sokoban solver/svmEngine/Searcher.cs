using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inferenceEngine.svmEngine
{

    /// <summary>
    /// breadth-first sercher on the problem state graph
    /// </summary>
    public class Searcher
    {

        Queue<Node<IState>> q = new Queue<Node<IState>>();
        List<IState> solVector = new List<IState>();
        StatesTree<IState> tree = new StatesTree<IState>();
        Node<IState> FinalStateNode;


        /// <summary>
        /// gets the possible states from the given state removing states that have already 
        /// been produced
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<IState> getNext(IState state)
        {
            List<IState> tmp = new List<IState>();

            foreach (IState item in state.Next())
            {
                if (!tree.Contains(item))
                {
                    tmp.Add(item);
                }
            }
            
            if (tmp.Count == 0)
                return null;
            else
                return tmp;
        }

        private List<Node<IState>> statesToNodes(List<IState> states)
        {
            List<Node<IState>> nodes = new List<Node<IState>>();
            //
            foreach (IState s in states)
            {
                nodes.Add(new Node<IState>(s));
            }
           
            return nodes;
        }

        
       
        /// <summary>
        /// searches for the final state ,returning true if found false otherwise, setting the final state
        /// </summary>
        private bool search(Node<IState> node)
        {
            if (node.Value.isTargetState())
            {
                FinalStateNode = node;
                return true;
            }
            else
            {
                if (!(node.Value.isBlockedState()))//for blocked state optimization
                {

                    List<IState> nxt = getNext(node.Value);
                    if (nxt != null)
                    {
                        List<Node<IState>> nodes = statesToNodes(nxt);
                        node.addChildren(nodes);

                        foreach (Node<IState> item in nodes)
                        {
                            q.Enqueue(item);
                        }
                    }
                }
                //
                if (q.Count > 0)
                {
                    
                    return search(q.Dequeue());
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// returns the entire path to solution state form the initial state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<IState> getSolution(IState initialState)
        {
            Node<IState> root = new Node<IState>(initialState);
            tree.setRoot(root);
            if (search(root))
            {
                for (Node<IState> i = FinalStateNode; i.Parent != null; i = i.Parent)
                {
                    solVector.Add(i.Value);
                }
                this.tree = null;//helping garpage collector
                solVector.Reverse();
                return solVector;
            }
            else
            {
                return null;
            }
        }

        

    }
}