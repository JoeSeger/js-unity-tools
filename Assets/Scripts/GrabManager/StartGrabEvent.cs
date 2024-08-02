using UnityEngine;

namespace GrabManager
{
    /// <summary>
    /// Event struct to signal the start of a grab interaction.
    /// Contains the enemy's grab point and the index of the player's grab point.
    /// </summary>
    public struct StartGrabEvent
    {
        /// <summary>
        /// Gets the enemy's grab point Transform.
        /// </summary>
        public Transform EnemyGrabPoint { get; }

        /// <summary>
        /// Gets the index of the player's grab point.
        /// </summary>
        public int PlayerGrabPointIndex { get; }

        /// <summary>
        /// Initializes a new instance of the StartGrabEvent struct.
        /// </summary>
        /// <param name="enemyGrabPoint">The Transform of the enemy's grab point.</param>
        /// <param name="playerGrabPointIndex">The index of the player's grab point.</param>
        public StartGrabEvent(Transform enemyGrabPoint, int playerGrabPointIndex)
        {
            EnemyGrabPoint = enemyGrabPoint;
            PlayerGrabPointIndex = playerGrabPointIndex;
        }
    }
}