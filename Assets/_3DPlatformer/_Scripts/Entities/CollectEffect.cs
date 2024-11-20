using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    public class CollectEffect : MonoBehaviour
    {
        [SerializeField] private GameObject spawnVFX;
        [SerializeField] private AudioClip soundSFX;

        public void Collect()
        {
            Instantiate(spawnVFX, transform.position, quaternion.identity);
            GetComponent<AudioSource>().Play();
        }
    }
}