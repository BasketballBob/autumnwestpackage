using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class VariableTracker : MonoBehaviour
    {
        [SerializeField]
        private VariableTrackerDisplay _trackerDisplayBase;

        private static List<TrackedVariable> _trackedVariables = new List<TrackedVariable>();
        private List<VariableTrackerDisplay> _trackerDisplays = new List<VariableTrackerDisplay>();

        private void Start()
        {
            _trackerDisplays.Add(_trackerDisplayBase);
        }

        private void LateUpdate()
        {
            SyncDisplayCount();
            SyncDisplayValues();
        }

        public static void AddTrackedVariable<T>(string name, IFormattable variable) where T : IFormattable
        {
            if (VariableIsTracked(name)) return;
            _trackedVariables.Add(new TrackedVariable<T>(name, variable));
        }
        public static void AddTrackedVariableFunc<T>(string name, Func<T> func) where T : IFormattable
        {
            if (VariableIsTracked(name)) return;
            _trackedVariables.Add(new TrackedVariable<T>(name, new FormattableFunc<T>(func)));
        }

        public static void RemoveTrackedVariable(string name)
        {
            _trackedVariables.Remove(GetTrackedVariable(name));
        }

        public static TrackedVariable GetTrackedVariable(string name)
        {
            foreach (TrackedVariable element in _trackedVariables)
            {
                if (name == element.Name)
                {
                    return element;
                }
            }

            return null;
        }

        public static bool VariableIsTracked(string name) => GetTrackedVariable(name) != null;

        private void SyncDisplayCount()
        {
            while (_trackerDisplays.Count < _trackedVariables.Count)
            {
                VariableTrackerDisplay display = Instantiate(_trackerDisplayBase.gameObject, transform).GetComponent<VariableTrackerDisplay>();
                _trackerDisplays.Add(display);
            }

            for (int i = 0; i < _trackerDisplays.Count; i++)
            {
                if (i < _trackedVariables.Count) _trackerDisplays[i].gameObject.SetActive(true);
                else _trackerDisplays[i].gameObject.SetActive(false);
            }
        }

        private void SyncDisplayValues()
        {
            for (int i = 0; i < _trackerDisplays.Count; i++)
            {
                if (i < _trackedVariables.Count)
                {
                    _trackerDisplays[i].SetValues(_trackedVariables[i]);
                }
            }
        }
    }
}
