using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace AWP
{
    public class DialogueInputIndicator : MonoBehaviour
    {
        [SerializeField]
        private Image _img;
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private AWLineView _lineView;
        

        private void LateUpdate()
        {
            SyncAnimation();
        }

        private void SyncAnimation()
        {
            _anim.SetBool("Indicating", _lineView.WaitingOnInput);
            _img.color = _lineView.TextTMP.color;
        }
    }
}
