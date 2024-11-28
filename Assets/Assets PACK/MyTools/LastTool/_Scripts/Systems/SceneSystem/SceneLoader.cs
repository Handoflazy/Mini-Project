using System.Collections;
using System.Threading.Tasks;
using Systems.SceneManagement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Event_System.EventChannel;

namespace System.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image _loadingBar;
        [SerializeField] private float _fillSpeed = 0.5f;

        [SerializeField] private Canvas _loadingCanvas;
        [SerializeField] private Camera _loadingCamera;
        [SerializeField] private SceneGroup[] _sceneGroup;
        [Header("Broadcast On")]
        [SerializeField] private VoidEventChannel OnSceneReady;// picked up by the SpawnSystem
        

        private float _targetProgress;
        public bool IsLoading { get; private set; }


        public readonly SceneGroupManager manager = new SceneGroupManager();

        private void Awake()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded" + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded" + sceneName);
            manager.OnSceneGroupLoaded +=() => Debug.Log("Scene Group Loaded");


        }
        async void Start()
        {
            await LoadSceneGroup(0);
        }

        private void Update()
        {
            if (!IsLoading) return;

            float currentFillAmount = _loadingBar.fillAmount;
            float progressDiffer = Mathf.Abs(currentFillAmount-_targetProgress);
            float dynamicFillSpeed = progressDiffer * _fillSpeed;
            _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount,_targetProgress, dynamicFillSpeed*Time.deltaTime);
        }

        public void LoadSceneGroupWrapper(int index)
        {
            _ = LoadSceneGroup(index);
        }
        public async Task LoadSceneGroup(int index)
        {
            _loadingBar.fillAmount = 0;
            _targetProgress = 1f;

            if (index < 0 || index >= _sceneGroup.Length)
            {
                Debug.LogError("invalid scene group index" + index);
                return;
            }
            LoadProgress progress = new LoadProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);

            EnableLoadingCanvas();

            await manager.LoadScenes(_sceneGroup[index],progress);
            EnableLoadingCanvas(false);
            OnSceneReady.Invoke();
        }

        private void EnableLoadingCanvas(bool Enable = true)
        {
            IsLoading = Enable;
            _loadingCamera.gameObject.SetActive(Enable);
            _loadingCanvas.gameObject.SetActive(Enable);
        }
    }
    public class LoadProgress : IProgress<float>
    {
        public event Action<float> Progressed;
        const float RATIO = 1F;
        public void Report(float value)
        {
            Progressed?.Invoke(value/ RATIO);  
        }

    }
}