using System.Collections;
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

        bool isTyping = default;
        public void SetDialogue(string dialogueLine, ActorSO actor, bool isMainProtagonist)
        {
            actorNamePanel.SetActive(!isMainProtagonist);
            mainProtagonistNamePanel.SetActive(isMainProtagonist);
            
            if (!isMainProtagonist)
            {
                actorNameText.text = actor.ActorName;
            }
            linetext.text = dialogueLine;

        }
        IEnumerator TypeText(string line, float charactersPerSecond)
        {
            float timer = 0;
            float interval = 1 / charactersPerSecond;
            string textBuffer = null;
            char[] chars = line.ToCharArray();
            int i = 0;
            while (i < chars.Length)
            {
                if (timer < Time.deltaTime)
                {
                    textBuffer += chars[i];
                    linetext.text = textBuffer;
                    timer += interval;
                    i++;
                }
                else
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }
            }

        }
    }
}