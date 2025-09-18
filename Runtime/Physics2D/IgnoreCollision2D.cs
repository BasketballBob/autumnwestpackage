using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.Utilities;
using UnityEngine;

public class IgnoreCollision2D : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Collider2D _callingCollider;

    [SerializeField]
    private List<Collider2D> _ignoredColliders = new List<Collider2D>();

    private void OnEnable()
    {
        IgnoreCollisions(_callingCollider);
        IgnoreCollisions(_rb);
    }

    public void IgnoreCollisions(Collider2D col)
    {
        IgnoreCollisions(col, _ignoredColliders);
    }

    public void IgnoreCollisions(Rigidbody2D rb)
    {
        if (rb == null) return;
        _ignoredColliders.ForEach(x => AWPhysics2D.IgnoreRigidbodyColliderCollision(rb, x));
    }

    public static void IgnoreCollisions(Collider2D col, List<Collider2D> ignoredColliders)
    {
        if (ignoredColliders.IsNullOrEmpty()) return;

        foreach (Collider2D element in ignoredColliders)
        {
            if (element == null) continue;
            Physics2D.IgnoreCollision(col, element, true);
        }
    }
}
