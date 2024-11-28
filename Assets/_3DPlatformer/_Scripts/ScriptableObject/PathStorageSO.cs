using Platformer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.ScriptableObjectData
{
    [CreateAssetMenu(fileName = "PathStorage", menuName = "Gameplay/Path Storage")]
    public class PathStorageSO : DescriptionBaseSO
    {
        [Space]
        [ReadOnly] public PathSO lastPathTaken;
    }
}