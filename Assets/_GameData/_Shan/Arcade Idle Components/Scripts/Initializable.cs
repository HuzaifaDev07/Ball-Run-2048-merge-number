using System;
using UnityEngine;

namespace ArcadeIdle
{
    [Serializable]
    public abstract class Initializable : MonoBehaviour
    {
        public abstract void Initialize();
    }
}