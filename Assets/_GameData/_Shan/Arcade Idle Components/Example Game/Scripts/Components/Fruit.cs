using System;
using UnityEngine;
using System.Collections;

namespace ArcadeIdle
{
    //[RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Fruit : MonoBehaviour
    {
        public SpecialBackpack.ResourceType resourceType;
    }
}