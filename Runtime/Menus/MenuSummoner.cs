using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class MenuSummoner : MonoBehaviour
    {
        [SerializeField]
        [AssetsOnly]
        private Menu _menuPrefab;

        private Menu _menuInstance;

        public void Push()
        {
            CreateInstance();
            _menuInstance.SkipInitialization();
            _menuInstance.PushSelf();
        }

        private void CreateInstance()
        {
            if (_menuInstance != null) return;
            _menuInstance = Instantiate(_menuPrefab);
            _menuInstance.OnPopFinish += DestroyInstance;
        }
        
        private void DestroyInstance()
        {
            if (_menuInstance == null) return;
            Destroy(_menuInstance.gameObject);
        }
    }
}
