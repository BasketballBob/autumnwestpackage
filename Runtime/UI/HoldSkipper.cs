using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace AWP
{
    public class HoldSkipper : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private InputActionReference _skipInput;
        [SerializeField]
        private float _holdDuration = 1.25f;
        [SerializeField]
        private float _releaseDuration = .35f;
        public UnityEvent OnSkip;

        private bool _skipped;
        private bool _isPressed;
        private bool _isPressedOld;
        private Alarm _skipAlarm = new Alarm(1);

        private void Start()
        {
            SyncAnimationSpeed();
        }

        private void LateUpdate()
        {
            if (_skipped) return;

            _isPressed = _skipInput.action.IsPressed();

            if (_isPressed) _skipAlarm.TickForDuration(Time.deltaTime, _holdDuration);
            else _skipAlarm.TickForDuration(-Time.deltaTime, _releaseDuration);

            if (_isPressed != _isPressedOld) SyncAnimationSpeed();
            if (_skipAlarm.IsFinished()) Skip();

            _isPressedOld = _isPressed;
        }

        private void Skip()
        {
            OnSkip.Invoke();
            _skipped = true;
        }

        private void SyncAnimationSpeed()
        {
            _anim.Play("HoldSkip", 0, _skipAlarm.Delta);
            _anim.SetFloat("Speed", _anim.GetSpeedForDuration(_isPressed ? _holdDuration : -_releaseDuration));
        }
    }
}
