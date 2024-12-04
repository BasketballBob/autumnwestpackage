using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision2D : MonoBehaviour
{
    [SerializeField]
    private Collider2D _callingCollider;

    [SerializeField]
    private List<Collider2D> _ignoredColliders = new List<Collider2D>();

    private void OnEnable()
    {
        foreach (Collider2D element in _ignoredColliders)
        {
            Physics2D.IgnoreCollision(_callingCollider, element, true);
        }
    }
}
