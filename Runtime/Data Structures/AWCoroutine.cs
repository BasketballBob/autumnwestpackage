using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWCoroutine
    {
        // private Coroutine _coroutine;
        // private MonoBehaviour _attachedMono;

        // public bool IsRunning => _coroutine != null;

        // public void StartRoutine(MonoBehaviour monoBehaviour, IEnumerator routine)
        // {
        //     _attachedMono = monoBehaviour;
        //     monoBehaviour.StartCoroutine(HandlerRoutine());

        //     IEnumerator HandlerRoutine()
        //     {
        //         yield return routine;
        //         _coroutine = null;
        //     }
        // }

        // public void StopRoutine()
        // {
        //     if (!IsRunning) return;
        //     if (_attachedMono == null) return;

        //     _attachedMono.StopCoroutine(_coroutine);
        //     _coroutine = null;
        // }
    }
}
