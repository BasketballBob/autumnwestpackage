using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorPlayer : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private Animator _anim;

        private void Reset()
        {
            _anim = GetComponent<Animator>();
        }

        #region Yarn Commands
        [YarnCommand("PlayAnimation")]
        public void PlayAnimation(string name, int layer = 0)
        {
            _anim.Play(name, layer);
        }

        [YarnCommand("PlayAnimation")]
        public void PlayAnimation(string name, int layer, float normalizedTime)
        {
            _anim.Play(name, layer, normalizedTime);
        }

        [YarnCommand("WaitOnAnimation")]
        public IEnumerator WaitForAnimationToComplete(string name, int layer = 0)
        {
            yield return _anim.WaitForAnimationToComplete(name, layer);
        }
        #endregion
    }
}
