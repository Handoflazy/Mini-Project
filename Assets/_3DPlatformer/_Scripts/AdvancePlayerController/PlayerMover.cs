
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.AdvancePlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMover : MonoBehaviour
    {
        #region Fields
        [Header("Collider Settings")] [Range(0f, 1f)] [SerializeField]
        private float stepHeightRatio = 0.1f;

        [FormerlySerializedAs("_colliderHeight")] [SerializeField] private float colliderHeight = 2f;
        [FormerlySerializedAs("_colliderThickness")] [SerializeField] private float colliderThickness = 1f;
        [FormerlySerializedAs("_offset")] [SerializeField] private Vector3 offset = Vector3.zero;

       
        
        
        
        private Rigidbody rb;
        private Transform tr;
        [SerializeField,Required] CapsuleCollider col;
        RayCastSensor sensor;

        private bool isGrounded;
        private float baseSensorRange;
        private Vector3 currentGroundAdjustmentVelocity;
        private int currentLayer;

        [Header("Sensor Settings")] [SerializeField]
        private bool isInDebugMode;

        private bool isUsingExtendedSensorRange = true;

        #endregion

        private void Awake()
        {
            Setup();
            RecalculateColliderDimesions();
        }

        void Setup()
        {
            rb = GetComponent<Rigidbody>();
            tr = transform;
            
            rb.freezeRotation = true;
            rb.useGravity = false;
           
        }
        protected  void OnValidate()
        {
            tr = transform;
            if (gameObject.activeInHierarchy)
            {
                RecalculateColliderDimesions();
            }
        }

        private void RecalculateColliderDimesions()
        {
            col.height = colliderHeight*(1-stepHeightRatio);
            col.radius = colliderThickness / 2;
            col.center = offset*colliderHeight+ new Vector3(0,stepHeightRatio*col.height/2f,0);

            if (col.height / 2f < col.radius)
            {
                col.radius = col.height / 2f;
            }
            RecalibrateSensor();
        }

        private void RecalibrateSensor()
        {
            sensor??= new RayCastSensor(transform);
            sensor.SetCastOrigin(col.bounds.center);
            sensor.SetCastDirection(RayCastSensor.CastDirection.Down);
            RecalculateSensorLayerMask();

            const float safetyDistanceFactor = 0.001f; // Small factor added to prevent clipping issues when the sensor range is calculated
            
            float length = colliderHeight * (1f - stepHeightRatio) * 0.5f + colliderHeight * stepHeightRatio;
            baseSensorRange = length * (1f + safetyDistanceFactor) * tr.localScale.x;
            sensor.CastLength = length * tr.localScale.x;
            //sensor.SetCastRadius(new Vector3(colliderThickness/2, 0,colliderThickness/2));
        }

        private void RecalculateSensorLayerMask()
        {
            int objectLayer = gameObject.layer;
            int layerMask = Physics.AllLayers;

            for (int i = 0; i < 32; i++)
            {
                if (Physics.GetIgnoreLayerCollision(objectLayer, i))
                {
                    layerMask &= ~(1 << i);
                }
            }
            int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");   
            layerMask &= ~(1 << ignoreRaycastLayer);
            sensor.LayerMask = layerMask;
            currentLayer = objectLayer;
            
        }

        public void CheckForGround()
        {
            if(currentLayer !=gameObject.layer)
                RecalculateSensorLayerMask();
            
            currentGroundAdjustmentVelocity = Vector3.zero; 
            sensor.CastLength = isUsingExtendedSensorRange 
                ? baseSensorRange + colliderHeight * tr.localScale.x * stepHeightRatio
                : baseSensorRange;
            sensor.Cast();
            
            isGrounded = sensor.HasDetectedHit();

            if (!isGrounded)
                return;
            float distance = sensor.GetDistance();
            float upperLimit = colliderHeight * tr.localScale.x * (1f - stepHeightRatio) * 0.5f;
            float middle = upperLimit + colliderHeight * tr.localScale.x * stepHeightRatio;
            float distanceToGo = middle - distance;
            
            currentGroundAdjustmentVelocity = tr.up*(distanceToGo/Time.fixedDeltaTime);
            

        }

        public void SetVelocity(Vector3 velocity) => rb.linearVelocity = velocity + currentGroundAdjustmentVelocity;
        public void SetExtendSensorRange(bool isEntended)=>isUsingExtendedSensorRange = isEntended;
        public Vector3 GetGroundNormal() => sensor.GetNormal();
        public bool IsGrounded() => isGrounded;
    }
}