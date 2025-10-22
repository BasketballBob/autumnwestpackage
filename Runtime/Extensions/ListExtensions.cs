using System;
using System.Collections;
using System.Collections.Generic;

namespace AWP
{
    public static class ListExtensions
    {
        public static T FirstItem<T>(this List<T> list)
        {
            return list[0];
        }
        public static void SetFirstItem<T>(this List<T> list, T newValue)
        {
            list[0] = newValue;
        }
        public static void RemoveFirstItem<T>(this List<T> list)
        {
            list.RemoveAt(0);
        }
        public static T PopFirstItem<T>(this List<T> list)
        {
            T firstItem = list.FirstItem();
            list.RemoveFirstItem();
            return firstItem;
        }

        public static T LastItem<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
        public static void SetLastItem<T>(this List<T> list, T newValue)
        {
            list[list.Count - 1] = newValue;
        }
        public static void RemoveLastItem<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
        public static T PopLastItem<T>(this List<T> list)
        {
            T lastItem = list.LastItem();
            list.RemoveLastItem();
            return lastItem;
        }

        public static T RemoveAndReturn<T>(this List<T> list, int index)
        {
            T returnVal = list[index];
            list.RemoveAt(index);
            return returnVal;
        }

        public static void MoveToFront<T>(this List<T> list, int itemIndex) => list.MoveToIndex(itemIndex, 0);
        public static void MoveToBack<T>(this List<T> list, int itemIndex) => list.MoveToIndex(itemIndex, list.Count);
        public static void MoveToIndex<T>(this List<T> list, int itemIndex, int moveIndex)
        {
            T item = list[itemIndex];
            list.RemoveAt(itemIndex);
            list.Insert(moveIndex, item);
        }

        public static void AddIfAbsent<T>(this List<T> list, T item)
        {
            if (list.Contains(item)) return;
            list.Add(item);
        }

        public static void RemoveIfPresent<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) return;
            list.Remove(item);
        }

        /// <summary>
        /// Clears all null values from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ClearNullValues<T>(this List<T> list) where T : class
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        #region Stack functionality
        public static void StackPush<T>(this List<T> list, T newValue)
        {
            list.Add(newValue);
        }

        public static T StackPop<T>(this List<T> list)
        {
            T returnItem = list.LastItem();
            list.RemoveLastItem();
            return returnItem;
        }

        public static T StackPeek<T>(this List<T> list) => list.LastItem();
        #endregion

        #region Queue functionality
        public static void Enqueue<T>(this List<T> list, T newValue)
        {
            list.Insert(0, newValue);
        }

        public static T Dequeue<T>(this List<T> list)
        {
            return list.StackPop();
        }
        #endregion

        #region Item parsing
        public static T PullPriorityItem<T>(this List<T> list, Func<T, float> priorityFunc)
        {
            T currentItem = default;
            float currentPriority = float.MinValue;

            for (int i = 0; i < list.Count; i++)
            {
                if (priorityFunc(list[i]) > currentPriority)
                {
                    currentItem = list[i];
                    currentPriority = priorityFunc(list[i]);
                }
            }

            return currentItem;
        }
        #endregion
    }
}
