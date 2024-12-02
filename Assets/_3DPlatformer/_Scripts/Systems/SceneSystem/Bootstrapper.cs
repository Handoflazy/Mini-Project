using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Mobile_Farming_Game._Scripts.Systems.SceneSystem
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static async void Init()
        {
            Debug.Log("Bootstrapper....");
            await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}
