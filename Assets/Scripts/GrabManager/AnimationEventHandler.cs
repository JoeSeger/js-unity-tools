using UnityEngine;

namespace GrabManager
{
    public class AnimationEventHelper : MonoBehaviour
    {
        [SerializeField] private EventAggregator eventAggregator;

        public void TriggerStartGrab(Transform enemyGrabPoint, int playerGrabPointIndex) =>
            eventAggregator.Publish(new StartGrabEvent(enemyGrabPoint, playerGrabPointIndex));


        public void TriggerEndGrab() => eventAggregator.Publish(new EndGrabEvent());
    }
}