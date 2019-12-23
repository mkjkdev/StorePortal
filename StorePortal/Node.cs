using System;
namespace FinalProj
{
    class Node<T> where T : IComparable<T>
    {
        private T data;
        private Node<T> left, right;

        #region Constructors
        public Node(T data) : this(data, null, null) { }
        public Node(T data, Node<T> left, Node<T> right)
        {
            this.data = data;
            this.left = left;
            this.right = right;
        }
        #endregion

        #region Public Properties
        public T gsData
        {
            get { return data; }
            set { data = value; }
        }

        public Node<T> gsLeft
        {
            get { return left; }
            set { left = value; }
        }

        public Node<T> gsRight
        {
            get { return right; }
            set { right = value; }
        }
        #endregion

        #region Methods
        public int Search(T key)
        {
            return this.data.CompareTo(key);
        }

        public int compareTo(Node<T> node)
        {
            return this.gsData.CompareTo(node.gsData);
        }
        #endregion
    }
}
