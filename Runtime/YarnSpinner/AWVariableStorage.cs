using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AWP;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AWVariableStorage : VariableStorageBehaviour
    {
        public static AWVariableStorageData Data = new AWVariableStorageData();

        private Dictionary<string, float> _floats = new Dictionary<string, float>();
        private Dictionary<string, string> _strings = new Dictionary<string, string>();
        private Dictionary<string, bool> _bools = new Dictionary<string, bool>();

        private List<IDictionary> Dictionaries => new List<IDictionary>() { _floats, _strings, _bools };

        private void OnEnable()
        {
            LoadFromGlobalData();
        }

        private void OnDisable()
        {
            SaveToGlobalData();
        }

        public override bool TryGetValue<T>(string variableName, out T result)
        {
            result = default;

            if (_floats.ContainsKey(variableName))
            {
                result = (T)(object)_floats[variableName];
                return true;
            }
            else if (_strings.ContainsKey(variableName))
            {
                result = (T)(object)_strings[variableName];
                return true;
            }
            else if (_bools.ContainsKey(variableName))
            {
                result = (T)(object)_bools[variableName];
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

        /// <summary>
        /// Saves the local component data into the global data
        /// </summary>
        public void SaveToGlobalData()
        {
            Data.EnsureValues(_floats, _strings, _bools);
        }

        /// <summary>
        /// Loads the global data locally for use
        /// </summary>
        public void LoadFromGlobalData()
        {
            _floats.EnsureValues(Data.Floats);
            _strings.EnsureValues(Data.Strings);
            _bools.EnsureValues(Data.Bools);
        }

        #region Helper methods
        private void SetDictValue<TValue>(Dictionary<string, TValue> dict, string variableName, TValue value)
        {
            if (!dict.ContainsKey(variableName)) dict.Add(variableName, value);
            else dict[variableName] = value;
        }

        // [Button]
        // public void TEST()
        // {
        //     TypeConverter converter = TypeDescriptor.GetConverter(typeof(bool));

        //     //Debug.Log($"CONVERT TEST {converter.CanConvertTo()}");
        // }

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
        [Sirenix.OdinInspector.ReadOnly]
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

public class AWVariableStorageData : ISaveable<AWVariableStorageSave>
{
    public Dictionary<string, float> Floats = new Dictionary<string, float>();
    public Dictionary<string, string> Strings = new Dictionary<string, string>();
    public Dictionary<string, bool> Bools = new Dictionary<string, bool>();

    /// <summary>
    /// Ensures the provided values are in the dictionary
    /// </summary>
    public void EnsureValues(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools)
    {
        Floats.EnsureValues(floats);
        Strings.EnsureValues(strings);
        Bools.EnsureValues(bools);
    }

    public AWVariableStorageSave GetSaveData()
    {
        return new AWVariableStorageSave()
        {
            Floats = new DictionarySave<string, float>(this.Floats),
            Strings = new DictionarySave<string, string>(this.Strings),
            Bools = new DictionarySave<string, bool>(this.Bools)
        };
    }

    public void LoadSaveData(AWVariableStorageSave loadData)
    {
        Debug.Log($"TEST DATA NULL {loadData == null}");
        if (loadData == null) return;

        Floats = loadData.Floats.GetDictionary();
        Strings = loadData.Strings.GetDictionary();
        Bools = loadData.Bools.GetDictionary();
    }
}

[System.Serializable]
public class AWVariableStorageSave
{
    public DictionarySave<string, float> Floats;
    public DictionarySave<string, string> Strings;
    public DictionarySave<string, bool> Bools;
}
