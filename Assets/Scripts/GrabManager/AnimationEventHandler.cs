using UnityEngine;

namespace GrabManager
{
    /// <summary>
    /// Helper class to trigger start and end grab events using Unity's animation events.
    /// This class communicates with the EventAggregator to publish these events.
    /// </summary>
    public class AnimationEventHelper : MonoBehaviour
    {
        [SerializeField] private EventAggregator eventAggregator; // Reference to the EventAggregator ScriptableObject

        /// <summary>
        /// Triggers the StartGrabEvent with the specified enemy grab point and player grab point index.
        /// </summary>
        /// <param name="enemyGrabPoint">The Transform of the enemy's grab point.</param>
        /// <param name="playerGrabPointIndex">The index of the player's grab point.</param>
        public void TriggerStartGrab(Transform enemyGrabPoint, int playerGrabPointIndex) =>
            eventAggregator.Publish(new StartGrabEvent(enemyGrabPoint, playerGrabPointIndex));

        /// <summary>
        /// Triggers the EndGrabEvent to signal the end of a grab interaction.
        /// </summary>
        public void TriggerEndGrab() => eventAggregator.Publish(new EndGrabEvent());
    }
}