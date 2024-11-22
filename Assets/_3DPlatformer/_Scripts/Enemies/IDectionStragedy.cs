using Utilities.ImprovedTimers;
using UnityEngine;

namespace Platformer
{
    public interface IDectionStragedy
    {
        bool Execute(Transform player, Transform detector, CountdownTimer detectionTimer);
    }
}