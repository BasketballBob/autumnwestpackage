using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class SetActive : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _gameObjects = new List<GameObject>();

        public void SetObjectsActive(bool active)
        {
            _gameObjects.ForEach(x => x.SetActive(active));
        }
    }
}
