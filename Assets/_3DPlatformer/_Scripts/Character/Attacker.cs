using System;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AdvancePlayerController
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private GameObject attackCollider;

        public void EnableWeapon()
        {
            attackCollider.SetActive(true);
        }

        public void DisableWeapon()
        {
            attackCollider.SetActive(false);
        }
    }
}