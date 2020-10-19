using UnityEngine;

namespace Actor.Component
{
    public class CustomGravityController : MonoBehaviour
    {
        [Tooltip("How much gravity is applied when the character is on a walkable surface.")]
        [Range(0, 50)] public float groundedGravity = 9.81f;
        [Tooltip("How much gravity is applied when characters is moving upwards in a jump.")]
        [Range(0, 50)] public float jumpingGravity = 5f;
        [Tooltip("How much gravity is applied when the character is falling.")]
        [Range(0, 50)] public float fallingGravity = 15f;
        [Tooltip("The max downwards speed the player can reach when falling.")]
        [Range(0, 50)] public float maxFallSpeed = 20f;

        private GroundCheck _groundCheck;
        private Rigidbody _body;
        
        public Vector3 CurrentGravity { get; private set; }
        
        private void Awake()
        {
            _groundCheck = GetComponent<GroundCheck>();
            _body = GetComponent<Rigidbody>();

            _body.useGravity = false;
        }

        private void FixedUpdate()
        {
            Vector3 velocity = _body.velocity;
            float maxVelocityChange;
            
            if (_groundCheck.isJumpable || _groundCheck.isWalkable)
            {
                maxVelocityChange = groundedGravity * Time.deltaTime;
            }
            else if (_body.velocity.y > 0)
            {
                maxVelocityChange = jumpingGravity * Time.deltaTime;
            }
            else
            {
                maxVelocityChange = fallingGravity * Time.deltaTime;
            }

            velocity.y = Mathf.MoveTowards(velocity.y, _groundCheck.isWalkable ? Mathf.NegativeInfinity : -maxFallSpeed,
                maxVelocityChange);

            _body.velocity = velocity;
        }
    }
}
