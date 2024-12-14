using Platformer.Dialogue;
using TMPro;
using UnityEngine;

namespace Platformer.UI
{
    public class UIDialogueManager: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI linetext = default;
        [SerializeField] private TextMeshProUGUI actorNameText = default;
        [SerializeField] private GameObject actorNamePanel;
        [SerializeField] private GameObject mainProtagonistNamePanel;

        public void SetDialogue(string dialogueLine, ActorSO actor, bool isMainProtagonist)
        {
            linetext.text = dialogueLine;
            actorNamePanel.SetActive(!isMainProtagonist);
            mainProtagonistNamePanel.SetActive(isMainProtagonist);
            
            if (!isMainProtagonist)
            {
                actorNameText.text = actor.ActorName;
            }
            
        }
    }
}