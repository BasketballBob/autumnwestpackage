using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sr;
        [SerializeField]
        private WeightedItemPool<Sprite> _sprites = new WeightedItemPool<Sprite>();

        private void Start()
        {
            _sr.sprite = _sprites.PullItem();
        }
    }
}
