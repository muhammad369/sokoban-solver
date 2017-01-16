using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sokoban_solver
{

    /// <summary>
    /// breadth-first sercher on the problem state graph
    /// </summary>
    public class Searcher
    {

        Queue<Node<State>> q = new Queue<Node<State>>();
        List<State> solVector = new List<State>();
        Tree<State> tree = new Tree<State>();
        Node<State> FinalStateNode;


        /// <summary>
        /// gets the possible states from the given state removing states that have already 
        /// been produced
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<State> getNext(State state)
        {
            List<State> tmp = new List<State>();

            foreach (State item in state.Next())
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

        private List<Node<State>> statesToNodes(List<State> states)
        {
            List<Node<State>> nodes = new List<Node<State>>();
            //
            foreach (State s in states)
            {
                nodes.Add(new Node<State>(s));
            }
           
            return nodes;
        }

        
        /// <summary>
        /// searches for the final state ,returning true if found false otherwise, setting the final state
        /// </summary>
        /// <param name="initialState"></param>
        /// <returns></returns>
        bool search(State initialState)
        {
            if (initialState.TargetsNotYetReached == 0)
            {
                FinalStateNode = new Node<State>(initialState);
                return true;

            }
            else
            {
                Node<State> root = new Node<State>(initialState);
                tree.setRoot(root);
                List<State> nxt = getNext(initialState);
                if (nxt != null)
                {
                    List<Node<State>> nodes = statesToNodes(nxt);
                    root.addChildren(nodes);
                    
                    foreach (Node<State> item in nodes)
                    {
                        q.Enqueue(item);
                    }

                    return Search(q.Dequeue());
                }
                else //nxt == null
                {
                    return false;
                }
            }
        }

        private bool Search(Node<State> node)
        {
            if (node.Value.TargetsNotYetReached == 0)
            {
                FinalStateNode = node;
                return true;
            }
            else
            {
                if (!isBlockedState(node.Value))//for blocked state optimization
                {

                    List<State> nxt = getNext(node.Value);
                    if (nxt != null)
                    {
                        List<Node<State>> nodes = statesToNodes(nxt);
                        node.addChildren(nodes);
                        
                        foreach (Node<State> item in nodes)
                        {
                            q.Enqueue(item);
                        }
                    }
                }
                //
                if (q.Count > 0)
                {
                    
                    return Search(q.Dequeue());
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
        public List<State> getSolution(State initialState)
        {
            if (search(initialState))
            {
                for (Node<State> i = FinalStateNode; i.Parent != null; i = i.Parent)
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

        #region blocked state optimization

        private bool isBlockedState(State state)
        {
            foreach (Position item in state.blocks)
            {
                if (state.getCell(item) == State.block)//not block in target
                {

                    //
                    bool wallUP = false;
                    bool wallRight = false;
                    bool wallDown = false;
                    if (state.getCell(item.X, item.Y - 1) == State.wall)//up
                    {
                        wallUP = true;
                    }
                    if (state.getCell(item.X + 1, item.Y) == State.wall)//right
                    {
                        if (wallUP)
                        {
                            return true;
                        }
                        else
                        {
                            wallRight = true;
                        }

                    }
                    if (state.getCell(item.X, item.Y + 1) == State.wall)//down
                    {
                        if (wallRight)
                        {
                            return true;
                        }
                        else
                        {
                            wallDown = true;
                        }
                    }
                    if (state.getCell(item.X - 1, item.Y) == State.wall)//left
                    {
                        if (wallDown || wallUP)
                        {
                            return true;
                        }

                    }

                }
            }
            //reaching here means no block is in bad position
            return false;
        }
        #endregion

    }
}