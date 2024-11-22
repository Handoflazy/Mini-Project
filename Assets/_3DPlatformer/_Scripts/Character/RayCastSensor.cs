using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public class RayCastSensor
    {
        public float CastLength = 1f;
        public LayerMask LayerMask = 255;
        
        Vector3 _origin = Vector3.zero;
        private Transform _tf;
        
        public enum CastDirection { Forward, Backward, Left, Right, Up, Down }

        private CastDirection _castDirection;
        private RaycastHit _hitInfor;

        private Vector3 _boxWidth = new Vector3(0.2f, 0.2f, 0.2f);

        public RayCastSensor(Transform transform)
        {
            _tf = transform;
        }

        public void Cast()
        {
            Vector3 origin = _tf.TransformPoint(_origin);
            Vector3 direction = GetCastDirection();
            
            //Physics.BoxCast(origin,_boxWidth, direction, out _hitInfor,quaternion.identity, CastLength, LayerMask,QueryTriggerInteraction.Ignore);
            Physics.Raycast(origin, direction, out _hitInfor, CastLength, LayerMask,QueryTriggerInteraction.Ignore);
        }
        public bool HasDetectedHit()=>_hitInfor.collider != null;
        public Vector3 GetNormal() => _hitInfor.normal;
        public float GetDistance()=>_hitInfor.distance;
        public Collider GetCollider()=> _hitInfor.collider;
        public Transform GetTransform() => _hitInfor.transform;
        
        
        public void SetCastDirection(CastDirection castDirection)=>_castDirection = castDirection;
        //public void SetCastRadius(Vector3 boxWidth)=>_boxWidth = boxWidth;
        public void SetCastOrigin(Vector3 origin) => _origin = _tf.InverseTransformPoint(origin);
        private Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Forward => _tf.forward,
                CastDirection.Backward => -_tf.forward,
                CastDirection.Left => -_tf.right,
                CastDirection.Right => _tf.right,
                CastDirection.Up => _tf.up,
                CastDirection.Down => -_tf.up,
                _ => Vector3.one
            };
        }

        public void DrawDebug()
        {
            Debug.DrawLine(_tf.position, _tf.position + GetCastDirection() * CastLength, Color.red);
        }
    }
}