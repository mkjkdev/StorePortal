using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalProj
{
    class BinaryTree<T> where T : IComparable<T>
    {
        #region Public Properties
        public Node<T> Root { get; set; }
        #endregion

        #region Methods
        public virtual void preOrder(Node<T> node)
        {
            if (node == null)
            {
                return;
            }
            Console.WriteLine(node.gsData + "");
            preOrder(node.gsLeft);
            preOrder(node.gsRight);
        }

        /// <summary>
        /// converts unbalanced BST to a balanced BST
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public Node<T> buildTree(Node<T> root)
        {
            //store nodes of given BST in sorted order
            List<Node<T>> nodes = new List<Node<T>>();
            storeBSTNodes(root, nodes);
            //mergeSort(nodes, 0, nodes.Count);
            //construct balanced BST from nodes list
            int n = nodes.Count;
            return buildTreeUtil(nodes, 0, n - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Node<T> Insert(Node<T> root, T key)
        {
            if (root == null)
            {
                return null;
            }

            //get list of nodes from current tree
            //List<Node<T>> nodes = new List<Node<T>>();
            //storeBSTNodes(root, nodes);

            //create new node with new key data
            Node<T> newNode = new Node<T>(key);
            List<Node<T>> newNodes = new List<Node<T>>();
            newNodes.Add(newNode);

            return buildTreeUtil(newNodes, 0 ,newNodes.Count-1);
        }

        /// <summary>
        /// Binary Search for key
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="key"></param>
        public Node<T> Search(Node<T> root, String key)
        {
            //base case
            if (root == null)
            {
                return null;
            }

            List<Node<T>> nodes = new List<Node<T>>();
            storeBSTNodes(root, nodes);

            //store result
            Node<T> result;
            int min = 0;
            int max = nodes.Count-1;
            while (min <= max)
            {
                int mid = (max + min) / 2;
                String test = nodes[mid].gsData.ToString();
                if (string.Compare(test, key, StringComparison.Ordinal)==0)
                {
                    //Console.WriteLine("Search for : " +key+ " found");
                    result = new Node<T>(nodes[mid].gsData);
                    return result;
                }
                else if (string.Compare(key, test, StringComparison.Ordinal)<0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            return null;
        }
        /// <summary>
        /// Compare node data and bubble sort, convert to merge sort
        /// </summary>
        /// <param name="nodes"></param>
        public static void Sort(List<Node<T>> nodes)
        //private static List<Node<T>> Sort(List<Node<T>> left, List<Node<T>> right)
        {
            //bubble
            Node<T> temp;
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int p = 0; p < nodes.Count; p++)
                {
                    if (nodes[p].compareTo(nodes[i]) > 0)
                    {
                        temp = nodes[i];
                        nodes[i] = nodes[p];
                        nodes[p] = temp;
                    }
                }
            }

            //first implement
            //List<Node<T>> sortedNodes = new List<Node<T>>();

            //while (left.Count > 0 || right.Count > 0)
            //{
            //    if (left.Count > 0 && right.Count > 0)
            //    {
            //        if (left.First().compareTo(right.First()) <= 0)
            //        {
            //            sortedNodes.Add(left.First());
            //            left.Remove(left.First());
            //        }
            //        else
            //        {
            //            sortedNodes.Add(right.First());
            //            right.Remove(right.First());
            //        }
            //    }
            //    else if (left.Count > 0)
            //    {
            //        sortedNodes.Add(left.First());
            //        left.Remove(left.First());
            //    }
            //    else if (right.Count > 0)
            //    {
            //        sortedNodes.Add(right.First());
            //        right.Remove(right.First());
            //    }
            //}
            //return sortedNodes;

        }

        //public static List<Node<T>> mergeSort(List<Node<T>> nodes)
        //{
        //    int mid = nodes.Count / 2;
        //    List<Node<T>> left = new List<Node<T>>();
        //    List<Node<T>> right = new List<Node<T>>();

        //    for (int i = 0; i < mid; i++)
        //    {
        //        left.Add(nodes[i]);
        //    }
        //    for (int j = mid; j < nodes.Count; j++)
        //    {
        //        right.Add(nodes[j]);
        //    }

        //    return Sort(left, right);
        //}

        /// <summary>
        /// Store nodes from BST into list
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodes"></param>
        public void storeBSTNodes(Node<T> root, List<Node<T>> nodes)
        {
            //Base case if null
            if (root == null)
            {
                return;
            }
            //store nodes in order(which is sorted order for BST)
            Sort(nodes);
            storeBSTNodes(root.gsLeft, nodes);
            nodes.Add(root);
            storeBSTNodes(root.gsRight, nodes);
        }

        /// <summary>
        /// build tree from sorted list
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Node<T> buildTreeUtil(List<Node<T>> nodes, int start, int end)
        {
            //base case recursion
            if (start > end)
            {
                return null;
            }
            //get middle element and make it root
            int mid = (start + end) / 2;
            Node<T> node = nodes[mid];

            //using index in order traversal, construct left and right subtrees
            node.gsLeft = buildTreeUtil(nodes, start, mid - 1);
            node.gsRight = buildTreeUtil(nodes, mid + 1, end);
            return node;
        }

        #endregion

    }
}
