using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;
using Sirenix.Utilities;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.AWDialogueController)]
    [RequireComponent(typeof(DialogueRunner))]
    public class AWDialogueController : AWDialogueViewBase
    {
        protected const string EnterAnim = "DialogueController_Enter";
        protected const string ExitAnim = "DialogueController_Exit";

        [SerializeField]
        private Animator _animator;

        private DialogueRunner _dialogueRunner;
        private bool _startAutomatically;
        private string _startNode;

        protected void Awake()
        {

        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _dialogueRunner = GetComponent<DialogueRunner>();

            _startAutomatically = _dialogueRunner.startAutomatically;
            _startNode = _dialogueRunner.startNode;

            if (!_startAutomatically) _animator?.Play(ExitAnim, 0, 1);
            _dialogueRunner.startAutomatically = false;
        }

        private void Start()
        {
            if (_startAutomatically) StartDialogue();
        }

        public void StartDialogue()
        {
            StartAnimationRoutine(StartDialogueRoutine());

            Debug.Log("START");

            IEnumerator StartDialogueRoutine()
            {   
                yield return EnterAnimation();
                _dialogueRunner.StartDialogue(_startNode);
            }
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StartAnimationRoutine(RunLineRoutine());

            IEnumerator RunLineRoutine()
            {
                while (AnimationActive) yield return null;
                base.RunLine(dialogueLine, onDialogueLineFinished);
            }
        }

        public override void DialogueComplete()
        {
            StartAnimationRoutine(DialogueCompleteRoutine());

            IEnumerator DialogueCompleteRoutine()
            {
                yield return ExitAnimation();
            }
        }

        // public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        // {

        // }

        // public override void DismissLine(Action onDismissalComplete)
        // {
        //     onDismissalComplete?.Invoke();
        // }

        // public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        // {
        //     //onOptionSelected(0);
        // }

        public override void UserRequestedViewAdvancement()
        {
            _dialogueRunner.dialogueViews.ForEach((x) => 
            {
                if (x == this) return;
                x.UserRequestedViewAdvancement();
            });
        }

        #region Custom animations
            protected IEnumerator EnterAnimation()
            {
                yield return _animator?.WaitForAnimationToComplete(EnterAnim);
            }

            protected IEnumerator ExitAnimation()
            {
                yield return _animator?.WaitForAnimationToComplete(ExitAnim);
            }
        #endregion
    }
}