using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AWVariableStorage : VariableStorageBehaviour
    {
        private Dictionary<string, float> _floats = new Dictionary<string, float>();
        private Dictionary<string, string> _strings = new Dictionary<string, string>();
        private Dictionary<string, bool> _bools = new Dictionary<string, bool>();

        private List<IDictionary> Dictionaries => new List<IDictionary>() { _floats, _strings, _bools };

        public override bool TryGetValue<T>(string variableName, out T result)
        {
            result = default;

            Dictionary<string, T> dict = GetDict<T>();
            if (dict == null) return false;

            if (GetDict<T>().ContainsKey(variableName))
            {
                result = GetDict<T>()[variableName];
                return true;
            }

            return false;
        }

        public override void SetValue(string variableName, float floatValue) => SetDictValue(_floats, variableName, floatValue);
        public override void SetValue(string variableName, string stringValue) => SetDictValue(_strings, variableName, stringValue);
        public override void SetValue(string variableName, bool boolValue) => SetDictValue(_bools, variableName, boolValue);
        public override void Clear() => Dictionaries.ForEach(x => x.Clear());
        public override bool Contains(string variableName) => Dictionaries.Any(x => x.Contains(variableName));

        public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
        {
            return (_floats, _strings, _bools);
        }

        public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
        {
            if (clear) Clear();

            SyncDict(_floats, floats);
            SyncDict(_strings, strings);
            SyncDict(_bools, bools);
        }

        public bool NodeIsVisited(string nodeName)
        {
            if (TryGetValue<float>($"$Yarn.Internal.Visiting.{nodeName}", out float result))
            {
                return result > 0;
            }

            return false;
        }

        #region Helper methods
        private void SetDictValue<TValue>(Dictionary<string, TValue> dict, string variableName, TValue value)
        {
            if (!dict.ContainsKey(variableName)) dict.Add(variableName, value);
            else dict[variableName] = value;
        }

        private Dictionary<string, TValue> GetDict<TValue>()
        {
            Type genericType = typeof(TValue);
            Debug.Log($"{typeof(float)} {genericType} {typeof(TValue)} Compare {typeof(TValue) == typeof(float)}");

            if (typeof(TValue) == typeof(float)) return _floats as Dictionary<string, TValue>;
            else if (typeof(TValue) == typeof(string)) return _strings as Dictionary<string, TValue>;
            else if (typeof(TValue) == typeof(bool)) return _bools as Dictionary<string, TValue>;
            else return null;
        }

        /// <summary>
        /// Ensures that the receiver contains all newValues
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="receiver"></param>
        /// <param name="newValues"></param>
        private void SyncDict<TValue>(Dictionary<string, TValue> receiver, Dictionary<string, TValue> newValues)
        {
            newValues.ForEach(x =>
            {
                SetDictValue(receiver, x.Key, x.Value);
            });
        }
        #endregion

        #region Debug
        [InfoBox("@DrawDebug()")]
        [ReadOnly]
        public bool _ignore;
        private string DrawDebug()
        {
            string output = "";

            output += "Floats: \n";
            DrawPairs(_floats);
            output += "Strings: \n";
            DrawPairs(_strings);
            output += "Bools: \n";
            DrawPairs(_bools);

            return output;

            void DrawPairs<TValue>(Dictionary<string, TValue> dict)
            {
                dict.ForEach(x =>
                {
                    output += $"{x.Key} - {x.Value} \n";
                });
            }
        }
        #endregion
    }
}
