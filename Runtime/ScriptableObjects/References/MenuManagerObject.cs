using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class MenuManagerObject : SingletonReferenceObject<MenuManager>
    {
        private const string CreateItemName = "MenuManagerObject";
    }

    public interface IMenuManagerReference 
    {
        public MenuManager MenuManager { get; }
    }

    [System.Serializable]
    public sealed class MenuManageReferece : IMenuManagerReference
    {
        [SerializeField]
        private MenuManager _menuManager;

        public MenuManager MenuManager => _menuManager;
    }

    [System.Serializable]
    public sealed class MenuManagerObjectReference : IMenuManagerReference
    {
        [SerializeField]
        private MenuManagerObject _menuManager;

        public MenuManager MenuManager => _menuManager.Reference;
    }
}
