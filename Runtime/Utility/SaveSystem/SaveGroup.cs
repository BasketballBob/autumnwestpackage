using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class SaveGroup<TData>
    {
        [SerializeField] [InlineProperty]
        private LabeledList<TData> _labeledList = new LabeledList<TData>();

        public void SaveData(Object obj)
        {
            //Debug.Log($"SAVE DATA {obj} {obj is not ISaveable} {obj is not ISaveable<TData>} {typeof(TData)}");
            if (obj is not ISaveable<TData>) return;
            ISaveable<TData> saveable = obj as ISaveable<TData>;
            //Debug.Log($"SAVE DATA 2 {saveable}");

            _labeledList.SetItem(obj.name, saveable.GetSaveData());
        }

        public void LoadData(Object obj)
        {
            Debug.Log($"LoadData 1 {obj}");
            if (obj is not ISaveable<TData>) return;
            ISaveable<TData> saveable = obj as ISaveable<TData>;
            Debug.Log($"LoadData 2 {obj}");

            if (_labeledList.HasLabel(obj.name))
            {
                Debug.Log($"LoadData LABEL {obj}");
                saveable.LoadSaveData(_labeledList[obj.name]);
            }
            else 
            {   
                Debug.Log($"LoadData DEFAULT {obj}");
                saveable.LoadEmptyData();
            }
        }

        public void SaveResources(string path)
        {
            Resources.LoadAll(path).ForEach(x =>
            {
                SaveData(x);
            });
        }

        public void LoadResources(string path)
        {
            Resources.LoadAll(path).ForEach(x =>
            {
                LoadData(x);
            });
        }
    }
}
