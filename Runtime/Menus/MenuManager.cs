using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Menu _baseMenu;

        private AWStack<MenuItem> _menuStack = new AWStack<MenuItem>();

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        private void Push(Menu menu)
        {
            _menuStack.Push(new MenuItem()
            {
                Menu = menu,

            });
        }

        private void PopMenu()
        {
            
        }

        private class MenuItem
        {
            public Menu Menu;
        }
    }
}
