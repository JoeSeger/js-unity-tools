using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GrabManager
{
    public class PlayerGrabManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> playerGrabPoints; // Multiple grab points on the player
        [SerializeField] private Rig playerRig;
        [SerializeField] private TwoBoneIKConstraint rightHandIK;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private EventAggregator eventAggregator;
        [SerializeField] private double syncTime = 0.5;// Reference to the ScriptableObject

        private Transform _currentEnemyGrabPoint;
        private Transform _selectedPlayerGrabPoint;

        private void OnEnable()
        {
            eventAggregator.Subscribe<StartGrabEvent>(OnStartGrab);
            eventAggregator.Subscribe<EndGrabEvent>(OnEndGrab);
        }

        private void OnDisable()
        {
            eventAggregator.Unsubscribe<StartGrabEvent>(OnStartGrab);
            eventAggregator.Unsubscribe<EndGrabEvent>(OnEndGrab);
        }

        private async void OnStartGrab(StartGrabEvent startGrabEvent)
        {
            if (startGrabEvent.PlayerGrabPointIndex < 0 || startGrabEvent.PlayerGrabPointIndex >= playerGrabPoints.Count)
            {
                Debug.LogError("Invalid player grab point index");
                return;
            }

            _currentEnemyGrabPoint = startGrabEvent.EnemyGrabPoint;
            _selectedPlayerGrabPoint = playerGrabPoints[startGrabEvent.PlayerGrabPointIndex];

            playerRig.weight = 1f; // Activate the rig
            await SmoothPositionSync();
            await SmoothSyncPlayerToEnemyRotation(); // Ensure final position and rotation sync
        }

        private void OnEndGrab(EndGrabEvent endGrabEvent)
        {
            playerRig.weight = 0f; // Deactivate the rig
            transform.parent = null;
        }

        private void SyncPlayerToEnemyRotation()
        {
            if (_currentEnemyGrabPoint == null) return;

            rightHandIK.data.target.rotation = _currentEnemyGrabPoint.rotation;
            leftHandIK.data.target.rotation = _selectedPlayerGrabPoint.rotation;
        }

        private void SyncPlayerToEnemyPosition()
        {
            if (_currentEnemyGrabPoint == null) return;

            rightHandIK.data.target.position = _currentEnemyGrabPoint.position;
            leftHandIK.data.target.position = _selectedPlayerGrabPoint.position;
        }

        private async Task SmoothPositionSync()
        {
            transform.parent = _selectedPlayerGrabPoint;
            // Time to smoothly sync position
            var elapsedTime = 0.0;

            var initialPosition = transform.position;

            while (elapsedTime < syncTime)
            {
                elapsedTime += Time.deltaTime;
                var t = (float)(elapsedTime / syncTime);

                transform.position = Vector3.Lerp(initialPosition, _currentEnemyGrabPoint.position, t);

                await Task.Yield();
            }

            transform.position = _currentEnemyGrabPoint.position;
            SyncPlayerToEnemyPosition(); // Final sync to ensure exact match
        }

        private async Task SmoothSyncPlayerToEnemyRotation()
        { 
            // Time to smoothly sync rotation
            var elapsedTime = 0.0;

            var initialRotation = transform.rotation;

            while (elapsedTime < syncTime)
            {
                elapsedTime += Time.deltaTime;
                var t = (float)(elapsedTime / syncTime);

                transform.rotation = Quaternion.Slerp(initialRotation, _currentEnemyGrabPoint.rotation, t);

                await Task.Yield();
            }

            transform.rotation = _currentEnemyGrabPoint.rotation;
            SyncPlayerToEnemyRotation(); // Final sync to ensure exact match
        }
    }
}
