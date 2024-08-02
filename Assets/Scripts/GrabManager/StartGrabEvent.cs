using UnityEngine;

namespace GrabManager
{
    public struct StartGrabEvent
    {
        public Transform EnemyGrabPoint { get; }
        public int PlayerGrabPointIndex { get; }

        public StartGrabEvent(Transform enemyGrabPoint, int playerGrabPointIndex)
        {
            EnemyGrabPoint = enemyGrabPoint;
            PlayerGrabPointIndex = playerGrabPointIndex;
        }
    }
}