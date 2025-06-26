using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Triggered by any of the events contained
    /// </summary>
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class CompoundGameEvent : GameEvent, ISerializationCallbackReceiver
    {
        private const string CreateItemName = "CompoundGameEvent";

        [SerializeField]
        private List<GameEvent> _gameEvents;

        public void OnAfterDeserialize()
        {
            _gameEvents.ForEach(x => x.RegisterListener(Raise));
        }

        public void OnBeforeSerialize()
        {
            //_gameEvents.ForEach(x => x.UnregisterListener(Raise));
        }
    }
}
