using Platformer.CutScenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    public class DialogueControlMixerBehaviour : PlayableBehaviour
    {
        private CutsceneManager cutsceneManager = default;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if(Application.isPlaying == true)
            {  
                // Default state
                cutsceneManager = playerData as CutsceneManager;

                int inputCount = playable.GetInputCount();

                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight(i);

                    if (inputWeight > 0f)
                    {
                        ScriptPlayable<DialogueControlBehaviour> inputPlayable = (ScriptPlayable<DialogueControlBehaviour>)playable.GetInput(i);
                        DialogueControlBehaviour behaviour = inputPlayable.GetBehaviour();

                        behaviour.DisplayDialogueLine();
                    }
                }
            }

        } 
    }
}