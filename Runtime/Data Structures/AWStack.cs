using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWStack<T> : AWList<T>
{
    public T TopItem => this[Count - 1];
    public T BottomItem => this[0];

    public AWStack() : base()
    {

    }

    public void Push(T element)
    {
        Add(element);
    }

    public T Peek()
    {
        return TopItem;
    }

    public T Pop()
    {
        T item = TopItem;
        RemoveAt(Count - 1);
        return item;
    }
}
