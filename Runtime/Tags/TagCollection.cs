
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    [CreateAssetMenu(fileName = "Tag Collection")]
    public class TagCollection : SerializedScriptableObject
    {
        private const string AssetFolder = "Assets/Tag Collections/Scripts";
        private const string TagCollectionBase = AWPackage.RuntimePath + "/Tags/TagCollectionBase.txt";

        [SerializeField]
        private Object testAsset;
        [SerializeField]
        private string[] _tags = new string[32];

        [ShowInInspector] [ReadOnly]
        private Dictionary<string, List<AWTags>> _instances = new Dictionary<string, List<AWTags>>();

        public List<AWTags> GetInstances(string tag)
        {   
            return _instances[tag];
        }

        public bool ContainsInstanceOfTag(string tag)
        {
            if (!_instances.ContainsKey(tag)) return false;
            return !_instances[tag].IsNullOrEmpty();
        }

        public string[] GetTags()
        {
            return _tags;
        }

        public void AddInstance(AWTags tags)
        {
            if (tags.AppliedTags == 0) return;

            for (int i = 0; i < _tags.Length; i++)
            {
                if ((tags.AppliedTags & (1 << i)) == 0) continue;
                
                if (!_instances.ContainsKey(_tags[i]))
                {
                    _instances.Add(_tags[i], new List<AWTags>());
                }

                _instances[_tags[i]].Add(tags);
            }
        }

        public void RemoveInstance(AWTags tags)
        {
            if (tags.AppliedTags == 0) return;

            for (int i = 0; i < _tags.Length; i++)
            {
                if ((tags.AppliedTags & (1 << i)) == 0) continue;

                _instances[_tags[i]].Remove(tags);
            }
        }

        public bool ItemHasTag(GameObject gameObject, string tagName)
        {
            AWTags[] tagComponents = gameObject.GetComponents<AWTags>();
            if (tagComponents.IsNullOrEmpty()) return false;

            foreach (AWTags element in tagComponents)
            {
                if (element.Collection != this) continue;
                return (element.AppliedTags & (1 << NameToTag(tagName))) != 0;
            }

            return false;
        }

        public int NameToTag(string name)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if (_tags[i] != name) continue;
                return i;
            }

            throw new System.Exception($"Tag of name \"{name}\" does not exist!");
        }

        #if UNITY_EDITOR
            [Button()]
            private void GenerateAsset()
            {
                TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(TagCollectionBase);
                System.IO.Directory.CreateDirectory(AssetFolder);

                string newFileText = template.text;
                string scriptName = name + "Tags";
                string assetPath = AssetFolder + "/" + scriptName + ".cs";
                newFileText = newFileText.Replace("SCRIPT_NAME", scriptName);

                string variableText = "";
                for (int i = 0; i < _tags.Length; i++)
                {
                    if (_tags[i].IsNullOrWhitespace()) continue;
                    variableText += $"\tpublic const string {_tags[i]} = \"{_tags[i]}\";\n";
                }
                newFileText = newFileText.Replace("SCRIPT_DATA", variableText);

                Debug.Log(newFileText);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(assetPath, false))
                {
                    sw.Write(newFileText);
                }
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        #endif
    }
}
