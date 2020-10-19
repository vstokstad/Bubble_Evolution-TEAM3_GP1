using Actor.Component;
using UnityEngine;

namespace Actor.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public float airborneBlendVelocityWindow = 5;
        
        private readonly int _animGrounded = Animator.StringToHash("isGrounded");
        private readonly int _animGroundedBlend = Animator.StringToHash("GroundedBlend");
        private readonly int _animAirborneBlend = Animator.StringToHash("AirborneBlend");
        
        private Rigidbody _body;
        private MovementController _movementController;
        private GroundCheck _groundCheck;
        private Animator _animator;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _movementController = GetComponent<MovementController>();
            _groundCheck = GetComponent<GroundCheck>();
            _animator = GetComponentInChildren<Animator>();
            
            _animator.Play("Grounded");
        }

        private void Update()
        {
            _animator.SetBool(_animGrounded, _groundCheck.isWalkable);

            Vector3 velocity = _groundCheck.WorldToGround(_body.velocity);
            
            _animator.SetFloat(_animGroundedBlend, Mathf.Abs(velocity.x) / _movementController.maxSpeed);

            float blendValue =
                Mathf.Clamp01((velocity.y + airborneBlendVelocityWindow) / (airborneBlendVelocityWindow * 2));
            _animator.SetFloat(_animAirborneBlend, blendValue);
        }
    }
}
