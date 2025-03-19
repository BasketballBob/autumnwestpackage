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

        private void Update()
        {
            if (AWGameManager.Current.Debug1Pressed)
            {
                Display();
            }
        }

        [Button]
        private void Display()
        {
            _typewriter.StartShowingText(true);
        }
    }
}
