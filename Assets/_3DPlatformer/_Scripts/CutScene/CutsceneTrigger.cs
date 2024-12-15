using System;
using UnityEngine;
using UnityEngine.Playables;
using Utilities.EventChannel;

namespace Platformer.CutScenes
{
    public class CutsceneTrigger: MonoBehaviour
    {
        [SerializeField] private CutsceneManager cutsceneManager = default;
        [SerializeField] private PlayableDirector playableDirector = default;
        
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool playOnce;
        private Vector3 position;
        private Quaternion rotation;

        [SerializeField] private PlayableDirectorChannelSO playCutsceneEvent;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            if (playOnStart)
            {
                playCutsceneEvent?.RaiseEvent(playableDirector);
            }
        }
        public void PlaySpecificCutscene()
        {
            if (playCutsceneEvent != null)
                playCutsceneEvent.RaiseEvent(playableDirector);

            if (playOnce)
                Destroy(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            PlaySpecificCutscene();
        }
    }
}