using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GrabManager
{
    /// <summary>
    /// Manages the player's grab interactions with enemies using Unity's Animation Rigging package.
    /// Handles the synchronization of the player's position and rotation during grab animations.
    /// Ensures proper disposal of resources.
    /// </summary>
    public class PlayerGrabManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> playerGrabPoints; // Multiple grab points on the player
        [SerializeField] private Rig playerRig;
        [SerializeField] private TwoBoneIKConstraint rightHandIK;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private EventAggregator eventAggregator; // Reference to the ScriptableObject
        [SerializeField] private double syncTime = 0.5; // Duration to smoothly sync position and rotation

        private Transform _currentEnemyGrabPoint;
        private Transform _selectedPlayerGrabPoint;

        /// <summary>
        /// Subscribes to the StartGrabEvent and EndGrabEvent when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            eventAggregator.Subscribe<StartGrabEvent>(OnStartGrab);
            eventAggregator.Subscribe<EndGrabEvent>(OnEndGrab);
        }

        /// <summary>
        /// Unsubscribes from the StartGrabEvent and EndGrabEvent when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            eventAggregator.Unsubscribe<StartGrabEvent>(OnStartGrab);
            eventAggregator.Unsubscribe<EndGrabEvent>(OnEndGrab);
        }

        /// <summary>
        /// Handles the StartGrabEvent to initiate the grab process.
        /// Validates the player grab point index, activates the rig, and starts the smooth synchronization.
        /// </summary>
        /// <param name="startGrabEvent">The event containing the enemy grab point and player grab point index.</param>
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
            await SmoothSync();
        }

        /// <summary>
        /// Handles the EndGrabEvent to end the grab process.
        /// Deactivates the rig and resets the player's parent transform.
        /// </summary>
        /// <param name="endGrabEvent">The event signaling the end of the grab.</param>
        private void OnEndGrab(EndGrabEvent endGrabEvent)
        {
            playerRig.weight = 0f; // Deactivate the rig
            transform.parent = null;
        }

        /// <summary>
        /// Synchronizes the player's position and rotation with the enemy's grab point.
        /// </summary>
        private void SyncPlayerToEnemy()
        {
            if (_currentEnemyGrabPoint == null) return;

            rightHandIK.data.target.position = _currentEnemyGrabPoint.position;
            rightHandIK.data.target.rotation = _currentEnemyGrabPoint.rotation;
            leftHandIK.data.target.position = _selectedPlayerGrabPoint.position;
            leftHandIK.data.target.rotation = _selectedPlayerGrabPoint.rotation;
        }

        /// <summary>
        /// Smoothly synchronizes the player's position and rotation with the enemy's grab point over a specified duration.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SmoothSync()
        {
            transform.parent = _selectedPlayerGrabPoint;
            double elapsedTime = 0.0;

            Vector3 initialPosition = transform.position;
            Quaternion initialRotation = transform.rotation;

            while (elapsedTime < syncTime)
            {
                elapsedTime += Time.deltaTime;
                float t = (float)(elapsedTime / syncTime);

                transform.position = Vector3.Lerp(initialPosition, _currentEnemyGrabPoint.position, t);
                transform.rotation = Quaternion.Slerp(initialRotation, _currentEnemyGrabPoint.rotation, t);

                await Task.Yield();
            }

            transform.position = _currentEnemyGrabPoint.position;
            transform.rotation = _currentEnemyGrabPoint.rotation;
            SyncPlayerToEnemy(); // Final sync to ensure exact match
        }
    }
}
