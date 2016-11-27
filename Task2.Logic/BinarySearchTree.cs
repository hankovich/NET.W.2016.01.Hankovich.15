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
                using (IEnumerator<T> left = Left.PreOrder())
                {
                    while (left.MoveNext())
                    {
                        yield return left.Current;
                    }
                }
            }
            
            if (Right != null)
            {
                using (IEnumerator<T> right = Right.PreOrder())
                {
                    while (right.MoveNext())
                    {
                        yield return right.Current;
                    }
                }
            }
        }

        public IEnumerator<T> InOrder()
        {
            if (Left != null)
            {
                using (IEnumerator<T> left = Left.InOrder())
                {
                    while (left.MoveNext())
                    {
                        yield return left.Current;
                    }
                }
            }

            yield return Value;

            if (Right != null)
            {
                using (IEnumerator<T> right = Right.InOrder())
                {
                    while (right.MoveNext())
                    {
                        yield return right.Current;
                    }
                }
            }
        }

        public IEnumerator<T> PostOrder()
        {
            if (Left != null)
            {
                using (IEnumerator<T> left = Left.PostOrder())
                {
                    while (left.MoveNext())
                    {
                        yield return left.Current;
                    }
                }
            }

            if (Right != null)
            {
                using (IEnumerator<T> right = Right.PostOrder())
                {
                    while (right.MoveNext())
                    {
                        yield return right.Current;
                    }
                }
            }

            yield return Value;
        }
    }

    public class BinarySearchTree<T>: ICollection<T>
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
                        newNode.Right = new Node<T>(item) { Father = newNode };
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
            using (IEnumerator<T> it = PostOrder())
                while (it.MoveNext())
                    Remove(it.Current);
        }

        public bool Contains(T item)
        {
            return Contains(root, item);
        }

        public bool Contains(Node<T> startNode, T item)
        {
            while (true)
            {
                if (startNode == null)
                    return false;
                if (comparison(startNode.Value, item) == 0)
                    return true;
                if (comparison(startNode.Value, item) >= 1)
                {
                    startNode = startNode.Right;
                    continue;
                }
                if (comparison(startNode.Value, item) <= -1)
                {
                    startNode = startNode.Left;
                    continue;
                }
                break;
            }
            return true;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException($"{nameof(array)} must be not null.");
            if(arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayIndex)} is out of range");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException($"{nameof(array)} is too small");
            Array.Copy(this.ToArray(), 0, array, arrayIndex, Count);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
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
            return root.InOrder();
        }

        public IEnumerator<T> PostOrder()
        {
            return root.PostOrder();
        }

        public IEnumerator<T> PreOrder()
        {
            return root.PreOrder();
        }
        #endregion
    }
}
