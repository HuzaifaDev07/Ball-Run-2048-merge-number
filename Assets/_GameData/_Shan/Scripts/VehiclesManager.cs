using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class VehiclesManager : MonoBehaviour
    {
        [BoxGroup("PARENT TRANSFROM")]
        [SerializeField]
        private Transform _vehiclesParentObject;
        [BoxGroup("DESTINATION")]
        [SerializeField]
        private Transform[] _destinantions;
        [BoxGroup("DESTINATION")]
        [SerializeField]
        private StoppingPoint[] _stoppingPoints;
        [BoxGroup("DESTINATION")]
        [SerializeField]
        private Transform _finalPoint;
        [BoxGroup("Vehicle Prefab")]
        [SerializeField]
        private GameObject _vehiclePrefab;

        [InfoBox("Delay is used for Instantiate Vehicles After Time.")]
        [BoxGroup("DELAY")]
        [SerializeField]
        private bool _useDelay;
        [BoxGroup("DELAY")]
        [ShowIf("_useDelay")]
        [SerializeField]
        private float _delayTime = 0.1f;
        [BoxGroup("Capacity")]
        [SerializeField]
        private int[] _Capacity;

        private int vehiclesCapacity;

        private readonly List<GameObject> _spawnedVehicles = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnLoop());
        }
        private void SpawnVehicle()
        {
            var VehicleObject = Instantiate(_vehiclePrefab, _vehiclesParentObject);
            VehicleObject.SetActive(true);
            _spawnedVehicles.Add(VehicleObject);

            var VehicleGateway = VehicleObject.GetComponent<VehicleGateway>();
            VehicleGateway.SetDestination(_destinantions[DestinationPoint()]);
            _stoppingPoints[DestinationPoint()]._objectToAssign = VehicleObject;

            VehicleGateway.OrderFill += OnOderComplete;
        }
        private IEnumerator SpawnLoop()
        {
            var delay = _delayTime;

            while (true)
            {
                vehiclesCapacity = _Capacity[0];
                while (vehiclesCapacity > _spawnedVehicles.Count)
                {
                    SpawnVehicle();

                    yield return new WaitForSeconds(delay);
                }

                yield return new WaitWhile(() => _spawnedVehicles.Count == vehiclesCapacity);
                delay = _delayTime;
            }
        }
        private void OnOderComplete(GameObject vehicle)
        {
            _spawnedVehicles.Remove(vehicle);
            ReAssignDestinations();
            var VehicleGateway = vehicle.GetComponent<VehicleGateway>();
            VehicleGateway.SetDestination(_finalPoint,5f);
        }
        private void ReAssignDestinations()
        {
            for (int ind = 0; ind < _spawnedVehicles.Count; ind++)
            {
                _spawnedVehicles[ind].GetComponent<VehicleGateway>().SetDestination(_destinantions[ind]);

                _stoppingPoints[ind]._objectToAssign = _spawnedVehicles[ind];
            }
        }
        private int DestinationPoint()
        {
            return _spawnedVehicles.Count - 1;
        }
    }
}
