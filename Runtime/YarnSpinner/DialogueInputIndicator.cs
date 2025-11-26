using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class DialogueInputIndicator : MonoBehaviour
    {
        [SerializeField]
        private AWLineView _lineView;
        [SerializeField]
        private Animator _anim;

        private void LateUpdate()
        {
            SyncAnimation();
        }

        private void SyncAnimation()
        {
            _anim.SetBool("Indicating", _lineView.WaitingOnInput);
        }
    }
}
