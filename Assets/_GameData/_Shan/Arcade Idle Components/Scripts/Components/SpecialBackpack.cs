using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Special Backpack", 0)]
    public class SpecialBackpack : MonoBehaviour
    {
        public enum ResourceType
        {
            None,
            Apple,
            Orange,
            Burger,
            Plate,
            Package,
            Ball,
        }

        [Serializable]
        private struct Resource
        {
            public ResourceType Type;
            public GameObject Prefab;
        }

        [SerializeField] private Resource[] _resources;
        [BoxGroup("SPAWNER")] [SerializeField] private Transform[] _spawnPoints;
        //[BoxGroup("SPAWNER")] [SerializeField] private Transform _spawnPointAtMarket;

        public ResourceType ShowedItemsType { get; private set; } = ResourceType.None;
        public int ItemsCount
        {
            get => _itemsCount;
            private set
            {
                _itemsCount = Mathf.Clamp(value, 0, _spawnPoints.Length);
                HideItems();
                ShowItems();
            }
        }
        //Get EndPoint By Getting Counter Spawn Point
        public Transform SpawnPoint()
        {
            if (_itemsCount >= _spawnPoints.Length)
                return _spawnPoints[_itemsCount - 1];
            else
                return _spawnPoints[_itemsCount];
        }

        private int _itemsCount;
        private List<GameObject> _spawnedItems = new List<GameObject>();

        private void Start()
        {
            //CheckForErrors();
        }
        
        /*private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_resources.Length == 0)
                {
                    Debug.LogError("Special Backpack: resources settings is empty.");
                }
            }
        }*/

        private void DestroyItems()
        {
            for (var n = _spawnedItems.Count; n > 0; n--)
            {
                Destroy(_spawnedItems[n - 1]);
                _spawnedItems.RemoveAt(n - 1);
            }
        }

        private void SpawnItems()
        {
            var resource = _resources.FirstOrDefault(m => m.Type.Equals(ShowedItemsType));
            
            var list = new List<GameObject>();
            foreach (var spawnPoint in _spawnPoints)
            {
                var item = Instantiate(resource.Prefab, spawnPoint);
                list.Add(item);
            }
            
            _spawnedItems.AddRange(list);
        }

        private void HideItems()
        {
            foreach (var item in _spawnedItems)
            {
                item.SetActive(false);
            }
        }

        private void ShowItems()
        {
            for (var n = 0; n < _itemsCount; n++)
            {
                _spawnedItems[n].SetActive(true);
            }
        }

        /// <summary>
        /// Adds items to the backpack by resource type.
        /// </summary>
        public void AddItems(int count, ResourceType resourceType)
        {
            if (ShowedItemsType == resourceType) //Can add only if resource type match
            {
                ShowedItemsType = resourceType;
                ItemsCount += count;
            }
            else if (ItemsCount == 0)
            {
                ShowedItemsType = resourceType;
                DestroyItems();
                SpawnItems();
                ItemsCount += count;
            }
        }
        /// <summary>
        /// Adds items to the market by player.
        /// </summary>
        public void AddItemsToMarket(int count, ResourceType resourceType)
        {
            if (ShowedItemsType == resourceType) //Can add only if resource type match
            {
                ShowedItemsType = resourceType;
                ItemsCount += count;
            }
            else if (ItemsCount == 0)
            {
                ShowedItemsType = resourceType;
                DestroyItems();
                //SpawnItemsAndAnimate();
                ItemsCount += count;
            }
        }

        /// <summary>
        /// Removes items from the backpack.
        /// </summary>
        public void RemoveItems(int count)
        {
            ItemsCount -= count;
        }

        /// <summary>
        /// Returns true if backpack is full.
        /// </summary>
        public bool IsBackpackFull()
        {
            return ItemsCount == _spawnPoints.Length;
        }
    }
}
