using System;
using Platformer.CutScenes.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.CutScenes
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneManager:Singleton<CutsceneManager>
    {
        [SerializeField] private TimelineAsset m_timeline;
        private PlayableDirector m_director;

        private void Awake()
        {
            m_director = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            PlayCutscene(m_timeline);
        }

        private void PlayCutscene(TimelineAsset cutscene)
        {
            //TODO: BLOCK PLAYER INPUT
            m_director.playableAsset = cutscene;
            m_director.Evaluate();
            m_director.Play();
            m_director.stopped += OnCutsceneCompleted;
        }
        private void OnCutsceneCompleted(PlayableDirector director)
        {
            director.stopped -= OnCutsceneCompleted;

            // TODO return to normal gameplay
        }
        
    }
}