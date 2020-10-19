using System.Collections;
using UnityEngine;

namespace Actor.Component
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpController : MonoBehaviour
    {
        [Range(0, 50)] public float jumpForce = 6f;

        [Tooltip("Time in seconds for the the character to start falling after letting go of the jump key.")]
        [Range(0.01f, 1)] public float timeToCancelJump = 0.2f;

        [Tooltip("The time window after a jump has started where it can be canceled, forces a minimum jump duration.")]
        public float cancelTimeWindow = 0.1f;

        private float _timeOfJump;

        private bool _isJumping;
        private bool _isCanceling;
        
        private Rigidbody _body;
        private Transform _transform;
        private GroundCheck _groundCheck;
        private CustomGravityController _gravityController;
        private DashController _dashController;

        public bool IsJumping => _isJumping;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _transform = transform;
            _groundCheck = GetComponent<GroundCheck>();
            _gravityController = GetComponent<CustomGravityController>();
            _dashController = GetComponent<DashController>();
        }

        public bool Jump()
        {
            if(!_groundCheck.isJumpable || _dashController.isDashing || _isJumping)
                return false;
            
            Vector3 velocity = _body.velocity;
            _body.velocity = new Vector3(velocity.x, 0, velocity.z);
            
            _body.AddForce(_transform.up * jumpForce, ForceMode.Impulse);

            _isJumping = true;
            _timeOfJump = Time.time;
            
            _groundCheck.CancelCoyoteTime();

            StartCoroutine(IsJumpingCheck());

            return true;
        }

        public bool CancelJump()
        {
            if (!_isJumping || _body.velocity.y <= 0)
                return false;
            if (_isCanceling)
                return true;

            StartCoroutine(CancelJumpCoroutine());

            return true;
        }

        public void ExitJumpState()
        {
            _isJumping = false;
            _isCanceling = false;
            StopAllCoroutines();
        }

        private IEnumerator CancelJumpCoroutine()
        {
            _isCanceling = true;
            
            float timeSinceJump = Time.time - _timeOfJump;
            if(timeSinceJump < cancelTimeWindow)
                yield return new WaitForSeconds(cancelTimeWindow - timeSinceJump);


            float startVelocity = _body.velocity.y;
            float acceleration = startVelocity / timeToCancelJump;
            acceleration -= Mathf.Abs(_gravityController.CurrentGravity.y);

            if(acceleration <= 0)
            {
                _isCanceling = false;
                yield break;
            }
            
            while (_body.velocity.y > 0 && !_groundCheck.isJumpable)
            {
                _body.AddForce(Vector3.down * acceleration, ForceMode.Acceleration);
                yield return new WaitForFixedUpdate();
            }

            _isCanceling = false;
        }

        private IEnumerator IsJumpingCheck()
        {
            yield return new WaitForFixedUpdate();
            
            while (_groundCheck.isJumpable)
            {
                yield return new WaitForFixedUpdate();
            }

            while (!_groundCheck.isJumpable)
            {
                yield return new WaitForFixedUpdate();
            }

            _isJumping = false;
        }
    }
}
