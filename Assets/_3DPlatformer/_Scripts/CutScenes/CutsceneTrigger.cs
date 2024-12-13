using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Platformer.CutScenes
{
    public class CutsceneTrigger: MonoBehaviour
    {
        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private PlayableDirector playableDirector;
        
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool playOnce;
        private Vector3 position;
        private Quaternion rotation;
        private void Start()
        {
            if (playOnStart)
            {
                cutsceneManager.Play(playableDirector);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            cutsceneManager.Play(playableDirector);
        }

        private void OnTriggerExit(Collider other)
        {
            if(playOnce)
                Destroy(this);
        }
    }
}