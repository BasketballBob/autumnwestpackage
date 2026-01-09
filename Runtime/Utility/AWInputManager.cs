using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AWP
{
    [RequireComponent(typeof(PlayerInput))]
    public abstract class AWInputManager : MonoBehaviour
    {
        protected PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            InitializeActions();
        }

        private void Update()
        {
            UpdateInputs();
        }

        /// <summary>
        /// Used to initialize actions and action maps
        /// </summary>
        protected abstract void InitializeActions();

        /// <summary>
        /// Updates both update and fixed update inputs
        /// </summary>
        protected abstract void UpdateInputs();
    }
}
