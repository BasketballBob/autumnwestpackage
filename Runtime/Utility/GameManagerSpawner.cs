using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.GameManagerSpawner)]
    public class GameManagerSpawner : MonoBehaviour
    {
        [SerializeField]
        private AWGameManager _gameManagerPrefab;

        private void Awake()
        {
            if (AWGameManager.Current == null)
            {
                Instantiate(_gameManagerPrefab);
            }
        }
    }
}
