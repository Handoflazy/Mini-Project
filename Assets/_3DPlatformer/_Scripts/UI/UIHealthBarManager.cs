using Character;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class UIHealthBarManager : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSO protagonistHealth;


        public void UpdateHealthUI()
        {
            if (healthBarImage != null)
                healthBarImage.fillAmount =(float)protagonistHealth.CurrentHealth / protagonistHealth.MaxHealth;
        }
    }
}