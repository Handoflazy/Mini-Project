using UnityEngine;

namespace Platformer
{
    public class DescriptionBaseSO : SerializableScriptableObject
    {
        [TextArea] public string description;
    }
}
