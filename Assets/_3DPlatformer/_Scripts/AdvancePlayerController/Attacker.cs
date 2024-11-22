using System;
using Platformer.AdvancePlayerController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AdvancePlayerController
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private float attackRadius;
        [SerializeField] private int attackPoint;
        [SerializeField] private LayerMask damageableMask;

        // Khai báo mảng results ở đây để tái sử dụng, tránh tạo mới mỗi lần tấn công
        private readonly Collider[] results = new Collider[10];
        private Vector3 origin => transform.position+ transform.TransformVector(offset);

        public bool showGizmo = false;

        public UnityEvent OnSwingSword;

        public void Attack()
        {
            // Sử dụng OverlapSphereNonAlloc để tối ưu hiệu năng
            var size = Physics.OverlapSphereNonAlloc(origin, attackRadius, results, damageableMask);

            // Duyệt qua các collider trong phạm vi tấn công
            for (int i = 0; i < size; i++)
            {
                var hitCollider = results[i];
                if (hitCollider.TryGetComponent(out Damageable health))
                {
                    
                    health.TakeDamage(attackPoint); // Sửa lỗi chính tả "TakeDame" thành "TakeDamage"
                }
            }
            OnSwingSword?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (!showGizmo)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(origin, attackRadius);
        }
    }
}