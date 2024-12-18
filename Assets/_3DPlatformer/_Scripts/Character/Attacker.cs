using UnityEngine;

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