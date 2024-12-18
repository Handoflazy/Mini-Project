using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.AbilitySystem
{
    public class AbilityView : MonoBehaviour
    {
        [SerializeField] public AbilityButton[] buttons;

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Initialize(i);
            }
        }
        public void UpdateRadical(float progress)
        {
            if (float.IsNaN(progress))
            {
                progress = 0;
                
            }
            Array.ForEach(buttons,button =>button.UpdateRadicalFill(progress));
        }

        public void UpdateButtonSprite(IList<Ability> abilities)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < abilities.Count)
                {
                    buttons[i].UpdateButtonSprite(abilities[i].data.Icon);{}
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}