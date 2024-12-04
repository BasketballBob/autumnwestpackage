using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PauseMenu : Menu
    {
        private bool PauseInput => false;

        private void Update()
        {
            if (PauseInput) CheckToTogglePause();
        }

        private void CheckToTogglePause()
        {
            //if (_cu)
        }
    }
}
