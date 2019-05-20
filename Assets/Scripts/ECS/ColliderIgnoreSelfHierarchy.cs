using UnityEngine;

namespace ECS
{
    [RequireComponent(typeof(Collider))]
    public class ColliderIgnoreSelfHierarchy : MonoBehaviour
    {
        private Collider _collider;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _collider  = GetComponent<Collider>();
            var childColliders  = _transform.GetComponentsInChildren<Collider>();
            var parentColliders = _transform.GetComponentsInParent<Collider>();

            for (var i = 0; i < childColliders.Length; i++)
            {
                var childCollider = childColliders[i];
                Physics.IgnoreCollision(_collider, childCollider);
            }

            for (var i = 0; i < parentColliders.Length; i++)
            {
                var parentCollider = parentColliders[i];
                Physics.IgnoreCollision(_collider, parentCollider);
            }
        }
    }
}