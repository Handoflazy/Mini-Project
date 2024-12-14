using UnityEngine;
using Utilities.Event_System.EventChannel;

namespace Platformer._3DPlatformer._Scripts.CutScene
{
    public class CutsceneSceneLoader : MonoBehaviour
    {
        [SerializeField] private int sceneIndexGroupToLoad = default;

        [Header("Broadcasting on")]
        [SerializeField] private IntEventChannel sceneLoadChannel = default;

        //Used to load a location or menu from a cutscene
        public void LoadScene()
        {
            sceneLoadChannel.Invoke(sceneIndexGroupToLoad);
        }
    }
}