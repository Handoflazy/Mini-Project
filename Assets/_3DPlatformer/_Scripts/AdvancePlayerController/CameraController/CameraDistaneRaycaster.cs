using System;
using Platformer.AdvancePlayerController;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Advanced
{
    public class CameraDistaneRaycaster : MonoBehaviour
    {
        [SerializeField, Required] private Transform cameraTransform;
        [SerializeField, Required] private Transform cameraTargetTransform;

        public LayerMask layerMask = Physics.AllLayers;
        public float minimumDistanceFromObstacles = 0.1f;
        public float smoothingFactor = 25f;
        public float sphereRadius = 0.5f;

        private Transform tr;
        float currentDistance;


        private void Awake()
        {
            tr = transform;
            layerMask &= ~(1<<LayerMask.NameToLayer("Ignore Raycast"));
            currentDistance = (cameraTargetTransform.position - tr.position).magnitude;
        }

        private void LateUpdate()
        {
            Vector3 castDirection = cameraTargetTransform.position - tr.position;
            
            float distance = GetCameraDistance(castDirection);
            
            currentDistance = Mathf.Lerp(currentDistance,distance,Time.deltaTime * smoothingFactor);
            cameraTransform.position = tr.position + castDirection.normalized * currentDistance;
        }

        private float GetCameraDistance(Vector3 castDirection)
        {
            float distance = castDirection.magnitude+minimumDistanceFromObstacles;
            /*if (Physics.Raycast(new Ray(tr.position, castDirection), out RaycastHit hit, distance, layerMask,
                    QueryTriggerInteraction.Ignore))
            {
                return Mathf.Max(0f,hit.distance - minimumDistanceFromObstacles);
            }*/
            if (Physics.SphereCast(tr.position, sphereRadius, castDirection, out RaycastHit hit, distance, layerMask,
                    QueryTriggerInteraction.Ignore))
            {
                return Mathf.Max(0f,hit.distance - minimumDistanceFromObstacles);
            }
            return castDirection.magnitude;
        }
        
        
    }
}