using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TransformSpawner : ObjectSpawner<Transform>
    {
        protected override Transform CreateObject()
        {
            Transform trans = Instantiate(_objectSelector.GetItem(), transform.position +
                _pointArea.GetLocalPoint(), Quaternion.identity, SpawnParent);
            return trans;
        }
    }
}
