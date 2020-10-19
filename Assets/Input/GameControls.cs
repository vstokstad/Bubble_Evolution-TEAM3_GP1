// GENERATED AUTOMATICALLY FROM 'Assets/Input/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Object = UnityEngine.Object;

public class GameControls : IInputActionCollection, IDisposable {
    // Default
    private readonly InputActionMap m_Default;
    private readonly InputAction m_Default_Dash;
    private readonly InputAction m_Default_Jump;
    private readonly InputAction m_Default_MousePosition;
    private readonly InputAction m_Default_Move;
    private readonly InputAction m_Default_Pause;
    private readonly InputAction m_Default_Shoot;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private int m_KeyboardAndMouseSchemeIndex = -1;
    private int m_KeyboardOnlySchemeIndex = -1;

    public GameControls(){
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""524b06a7-9c61-4c8a-a538-dd3a0064796b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4f348658-93fe-4441-bcd3-8b71413c96ea"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""daf2a6b3-cd50-4937-807b-9275a469eb08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""a3fba9a6-1b2a-4e6a-b201-b30260d5e5b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""7ec8ac15-c903-4c73-8684-96202001a949"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""464bc01d-8c41-4997-ae09-9b9a5a3f0a99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""95b64281-0ba5-432c-8c14-ecb509a715f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""66ac24d6-b918-4364-af67-4285f210e757"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a291eee8-1937-4d96-97ba-3a6bff2a6dc3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""32847dd4-ef03-4970-bd33-e1db35449e59"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""95b64281-0ba5-432c-8c14-ecb509a715f4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""603ee286-8fe9-47f0-898d-5fab549ed078"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""427c1c40-7301-440b-bc5b-a58b2cf20fcf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cc9d1e28-2844-428d-93a9-da1307d40b7a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOnly"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6d6b9901-789a-4edf-828f-45f5387b2cf7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOnly"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f2cd36d7-16a4-42cc-a082-2b214768717c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse;KeyboardOnly"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abb22473-e8c1-4f1b-b494-350c2ec19cc0"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse;KeyboardOnly"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3193076f-cd8a-4526-8bc4-1114512995b5"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c66337a-36ef-42c5-b5e8-14fe356f2435"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37f1d946-1c8e-4540-8447-49fe51c3852a"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOnly;KeyboardAndMouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eadd0d3f-f42a-4eaf-a0e5-1e6410d5a267"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardOnly"",
            ""bindingGroup"": ""KeyboardOnly"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Default
        m_Default = asset.FindActionMap("Default", true);
        m_Default_Move = m_Default.FindAction("Move", true);
        m_Default_Jump = m_Default.FindAction("Jump", true);
        m_Default_Dash = m_Default.FindAction("Dash", true);
        m_Default_MousePosition = m_Default.FindAction("MousePosition", true);
        m_Default_Shoot = m_Default.FindAction("Shoot", true);
        m_Default_Pause = m_Default.FindAction("Pause", true);
    }

    public InputActionAsset asset { get; }
    public DefaultActions Default => new DefaultActions(this);

    public InputControlScheme KeyboardOnlyScheme {
        get {
            if (m_KeyboardOnlySchemeIndex == -1)
                m_KeyboardOnlySchemeIndex = asset.FindControlSchemeIndex("KeyboardOnly");
            return asset.controlSchemes[m_KeyboardOnlySchemeIndex];
        }
    }

    public InputControlScheme KeyboardAndMouseScheme {
        get {
            if (m_KeyboardAndMouseSchemeIndex == -1)
                m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }

    public void Dispose(){
        Object.Destroy(asset);
    }

    public InputBinding? bindingMask {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action){
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator(){
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator(){
        return GetEnumerator();
    }

    public void Enable(){
        asset.Enable();
    }

    public void Disable(){
        asset.Disable();
    }

    public struct DefaultActions {
        private readonly GameControls m_Wrapper;

        public DefaultActions(GameControls wrapper){
            m_Wrapper = wrapper;
        }

        public InputAction Move => m_Wrapper.m_Default_Move;
        public InputAction Jump => m_Wrapper.m_Default_Jump;
        public InputAction Dash => m_Wrapper.m_Default_Dash;
        public InputAction MousePosition => m_Wrapper.m_Default_MousePosition;
        public InputAction Shoot => m_Wrapper.m_Default_Shoot;
        public InputAction Pause => m_Wrapper.m_Default_Pause;

        public InputActionMap Get(){
            return m_Wrapper.m_Default;
        }

        public void Enable(){
            Get().Enable();
        }

        public void Disable(){
            Get().Disable();
        }

        public bool enabled => Get().enabled;

        public static implicit operator InputActionMap(DefaultActions set){
            return set.Get();
        }

        public void SetCallbacks(IDefaultActions instance){
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null) {
                Move.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                Jump.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnJump;
                Dash.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDash;
                Dash.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDash;
                Dash.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDash;
                MousePosition.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMousePosition;
                MousePosition.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMousePosition;
                MousePosition.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMousePosition;
                Shoot.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnShoot;
                Shoot.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnShoot;
                Shoot.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnShoot;
                Pause.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
                Pause.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
                Pause.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnPause;
            }

            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null) {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Dash.started += instance.OnDash;
                Dash.performed += instance.OnDash;
                Dash.canceled += instance.OnDash;
                MousePosition.started += instance.OnMousePosition;
                MousePosition.performed += instance.OnMousePosition;
                MousePosition.canceled += instance.OnMousePosition;
                Shoot.started += instance.OnShoot;
                Shoot.performed += instance.OnShoot;
                Shoot.canceled += instance.OnShoot;
                Pause.started += instance.OnPause;
                Pause.performed += instance.OnPause;
                Pause.canceled += instance.OnPause;
            }
        }
    }

    public interface IDefaultActions {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}