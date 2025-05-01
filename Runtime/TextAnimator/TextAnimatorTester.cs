using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Febucci.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class TextAnimatorTester : MonoBehaviour
    {
        [SerializeField]
        private TypewriterCore _typewriter;

        private void Start()
        {
            Display();
        }

        [Button]
        public void Display()
        {
            _typewriter.StartShowingText(true);
        }
    }
}
