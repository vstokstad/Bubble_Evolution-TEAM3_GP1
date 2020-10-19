using UnityEngine;

namespace Actor.Component
{
    public class SlopeGravityCounteract : MonoBehaviour
    {
        private Rigidbody _body;
        private GroundCheck _groundCheck;
        private CustomGravityController _gravityController;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _groundCheck = GetComponent<GroundCheck>();
            _gravityController = GetComponent<CustomGravityController>();
        }

        private void FixedUpdate()
        {
            if (_groundCheck.isWalkable)
            {
                Vector3 gravity = _gravityController.CurrentGravity;
                Vector3 gravCross = Vector3.Cross(_groundCheck.groundNormal, gravity);
                gravCross = Vector3.Cross(_groundCheck.groundNormal, gravCross);
                gravCross.Normalize();

                float inclineAngle = Vector3.Angle(-_groundCheck.groundNormal, gravity);
                float gravHForce = gravity.magnitude * Mathf.Sin(inclineAngle * Mathf.Deg2Rad);

                _body.AddForce(gravHForce * gravCross, ForceMode.Acceleration);
            }
        }
    }
}
