using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.EventChannel;
using Image = UnityEngine.UI.Image;

namespace Platformer.AbilitySystem
{
    /// <summary>
    /// for test now
    /// </summary>
    public class AbilityButton : MonoBehaviour
    {
        public Image radialImage;
        public Image abilityImage;
        public int index;
        [SerializeField] private IntEventChannel abilityPressedEvent;

        public void Initialize(int index)
        {
            this.index = index;
        }
        private void Start()
        {
            abilityPressedEvent.Invoke(index);
        }

        public void UpdateButtonSprite(Sprite newIcon)
        {
            abilityImage.sprite = newIcon;
        }

        public void UpdateRadicalFill(float progess)
        {
            if (radialImage)
            {
                radialImage.fillAmount = progess;
            }
        }
    }
}