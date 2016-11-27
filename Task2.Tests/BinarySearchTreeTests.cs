using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task2;
using Task2.Logic;

namespace Task2.Tests
{
    [TestFixture]
    public class BinarySearchTreeTests
    {
        [TestCase(new [] {4, 2, 1, 3, 6, 5, 7}, 4, 2, 6, 1, 3, 5, 7)]
        [Test]
        public void ToArray_IntParams_Array(int[] array, params int[] source)
        {
            BinarySearchTree<int> tree = new BinarySearchTree<int>(source);
            int[] arr = new int[tree.Count];
            tree.CopyTo(arr, 0);
            CollectionAssert.AreEqual(array, arr);
        }

        [TestCase(new [] { "koala", "bear", "killer", "door", "zoo", "petrol", "night" }, "koala", "zoo", "bear", "petrol", "night", "killer", "door")]
        [Test]
        public void ToArray_StringParams_Array(string[] array, params string[] source)
        {
            BinarySearchTree<string> tree = new BinarySearchTree<string>(source);
            string[] arr = new string[tree.Count];
            tree.CopyTo(arr, 0);
            CollectionAssert.AreEqual(array, arr);
        }
    }

    public class Book: IComparable<Book>
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public int CompareTo(object obj)
        {
            var book = obj as Book;
            if (book == null)
                throw new ArgumentNullException();
            return CompareTo(book);
        }

        public int CompareTo(Book book)
        {
            int ret = string.Compare(Title, book.Title, StringComparison.Ordinal);
            if (ret == 0)
                ret = Year.CompareTo(book.Year);
            return ret;
        }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

}
