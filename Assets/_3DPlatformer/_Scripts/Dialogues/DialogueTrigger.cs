using Platformer.Dialogue;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer.CutScenes
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool playOnce;

        [SerializeField] private DialogueDataSO dialogue;
        [SerializeField] private DialogueDataChannelSO dialogueDataChannelSo;

        private void Start()
        {
            if (playOnStart)
            {
                dialogueDataChannelSo.RaiseEvent(dialogue);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            dialogueDataChannelSo.RaiseEvent(dialogue);
        }

        private void OnTriggerExit(Collider other)
        {
            if (playOnce)
            {
                Destroy(this);
            }
        }
    }
}