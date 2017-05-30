//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="WrappingQueue.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
namespace Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///     A Reverse indexed queue (add to front) that overwrites its own tail.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class WrappingQueue<T> : IList<T>
    {
        [SerializeField]
        private T[] array;

        [SerializeField]
        private int length = 0;

        [SerializeField]
        private int start = -1;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrappingQueue{T}" /> class with <paramref name="maxHistory" />
        ///     size.
        ///     A Reverse indexed queue (add to front) that overwrites its own tail
        /// </summary>
        /// <param name="maxHistory">maximum tail size</param>
        public WrappingQueue(int maxHistory)
        {
            array = new T[maxHistory];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrappingQueue{T}" /> class  with a default size (10).
        ///     A Reverse indexed queue (add to front) that overwrites its own tail
        /// </summary>
        public WrappingQueue()
        {
            array = new T[10];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrappingQueue{T}" /> class from an enumerable collection.
        /// </summary>
        /// <param name="collection">
        ///     The collection.
        /// </param>
        /// <param name="isReversedOrder">
        ///     Is the collection already in Reversed Order.
        /// </param>
        public WrappingQueue(IEnumerable<T> collection, bool isReversedOrder = false)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (collection is WrappingQueue<T>)
            {
                isReversedOrder = true;
            }

            array = isReversedOrder ? collection.ToArray() : collection.Reverse().ToArray();

            length = array.Length;
            Start = 0;
        }

        /// <summary>
        ///     The number of currently used elements
        /// </summary>
        public int Count { get { return length; } private set { length = value < array.Length ? value : array.Length; } }

        public bool IsReadOnly { get { return array.IsReadOnly; } }

        public bool IsSynchronized { get { return false; } }

        public object SyncRoot { get { return this; } }

        private bool IsFixedSize { get { return array.IsFixedSize; } }

        private int Start { get { return start; } set { start = value > array.Length ? value - array.Length : (value < 0 ? value + array.Length : value); } }

        /// <summary>
        ///     Gets and sets the item at the given <paramref name="index" />
        /// </summary>
        /// <param name="index">0-indexed position</param>
        /// <returns>The element at <paramref name="index" /></returns>
        public T this[int index] { get { return array[FromInternal(index)]; } set { array[FromInternal(index)] = value; } }

        /// <summary>
        ///     Gets and sets the item at the given <paramref name="index" />
        /// </summary>
        /// <param name="index">0-indexed position</param>
        /// <returns>The element at <paramref name="index" /></returns>
        T IList<T>.this[int index] { get { return array[FromInternal(index)]; } set { array[FromInternal(index)] = value; } }

        /// <summary>
        ///     Adds a new item to the stack. Overwrites if Count equals stack size
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Start--;
            array[Start] = item;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        /// <summary>
        ///     Checks if the <paramref name="item" /> exists in the collection
        /// </summary>
        /// <param name="item">The item to locate</param>
        /// <returns>true if found</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] arr, int arrayIndex)
        {
            var j = arrayIndex;
            for (var i = 0; i < Count; i++)
            {
                arr.SetValue(i, FromInternal(j));
                j++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)array.GetEnumerator();
        }

        /// <summary>
        ///     Finds the index of the first occurrence of <paramref name="item" />
        /// </summary>
        /// <param name="item">Item to locate</param>
        /// <returns>Current index of item</returns>
        public int IndexOf(T item)
        {
            var i = Array.IndexOf((Array)array, item);
            return ToInternal(i);
        }

        /// <summary>
        ///     Inserts <paramref name="item" /> at <paramref name="index" /> pushing everything else down
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            if ((index >= Count) || (index > 0))
            {
                // index is out of bounds
                return;
            }

            if ((Count + 1) <= array.Length)
            {
                // List still has space, add another element
                Count++;
            }

            // push all other elements down
            for (var i = Count - 1; i >= 0; i--)
            {
                array[FromInternal(i)] = array[FromInternal(i + 1)];
            }

            // insert new element
            array[FromInternal(index)] = item;
        }

        public bool Remove(T item)
        {
            var idx = IndexOf(item);
            if (idx > 0)
            {
                RemoveAt(FromInternal(idx));
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                // index is inside bounds
                // pull all the next elements up
                for (var i = index; i < Count; i++)
                {
                    array[FromInternal(i)] = array[FromInternal(i + 1)];
                }
            }

            Count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }

        /// <summary>
        ///     Converts from 0-indexed to internal floating index and back again
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns the inverse of the index type passed</returns>
        private int FromInternal(int index)
        {
            return (Start + index) % array.Length;
        }

        /// <summary>
        ///     Converts from  internal floating to index0-indexed
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns the inverse of the index type passed</returns>
        private int ToInternal(int index)
        {
            return (array.Length - (Start - index)) % array.Length;
        }
    }
}