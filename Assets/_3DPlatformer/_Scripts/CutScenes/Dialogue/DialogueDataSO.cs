using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    [CreateAssetMenu(menuName = "CutsceneSystem/DialogueData")]
    public class DialogueDataSO : ScriptableObject
    {
        public List<DialogueLineSO> Conversation;
    }
}