using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class IgnoreCollision2D : MonoBehaviour
{
    [SerializeField]
    private Collider2D _callingCollider;

    [SerializeField]
    private List<Collider2D> _ignoredColliders = new List<Collider2D>();

    private void OnEnable()
    {
        IgnoreCollisions(_callingCollider);
    }

    public void IgnoreCollisions(Collider2D col)
    {
        IgnoreCollisions(col, _ignoredColliders);
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
