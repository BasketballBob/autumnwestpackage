using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PreferenceAccessor : MonoBehaviour
    {
        public void SavePreferences() => AWGameManager.SaveManager.SavePreferences();
        public void LoadPreferences() => AWGameManager.SaveManager.LoadPreferences();
    }
}
