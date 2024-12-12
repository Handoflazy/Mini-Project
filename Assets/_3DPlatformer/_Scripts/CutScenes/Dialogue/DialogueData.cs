using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Platformer.CutScenes.DialogClip.Data
{
    [CreateAssetMenu(menuName = "CutsceneSystem/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        [Tooltip("Leave this empty if the dialogue doesn't use timeline.")]
        public TimelineAsset TimelineAsset;

        public List<DialogueLine> Conversation;
    }

    [Serializable]  
    public class DialogueLine
    {
        public Sprite Figure;
        public string Actorname;
        [TextArea(3, 3)] public string Sentence;
    }
}