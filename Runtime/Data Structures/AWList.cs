using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWList<T>
{
    protected List<T> _items = new List<T>();

    public AWList()
    {
        _items = new List<T>();
    }

    public virtual T this[int index]
    {
        get 
        {
            return _items[index];
        }
        set
        {
            _items[index] = value;
        }
    }
}
