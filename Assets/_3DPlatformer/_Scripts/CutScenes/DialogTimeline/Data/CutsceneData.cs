using System;
using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CutsceneData
{
    // We can make this a list
     public DialogueDataSO DialogueDataSO;
    [Tooltip("Event placeholder that will be called after DialogueData reached its end.")]
    public string Event; 
}