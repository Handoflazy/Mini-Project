
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtils;
using AdvancePlayerController;

namespace Platformer.Advanced
{
    public class TurnTowardController : MonoBehaviour
    {
        [SerializeField, Required] private PlayerController controller;
        public float turnSpeed = 50f;
        private Transform tr;

        private float currentYRotation;
        private const float FALLOFFANGLE = 90;

        private void Start()
        {
            tr = transform;
            currentYRotation = tr.localEulerAngles.y;
        }

        void LateUpdate()
        {
            Vector3 velocity = Vector3.ProjectOnPlane(controller.GetMovementVelocity(), tr.parent.up);
            if (velocity.magnitude < 0.001f)
                return;
            float angleDifferrence = VectorMath.GetAngle(tr.forward, velocity.normalized, tr.parent.up);
            
            float step = Mathf.Sign(angleDifferrence)
                *Mathf.InverseLerp(0f, FALLOFFANGLE, Mathf.Abs(angleDifferrence))*Time.deltaTime*turnSpeed;
            currentYRotation += Mathf.Abs(step) > Mathf.Abs(angleDifferrence) ? angleDifferrence : step;

            tr.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }
}