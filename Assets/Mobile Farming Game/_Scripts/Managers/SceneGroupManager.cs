using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HDROutputUtils;

namespace Systems.SceneManagement
{
	public class SceneGroupManager
	{
		#region Events
		public event Action<string> OnSceneLoaded = delegate { };
		public event Action<string> OnSceneUnloaded = delegate { };
		public event Action OnSceneGroupLoaded = delegate { };
		#endregion

		readonly AsyncOperationHandleGroup handleGroup = new AsyncOperationHandleGroup(10);


		private SceneGroup _activeSceneGroup;
		public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
		{
			_activeSceneGroup = group;
			var loadedScenes = new List<String>();

			await UndeadScenes();

			int sceneCount = SceneManager.sceneCount;
			for (int i = 0; i < sceneCount; i++)
			{
				loadedScenes.Add(SceneManager.GetSceneAt(i).name);
			}
			var totalScenesToLoad = _activeSceneGroup.Scenes.Count;
			var operationGroup = new AsyncOperationGroup(totalScenesToLoad);
			
			for (int i = 0; i < totalScenesToLoad; i++)
			{
				var sceneData = group.Scenes[i];
				if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name))
					continue;

				if (sceneData.Reference.State == SceneReferenceState.Regular)
				{
					var Operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
					operationGroup.Operations.Add(Operation);
				}
				else if (sceneData.Reference.State == SceneReferenceState.Addressable)
				{
					var Handle = Addressables.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
					handleGroup.Handles.Add(Handle);
				}
				OnSceneLoaded.Invoke(sceneData.Name);

			}

			while (!operationGroup.IsDone||!handleGroup.IsDone)
			{
				progress.Report((operationGroup.Progress + handleGroup.Progress) / 2);
				await Task.Delay(1000);
			}
			Scene activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneByType(SceneType.ActiveScene));

			if (activeScene.IsValid())
			{
				SceneManager.SetActiveScene(activeScene);
			}

			OnSceneGroupLoaded.Invoke();
		}
		public async Task UndeadScenes()
		{
			var scenes = new List<String>();
			var activeScene = SceneManager.GetActiveScene().name;

			int sceneCount = SceneManager.sceneCount;

			for (int i = 0; i < sceneCount; i++)
			{
				var sceneAt = SceneManager.GetSceneAt(i);
				if (!sceneAt.isLoaded) continue;

				var sceneName = sceneAt.name;
				if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
				if(handleGroup.Handles.Any(h=>h.IsValid()&&h.Result.Scene.name == sceneName)) continue;

				scenes.Add(sceneName);
			}

			var operationGroup = new AsyncOperationGroup(sceneCount);
			foreach (var scene in scenes)
			{
				var operation = SceneManager.UnloadSceneAsync(scene);
				if (operation == null) continue;

				operationGroup.Operations.Add(operation);
				OnSceneUnloaded.Invoke(scene);
			}

			foreach(var handle in handleGroup.Handles)
			{
				if (handle.IsValid())
				{
					Addressables.UnloadSceneAsync(handle);
				}
			}
			handleGroup.Handles.Clear();

			while (!operationGroup.IsDone)
			{
				await Task.Delay(100);
			}
			await Resources.UnloadUnusedAssets();
		}
	}

	public readonly struct AsyncOperationGroup // for regulary Scene
	{
		public readonly List<AsyncOperation> Operations;

		public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
		public bool IsDone => Operations.All(o => o.isDone);
		public AsyncOperationGroup(int initialCapacity)
		{
			Operations = new List<AsyncOperation>(initialCapacity);
		}
	}

	public readonly struct AsyncOperationHandleGroup // for Addressible Scene
	{
		public readonly List<AsyncOperationHandle<SceneInstance>> Handles;

		public float Progress => Handles.Count == 0 ? 0 : Handles.Average(h => h.PercentComplete);
		public bool IsDone => Handles.Count == 0 || Handles.All(h => h.IsDone);

		public AsyncOperationHandleGroup(int initialCapacity)
		{
			Handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
		}
	}

}