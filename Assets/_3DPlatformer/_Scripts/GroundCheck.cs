using UnityEngine;

namespace Platformer
{
    public class GroundCheck : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float _groundDistance = 0.08f;
        [SerializeField] private LayerMask _groundMask;
        public bool IsGrounded { get; private set; }

        private void Update()
        {
            // Tạo một vị trí dưới chân nhân vật để kiểm tra va chạm với mặt đất
            Vector3 position = transform.position + Vector3.down * _groundDistance;

            // Sử dụng Physics.CheckSphere để kiểm tra va chạm
            IsGrounded = Physics.CheckSphere(position, _groundDistance, _groundMask);
        }

        // Gizmos để dễ dàng hiển thị bán kính va chạm trong cảnh
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * _groundDistance, _groundDistance);
        }
    }
}