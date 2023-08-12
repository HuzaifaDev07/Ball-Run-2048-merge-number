using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Arcade Idle Components/Settings", order = 0)]
    public class Settings : ScriptableObject
    {
        [InfoBox("Show warnings if you forget to set something")]
        public bool ShowWarnings;
    }
}