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
    }
}
