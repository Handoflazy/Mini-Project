using System;
using DG.Tweening;
using UnityEngine;

namespace Platformer
{
    public class SpawnEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnVFX;
        [SerializeField] private float animationDuration = 1f;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBounce);
            if (_spawnVFX != null)
            {
                Instantiate(_spawnVFX, transform.position, Quaternion.identity);
            }
            GetComponent<AudioSource>().Play();
        }
    }
}