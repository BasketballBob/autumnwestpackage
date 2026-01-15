using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AnimatorPlayer : MonoBehaviour
    {
        [SerializeField]
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

        [YarnCommand("PlayAnimationAtTime")]
        public void PlayAnimation(string name, int layer, float normalizedTime)
        {
            _anim.Play(name, layer, normalizedTime);
        }

        [YarnCommand("SetBool")]
        public void SetBool(string name, bool value)
        {
            _anim.SetBool(name, value);
        }

        [YarnCommand("SetFloat")]
        public void SetFloat(string name, float value)
        {
            _anim.SetFloat(name, value);
        }

        [YarnCommand("SetTrigger")]
        public void SetTrigger(string name)
        {
            _anim.SetTrigger(name);
        }

        [YarnCommand("WaitOnAnimation")]
        public IEnumerator WaitForAnimationToComplete(string name, int layer = 0)
        {
            yield return _anim.WaitForAnimationToComplete(name, layer);
        }
        #endregion
    }
}
