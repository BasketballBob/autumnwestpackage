using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class EnterExitUI : MonoBehaviour
    {
        private const string EnterAnimation = "Enter";
        private const string ExitAnimation = "Exit";

        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private Selectable _selectable;
        [SerializeField]
        private int _layer;

        private bool _visible = true;
        private SingleCoroutine _visibleRoutine;

        private void Reset()
        {
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            _visibleRoutine = new SingleCoroutine(this);
        }

        public void SetVisible(bool visible, bool immediate = false)
        {
            if (_visible == visible) return;
            if (!gameObject.activeInHierarchy) return;

            _visible = visible;

            if (immediate)
            {
                _anim.Play(_visible ? EnterAnimation : ExitAnimation, _layer, 0);
                _anim.Update(0);
            }
            else _anim.Play(_visible ? EnterAnimation : ExitAnimation, _layer);
            _visibleRoutine.StartRoutine(_visible ? Enter() : Exit());

            IEnumerator Enter()
            {
                TrySetInteractable(false);
                yield return _anim.WaitForAnimationToComplete(EnterAnimation);
                TrySetInteractable(true);
            }

            IEnumerator Exit()
            {
                TrySetInteractable(false);
                yield return _anim.WaitForAnimationToComplete(ExitAnimation);
            }
        }
        
        private void TrySetInteractable(bool interactable)
        {
            if (_selectable == null) return;
            _selectable.interactable = interactable;
        }
    }
}
