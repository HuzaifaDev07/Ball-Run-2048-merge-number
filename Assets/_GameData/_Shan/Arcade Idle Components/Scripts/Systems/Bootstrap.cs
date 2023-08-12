using System.Collections.Generic;
using System.Linq;
using ArcadeIdle;
using NaughtyAttributes;
using UnityEngine;

namespace Idle_Arcade_Components.Scripts.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private List<Initializable> _scriptsToInitializeOnAwake = new List<Initializable>();
        [SerializeField]
        private List<Initializable> _scriptsToInitializeOnStart = new List<Initializable>();

        //[Button]
        //public void GetObjectsForAwake()
        //{
        //    _scriptsToInitializeOnAwake = FindObjectsByType<Initializable>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        //}
        
        //[Button]
        //public void GetObjectsForStart()
        //{
        //    _scriptsToInitializeOnStart = FindObjectsByType<Initializable>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        //}
        
        private void Awake()
        {
            Initialize(_scriptsToInitializeOnAwake);
        }

        private void Start()
        {
            Initialize(_scriptsToInitializeOnStart);
        }

        private void Initialize(List<Initializable> scripts)
        {
            foreach (var script in scripts)
            {
                if(script == null) continue;
                script.Initialize();
            }
        }
    }
}