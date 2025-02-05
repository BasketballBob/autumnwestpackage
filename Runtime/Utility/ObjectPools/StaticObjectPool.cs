using System.Collections;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// An object pool that manually controls it's own elements
    /// Enabling and disabling objects to fit the count
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public abstract class StaticObjectPool<TObject> : ObjectPool<TObject> where TObject : Component
    {
        public int ActiveItemCount 
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                SyncItemCount();
            }   
        }
        private int _itemCount;

        protected override void Start()
        {
            base.Start();
            ActiveItemCount = _minItemCount;
        }

        protected void SyncItemCount()
        {
            // Create new items if necessary
            while (_poolItems.Count < ActiveItemCount)
            {
                CreateObject();
            }

            // Set desired count active
            for (int i = 0; i < _poolItems.Count; i++)
            {
                if (i < ActiveItemCount)
                {
                    EnableObject(_poolItems[i]);
                }
                else DisableObject(_poolItems[i]);
            }
        }

        protected void ModifyActiveItems()
        {
            for (int i = 0; i < ActiveItemCount; i++)
            {
                ActiveModification(_poolItems[i], i);
            }
        }
        protected abstract void ActiveModification(TObject obj, int index);
    }
}
