using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer.GamePlay
{
    
    public enum GameState
    {
        Gameplay, //regular state: player moves, attacks, can perform actions
        Pause, //pause menu is opened, the whole game world is frozen
        Inventory, //when inventory UI or cooking UI are open
        Dialogue,
        Cutscene,
        LocationTransition, //when the character steps into LocationExit trigger, fade to black begins and control is removed from the player
        Combat, //enemy is nearby and alert, player can't open Inventory or initiate dialogues, but can pause the game
    }
   // [CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 51)]
    public class GameStateSO : DescriptionBaseSO
    {
        public GameState CurrentGameState => currentGameState;

        [Header("Game states")] [SerializeField] [ReadOnly]
        private GameState currentGameState = default;

        [SerializeField] [ReadOnly] private GameState previousGameState;

        [Header("Broadcasting On")] [SerializeField]
        private BoolEventChannel onCombatStateEvent = default;

        private List<Transform> alertEnemies;

        private void Start()
        {
            alertEnemies = new List<Transform>();
        }

        public void AddAlertEnemy(Transform enemy)
        {
            if (!alertEnemies.Contains(enemy))
            {
                alertEnemies.Add(enemy);
            }
            UpdateGameState(GameState.Combat);
        }

        public void RemoveAlertEnemy(Transform enemy)
        {
            if ( alertEnemies.Contains(enemy))
            {
                alertEnemies.Remove(enemy);

                if (alertEnemies.Count == 0)
                {
                    UpdateGameState(GameState.Gameplay);
                }
            }
        }

        public void UpdateGameState(GameState newGameState)
        {
            if (currentGameState == newGameState) return;

            if (newGameState == GameState.Combat)
            {
                onCombatStateEvent.Invoke(true);
            }
            else
            {
                onCombatStateEvent.Invoke(false);
            }
            previousGameState = currentGameState;
            currentGameState = newGameState;
        }

        public void ResetToPreviousGameState()
        {
            if (previousGameState == currentGameState)
                return;
            if (previousGameState == GameState.Combat)
            {
                onCombatStateEvent.Invoke(false);
            }
            else if(currentGameState == GameState.Combat)
            {
                onCombatStateEvent.Invoke(true);
            }
            (previousGameState, currentGameState) = (currentGameState, previousGameState);
        }
    }
}