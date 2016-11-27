using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Logic
{
    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Father { get; set; }

        public Node(T item)
        {
            Value = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var v in Left)
                {
                    yield return v;
                }
            }

            yield return Value;

            if (Right != null)
            {
                foreach (var v in Right)
                {
                    yield return v;
                }
            }
        }

        public IEnumerator<T> PreOrder()
        {
            yield return Value;

            if (Left != null)
            {
                IEnumerator<T> left = Left.PreOrder();

                while (left.MoveNext())
                    yield return left.Current;
            }

            if (Right != null)
            {
                IEnumerator<T> right = Right.PreOrder();

                while (right.MoveNext())
                    yield return right.Current;
            }
        }

        public IEnumerator<T> InOrder()
        {
            if (Left != null)
            {
                IEnumerator<T> left = Left.InOrder();

                while (left.MoveNext())
                    yield return left.Current;
            }

            yield return Value;

            if (Right != null)
            {
                IEnumerator<T> right = Right.InOrder();

                while (right.MoveNext())
                    yield return right.Current;
            }
        }

        public IEnumerator<T> PostOrder()
        {
            if (Left != null)
            {
                IEnumerator<T> left = Left.PostOrder();

                while (left.MoveNext())
                    yield return left.Current;
            }

            if (Right != null)
            {
                IEnumerator<T> right = Right.PostOrder();

                while (right.MoveNext())
                    yield return right.Current;
            }

            yield return Value;
        }
    }

    public class BinarySearchTree<T> : ICollection<T>
    {
        private Node<T> root;
        private readonly Comparison<T> comparison;

        #region Ctors

        public BinarySearchTree(Comparison<T> cmp)
        {
            if (cmp == null)
                throw new ArgumentNullException($"{nameof(cmp)} must be not null.");

            comparison = cmp;
        }

        public BinarySearchTree(IComparer<T> cmp)
        {
            if (cmp == null)
                throw new ArgumentNullException($"{nameof(cmp)} must be not null.");

            comparison = cmp.Compare;
        }

        public BinarySearchTree(IEnumerable<T> source) : this()
        {
            if (source == null)
                throw new ArgumentNullException($"{nameof(source)} must be not null.");

            foreach (var item in source)
            {
                Add(item);
            }
        }

        public BinarySearchTree(Comparison<T> cmp, IEnumerable<T> source)
        {
            if (cmp == null)
                throw new ArgumentNullException($"{nameof(cmp)} must be not null.");
            if (source == null)
                throw new ArgumentNullException($"{nameof(source)} must be not null.");

            comparison = cmp;
            foreach (var item in source)
            {
                Add(item);
            }
        }

        public BinarySearchTree(IComparer<T> cmp, IEnumerable<T> source)
        {
            if (cmp == null)
                throw new ArgumentNullException($"{nameof(cmp)} must be not null.");
            if (source == null)
                throw new ArgumentNullException($"{nameof(source)} must be not null.");

            comparison = cmp.Compare;
            foreach (var item in source)
            {
                Add(item);
            }
        }

        public BinarySearchTree()
        {
            comparison = Comparer<T>.Default.Compare;
        }

        #endregion

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException($"{nameof(item)} must be not null.");
            if (root == null)
            {
                root = new Node<T>(item)
                {
                    Father = null,
                    Left = null,
                    Right = null
                };
                Count++;
                return;
            }
            Node<T> newNode = root;
            while (true)
            {
                if (comparison(item, newNode.Value) <= -1)
                {
                    if (newNode.Left == null)
                    {
                        newNode.Left = new Node<T>(item) {Father = newNode};
                        Count++;
                        break;
                    }
                    newNode = newNode.Left;
                    continue;
                }
                if (comparison(item, newNode.Value) >= 1)
                {
                    if (newNode.Right == null)
                    {
                        newNode.Right = new Node<T>(item) {Father = newNode};
                        Count++;
                        break;
                    }
                    newNode = newNode.Right;
                    continue;
                }
                if (comparison(item, newNode.Value) == 0)
                {
                    //Need some exception?
                    break;
                }
            }
        }

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return FindNode(item) != null;
        }

        private Node<T> FindNode(T item)
        {
            Node<T> startNode = root;
            while (true)
            {
                if (startNode == null)
                    return null;
                if (comparison(startNode.Value, item) == 0)
                    return startNode;
                if (comparison(item, startNode.Value) >= 1)
                {
                    startNode = startNode.Right;
                    continue;
                }
                if (comparison(item, startNode.Value) <= -1)
                {
                    startNode = startNode.Left;
                    continue;
                }
                break;
            }
            return null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException($"{nameof(array)} must be not null.");
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayIndex)} is out of range");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException($"{nameof(array)} is too small");
            
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(T item)
        {
            Node<T> node = FindNode(item);
            if (node == null)
                return false;

            if (node.Left == null && node.Right == null) //No children
            {
                if (node.Father != null)
                    if (ReferenceEquals(node.Father.Left, node))
                        node.Father.Left = null;
                    else
                        node.Father.Right = null;
                else
                    root = null;

                Count--;
                return true;
            }

            if (node.Left == null ^ node.Right == null) //One child
            {
                if (node.Left == null)
                {
                    if (node.Right != null)
                        node.Right.Father = node.Father;
                    if (node.Father != null)
                    {
                        if (ReferenceEquals(node.Father.Left, node))
                            node.Father.Left = node.Right;
                        else
                            node.Father.Right = node.Right;
                    }
                    else
                        root = node.Right;
                }
                else
                {
                    if (node.Left != null)
                        node.Left.Father = node.Father;
                    if (node.Father != null)
                    {

                        if (ReferenceEquals(node.Father.Left, node))
                            node.Father.Left = node.Left;
                        else
                            node.Father.Right = node.Left;
                    }
                    else
                        root = node.Left;
                }

                Count--;
                return true;
            }

            if (node.Left != null && node.Right != null)
            {
                Node<T> nodeToReplace = node.Left;
                while (nodeToReplace.Right != null)
                    nodeToReplace = nodeToReplace.Right;
                Remove(nodeToReplace.Value);
                if (node.Left != null)
                    node.Left.Father = nodeToReplace;
                if (node.Right != null)
                    node.Right.Father = nodeToReplace;
                nodeToReplace.Father = node.Father;
                nodeToReplace.Right = node.Right;
                nodeToReplace.Left = node.Left;
                if (ReferenceEquals(node, root))
                    root = nodeToReplace;
                else
                {
                    if (ReferenceEquals(node.Father.Left, node))
                        node.Father.Left = nodeToReplace;
                    else
                        node.Father.Right = nodeToReplace;
                }
                return true;
            }
            return true;
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        #region Enumenators

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return PreOrder();
        }

        public IEnumerator<T> InOrder()
        {
            return root?.InOrder();

        }

        public IEnumerator<T> PostOrder()
        {
            return root?.PostOrder();
        }

        public IEnumerator<T> PreOrder()
        {
            return root?.PreOrder();
        }

        #endregion
    }
}
