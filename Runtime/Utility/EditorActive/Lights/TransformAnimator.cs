using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    public abstract class TransformAnimator : EditorActiveAnimator
    {
        [SerializeField] [Required]
        protected Transform _anchor;
    }
}
