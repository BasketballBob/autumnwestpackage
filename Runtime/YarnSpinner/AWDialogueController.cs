using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System.Linq;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.AWDialogueController)]
    [RequireComponent(typeof(DialogueRunner))]
    public class AWDialogueController : AWDialogueViewBase
    {
        protected const string EnterAnim = "DialogueController_Enter";
        protected const string ExitAnim = "DialogueController_Exit";
        protected const string TextboxEnterAnim = "Textbox_Enter";
        protected const string TextboxExitAnim = "Textbox_Exit";

        [SerializeField]
        private Animator _animator;
        [ShowInInspector]
        private RunnerState _currentState;
        [SerializeField]
        private string _startNode;

        public Action OnStartDialogue;
        public Action<LocalizedLine, Action> OnRunLine;
        public Action<LocalizedLine, Action> OnInterruptLine;
        public Action<Action> OnDismissLine;
        public Action<DialogueOption[], Action<int>> OnRunOptions;
        public Action OnUserRequestedViewAdvancement;
        public Action OnDialogueComplete;

        private DialogueRunner _dialogueRunner;
        private bool _startAutomatically;
        private List<AWDialogueViewBase> _childViews = new List<AWDialogueViewBase>();

        public enum RunnerState { Off, EnterAnimation, RunningLine, ExitAnimation };

        /// <summary>
        /// Used to halt the automatic progression of the dialogue (add custom animations / waiting)
        /// </summary>
        public override bool Paused
        { 
            get => base.Paused; 
            set 
            {
                _childViews.ForEach(x => x.Paused = value);
                base.Paused = value; 
            }
        }
        public bool IsRunning => _currentState != RunnerState.Off;
        public string StartNode { get => _startNode; set { _startNode = value; }}
        protected virtual bool EnterTextboxAutomatically => true;
        protected bool TextboxVisible { get; private set; }

        protected void Awake()
        {
            _dialogueRunner = GetComponent<DialogueRunner>();
            foreach (DialogueViewBase view in _dialogueRunner.dialogueViews)
            {
                if (view == this) continue;
                _childViews.Add(view as AWDialogueViewBase);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _startAutomatically = _dialogueRunner.startAutomatically;

            if (!_startAutomatically) 
            {
                _animator?.Play(ExitAnim, 0, 1);
                _animator?.Play(TextboxExitAnim, 1, 1);
            }
            _dialogueRunner.startAutomatically = false;
        }

        protected virtual void Start()
        {
            if (_startAutomatically) StartDialogue();
        }

        public void StartDialogue()
        {
            StartAnimationRoutine(StartDialogueRoutine());

            IEnumerator StartDialogueRoutine()
            {   
                yield return EnterAnimation();
                if (EnterTextboxAutomatically) yield return EnterTextboxAnimation();
                _dialogueRunner.StartDialogue(_startNode);
            }

            OnStartDialogue?.Invoke();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _currentState = RunnerState.RunningLine;
            StartAnimationRoutine(RunLineRoutine());

            IEnumerator RunLineRoutine()
            {
                while (Paused) yield return null;
                if (!TextboxVisible) yield return EnterTextboxAnimation();

                base.RunLine(dialogueLine, onDialogueLineFinished);
            }

            OnRunLine?.Invoke(dialogueLine, onDialogueLineFinished);
        }

        public override void DialogueComplete()
        {
            StartAnimationRoutine(DialogueCompleteRoutine());

            IEnumerator DialogueCompleteRoutine()
            {
                yield return ExitTextboxAnimation();
                yield return ExitAnimation();
            }

            OnDialogueComplete?.Invoke();
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            base.InterruptLine(dialogueLine, onDialogueLineFinished);

            OnInterruptLine?.Invoke(dialogueLine, onDialogueLineFinished);
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            base.DismissLine(onDismissalComplete);

            OnDismissLine?.Invoke(onDismissalComplete);
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            base.RunOptions(dialogueOptions, onOptionSelected);

            OnRunOptions?.Invoke(dialogueOptions, onOptionSelected);
        }

        public override void UserRequestedViewAdvancement()
        {
            if (Paused) return;

            _childViews.ForEach(x => x.UserRequestedViewAdvancement());
            OnUserRequestedViewAdvancement?.Invoke();
        }

        public IEnumerator ExitDialogueBox() => ExitTextboxAnimation();

        #region Events
            public IEnumerator WaitUntilComplete()
            {
                while (IsRunning)
                {
                    yield return null;
                }
            }
        #endregion

        #region Custom animations
            protected virtual IEnumerator EnterAnimation()
            {
                _currentState = RunnerState.EnterAnimation;
                yield return _animator?.WaitForAnimationToComplete(EnterAnim);
            }

            protected virtual IEnumerator ExitAnimation()
            {
                _currentState = RunnerState.ExitAnimation;
                yield return _animator?.WaitForAnimationToComplete(ExitAnim);
                _currentState = RunnerState.Off;
            }

            protected virtual IEnumerator EnterTextboxAnimation()
            {
                TextboxVisible = true;
                yield return _animator?.WaitForAnimationToComplete(TextboxEnterAnim, 1);
            }

            protected virtual IEnumerator ExitTextboxAnimation()
            {
                TextboxVisible = false;
                yield return _animator?.WaitForAnimationToComplete(TextboxExitAnim, 1);
            }
        #endregion
    }
}