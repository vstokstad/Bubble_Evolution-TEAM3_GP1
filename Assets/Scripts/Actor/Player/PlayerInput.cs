using System;
using System.Collections.Generic;
using Actor.Component;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Actor.Player {
    public class PlayerInput : MonoBehaviour {
        [Range(0, 1)] public float inputBufferDuration = 0.2f;

        private GameControls _controls;
        private DashController _dashController;

        private Dictionary<Func<bool>, float> _inputBuffer;

        private Func<bool> _jumpAction;
        private Func<bool> _jumpCancelAction;
        private JumpController _jumpController;

        private MovementController _movementController;

        //  private RotationController _rotationController;
        private WeaponController _weaponController;

        private void Awake(){
            _controls = new GameControls();
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();
            _jumpController = GetComponent<JumpController>();
            //    _rotationController = GetComponent<RotationController>();
            _dashController = GetComponent<DashController>();

            _jumpAction = () => _jumpController.Jump();
            _jumpCancelAction = () => _jumpController.CancelJump();
            _inputBuffer = new Dictionary<Func<bool>, float>();
        }

        private void Update(){
            Func<bool>[] keys = new Func<bool>[_inputBuffer.Count];
            _inputBuffer.Keys.CopyTo(keys, 0);

            foreach (Func<bool> key in keys) {
                _inputBuffer[key] -= Time.deltaTime;

                if (_inputBuffer[key] <= 0 || key.Invoke())
                    _inputBuffer.Remove(key);
            }
        }

        private void OnEnable(){
            _controls.Default.Move.performed += OnMove;
            _controls.Default.Jump.performed += OnJumpPerformed;
            _controls.Default.Jump.canceled += OnJumpCanceled;
            _controls.Default.Dash.performed += OnDash;
            //  _controls.Default.RotateCamera.performed += OnRotate;
            _controls.Default.Shoot.performed += OnShoot;
            _controls.Default.Pause.performed += OnPause;

            _controls.Enable();
        }

        private void OnDisable(){
            _controls.Default.Move.performed -= OnMove;
            _controls.Default.Jump.performed -= OnJumpPerformed;
            _controls.Default.Jump.canceled -= OnJumpCanceled;
            _controls.Default.Dash.performed -= OnDash;
            //  _controls.Default.RotateCamera.performed -= OnRotate;
            _controls.Default.Shoot.performed -= OnShoot;
            _controls.Default.Pause.performed -= OnPause;
            _controls.Disable();
        }

        public void PauseComponents(){
            _controls.Default.Move.performed -= OnMove;
            _controls.Default.Jump.performed -= OnJumpPerformed;
            _controls.Default.Jump.canceled -= OnJumpCanceled;
            _controls.Default.Dash.performed -= OnDash;
            //  _controls.Default.RotateCamera.performed -= OnRotate;
            _controls.Default.Shoot.performed -= OnShoot;
        }

        public void UnpauseComponents(){
            _controls.Default.Move.performed += OnMove;
            _controls.Default.Jump.performed += OnJumpPerformed;
            _controls.Default.Jump.canceled += OnJumpCanceled;
            _controls.Default.Dash.performed += OnDash;
            //  _controls.Default.RotateCamera.performed -= OnRotate;
            _controls.Default.Shoot.performed += OnShoot;
        }

        public Vector2 GetMouseScreenPosition(){
            return _controls.Default.MousePosition.ReadValue<Vector2>();
        }

        private void OnMove(InputAction.CallbackContext context){
            _movementController.MoveVector = context.ReadValue<Vector2>();
        }

        private void OnPause(InputAction.CallbackContext context){
            PauseComponents();
            UIController.instance.PauseGame(this);
        }

        private void OnJumpPerformed(InputAction.CallbackContext context){
            if (!_jumpController.Jump())
                BufferInput(_jumpAction, inputBufferDuration);

            UnBufferInput(_jumpCancelAction);
        }

        private void OnJumpCanceled(InputAction.CallbackContext context){
            if (!_jumpController.CancelJump())
                BufferInput(_jumpCancelAction, inputBufferDuration);
        }

        private void OnDash(InputAction.CallbackContext context){
            Vector2 mousePos = _controls.Default.MousePosition.ReadValue<Vector2>();
            _dashController.Dash(mousePos);

            _jumpController.ExitJumpState();
        }
#if DEBUG
        //uncomment references above to get this working.
        /// private void OnRotate(InputAction.CallbackContext context){
        /// if (context.ReadValue
        /// <float>
        ///     ().Equals(-1f)) _rotationController.RotateCameraLeft();
        ///     if (context.ReadValue
        ///     <float>
        ///         ().Equals(1f)) _rotationController.RotateCameraRight();
        ///         }
#endif
        private void OnShoot(InputAction.CallbackContext context){
            _weaponController.Fire();
        }

        private void BufferInput(Func<bool> inputAction, float duration){
            if (_inputBuffer.ContainsKey(inputAction))
                _inputBuffer[inputAction] = duration;
            else
                _inputBuffer.Add(inputAction, duration);
        }

        private void UnBufferInput(Func<bool> inputAction){
            if (_inputBuffer.ContainsKey(inputAction))
                _inputBuffer.Remove(inputAction);
        }
    }
}