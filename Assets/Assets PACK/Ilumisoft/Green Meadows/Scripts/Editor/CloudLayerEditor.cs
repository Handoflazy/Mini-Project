using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ilumisoft.GreenMeadows
{
    [CustomEditor(typeof(CloudLayer))]
    public class CloudLayerEditor : Editor
    {
        CloudLayer CloudLayer { get; set; }

        private void OnEnable()
        {
            CloudLayer = target as CloudLayer;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            Button button = new Button();
            button.text = "Generate Clouds";
            button.style.marginTop = 10;
            button.clicked += GenerateClouds;
            root.Add(button);

            return root;
        }

        public void GenerateClouds()
        {
            // Destroy all existing clouds that are controlled by this layer
            ClearClouds();

            // Set the seed to make sure clouds are spawned at the same position each startup
            Random.InitState(CloudLayer.RandomSeed);

            for (int i = 0; i < CloudLayer.NumberofClouds; i++)
            {
                var randomPoint = GetRandomPosition();
                SpawnRandomCloud(randomPoint, -randomPoint);
            }

            // Randomze the seed afterwards, otherwise all other calls relying on random would not be really random anymore
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        /// <summary>
        /// Returns a random position on the skydome
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRandomPosition()
        {
            var randomPoint = Vector3.zero;

            float radius = CloudLayer.SpawnRadius;

            do
            {
                randomPoint = Random.onUnitSphere * radius;
            } while (randomPoint.y < radius * CloudLayer.MinLevel || randomPoint.y > radius * CloudLayer.MaxLevel);

            return randomPoint;
        }

        /// <summary>
        /// Destroy all existing clouds that are controlled by this layer
        /// </summary>
        private void ClearClouds()
        {
            var children = new List<GameObject>();

            for(int i=0; i< CloudLayer.transform.childCount; i++)
            {
                var child = CloudLayer.transform.GetChild(i);

                children.Add(child.gameObject);
            }

            foreach(var child in children)
            {
                Undo.DestroyObjectImmediate(child);
            }
        }

        /// <summary>
        /// Spawns a random cloud at the given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        private void SpawnRandomCloud(Vector3 position, Vector3 normal)
        {
            // Cancel if no cloud prefab exists
            if (CloudLayer.CloudPrefabs.Count == 0)
            {
                return;
            }

            // Get a random prefab
            var prefab = CloudLayer.CloudPrefabs[Random.Range(0, CloudLayer.CloudPrefabs.Count)];

            // Instantiate the cloud and make it look at the center of the skydome
            var cloud = PrefabUtility.InstantiatePrefab(prefab, CloudLayer.transform) as GameObject;

            cloud.transform.position = position;
            cloud.transform.rotation = Quaternion.LookRotation(-normal);

            // Randomize the scale of the cloud
            cloud.transform.localScale *= Random.Range(CloudLayer.MinScale, CloudLayer.MaxScale);

            var scale = cloud.transform.localScale;

            scale.x *= Random.Range(1, CloudLayer.HorizontalStretch);

            cloud.transform.localScale = scale;

            cloud.transform.position += cloud.transform.forward * Random.value * 5.0f;

            Undo.RegisterCreatedObjectUndo(cloud, "Generate Clouds");
        }
    }
}