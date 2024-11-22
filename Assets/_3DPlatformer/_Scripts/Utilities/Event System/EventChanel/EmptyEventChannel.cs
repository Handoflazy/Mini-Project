using UnityEngine;

namespace Utilities.Event_System.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Empty Channel")]
    
    public class EmptyEventChannel : EventChannel<Empty>
    {
        
    }
    public readonly struct Empty{}
}