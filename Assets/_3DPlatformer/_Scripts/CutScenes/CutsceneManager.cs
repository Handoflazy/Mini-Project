using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Platformer.CutScenes
{
    public class CutsceneManager : MonoBehaviour
    {
        private PlayableDirector _activePlayableDirector;
        [SerializeField] private InputReader _inputReader = default;
        private CutsceneData _cutsceneData;

        private void Awake()
        {
            _inputReader.AdvanceDialogueEvent +=OnAdvance;
        }

        private void OnDestroy()
        {
            _inputReader.AdvanceDialogueEvent -=OnAdvance;
        }

        public void Play(PlayableDirector activePlayableDirector)
        {
            _activePlayableDirector = activePlayableDirector;
            activePlayableDirector.Play();
            activePlayableDirector.stopped += ctx => CutsceneEnded();
            _inputReader.EnableDialogueInput();
        }

        void OnAdvance()
        {   
        }

        void PauseTimeline()
        {
            _activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        void ResumeTimelien()
        {
            _activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        void CutsceneEnded()
        {
            _inputReader.EnableGameplayInput();
        }
    }
}