
using System;
using System.SceneManagement;
using Platformer._Scripts.Utilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.LoadScene
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private int loadGroupSceneIndex;
        [SerializeField] private Button playerButton;
        [SerializeField] private Button exitButton;
        private SceneLoader sceneLoader;
        private void Awake()
        {
            sceneLoader = GlobalObjectManager.Instance.GetObject(10000).GetComponent<SceneLoader>();
            playerButton.onClick.AddListener(()=>_ = sceneLoader.LoadSceneGroup(loadGroupSceneIndex));
            exitButton.onClick.AddListener(()=>Helpers.QuitGame());
        }

    
    }
}