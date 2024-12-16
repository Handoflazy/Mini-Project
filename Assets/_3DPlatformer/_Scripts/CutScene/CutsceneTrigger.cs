using UnityEngine;
using UnityEngine.Playables;
using Utilities.EventChannel;

namespace Platformer.CutScenes
{
    public class CutsceneTrigger: MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector = default;
        
        [SerializeField] private bool playOnStart = default;
        [SerializeField] private bool playOnce = default;
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