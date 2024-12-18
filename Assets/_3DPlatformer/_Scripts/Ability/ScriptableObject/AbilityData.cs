using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.AbilitySystem
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "Ability/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        [VerticalGroup("row1/left")] public AnimationClip AnimationClip;
        [VerticalGroup("row1/left")] public int AnimationHash;
        [VerticalGroup("row1/left")] public float Duration;
        [PreviewField(50,ObjectFieldAlignment.Right),HideLabel]
        [HorizontalGroup("row1",50)] public Sprite Icon;

        private void OnValidate()
        {
            AnimationHash = Animator.StringToHash(AnimationClip.name);
        }
    }
}