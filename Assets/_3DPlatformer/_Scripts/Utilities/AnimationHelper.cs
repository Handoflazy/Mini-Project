using UnityEngine;
using UnityEngine.Events;

namespace Platformer.Utilities
{
    public class AnimationHelper: MonoBehaviour
    {
        public UnityEvent AnimationEvent;

        public void Raise()
        {
            AnimationEvent?.Invoke();
        }
    }
}