using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class Rigidbody2DSpawner : ObjectSpawner<Rigidbody2D>
    {
        protected override Rigidbody2D CreateObject()
        {
            Rigidbody2D rb = Instantiate(_objectSelector.GetItem(), (Vector2)transform.position +
                _pointArea.GetLocalPoint(), Quaternion.identity); 
            return rb;
        }

        public virtual void LaunchTowards(Vector2 target, float magnitude, float eulerZ)
        {
            Spawn(x =>
            {
                x.rotation = eulerZ;
                x.LaunchTowards(target, magnitude);
            });
        }
    }
}
