using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWStack<T> : AWList<T>
{
    public T TopItem => _items[_items.Count - 1];
    public T BottomItem => _items[0];

    public AWStack() : base()
    {

    }

    public void Push(T element)
    {
        _items.Add(element);
    }

    public T Peek()
    {
        return TopItem;
    }

    public T Pop()
    {
        T item = TopItem;
        _items.RemoveAt(_items.Count - 1);
        return item;
    }
}
