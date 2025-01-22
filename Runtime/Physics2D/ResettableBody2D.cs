using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ResettableBody2D : ResettableBody, IResettable
{
    private Rigidbody2D _rb;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private bool _initialized;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _initialized = true;
    }

    public void Reset()
    {
        if (!_initialized) return;

        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        if (_zeroVelocity) _rb.velocity = Vector2.zero;
        if (_zeroAngularVelocity) _rb.angularVelocity = 0;
    }
}
