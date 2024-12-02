using System;
using UnityEngine;
using Utilities.Event_System.EventChannel;



namespace Platformer.Systems.SpawnSystem
{
    
    /// <summary>
    /// This class contains the function to call when play button is pressed
    /// </summary>
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private int sceneGroupIndexToLoad;
        //TODO: SAVE SYSTEM

        [Header("Broadcasting on")]
        [SerializeField]
        private IntEventChannel loadLocation;

        private bool hasSaveData;

        private void Start()
        {
            
        }

        public void StartNewGame()
        {
            hasSaveData = false;
            loadLocation.Invoke(sceneGroupIndexToLoad);
        }

        public void OnResetSaveDataPress()
        {
            hasSaveData = false;
        }
        public void ContinuePreviousGame()
        {
            //TODO: LOAD SAVE GAME, LOAD PREVIOUS LOCATION;
            
            
            
        }
    }
}