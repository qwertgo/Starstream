//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Scripts/Input/TrackerInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TrackerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TrackerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TrackerInputAction"",
    ""maps"": [
        {
            ""name"": ""ViveTracker"",
            ""id"": ""c9ca3ce8-3929-4b69-b0f5-701b1b6c4296"",
            ""actions"": [
                {
                    ""name"": ""Position_1"",
                    ""type"": ""Value"",
                    ""id"": ""7cb4ab92-6eb5-4747-b02a-faae5f89bf25"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotation_1"",
                    ""type"": ""Value"",
                    ""id"": ""9f5df3db-dd12-4b68-a2ae-29a74d4d86cf"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Position_2"",
                    ""type"": ""Value"",
                    ""id"": ""2f659638-0c19-458d-b5f4-9655671844bf"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotation_2"",
                    ""type"": ""Value"",
                    ""id"": ""81c9b2aa-e42a-4eb8-950a-2c62b2567b00"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""GamepadPosition"",
                    ""type"": ""Value"",
                    ""id"": ""f5f698be-2c76-4c71-9650-e6d6cf0af6e9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""826b8d67-d48c-4c27-8901-406134ff5e5e"",
                    ""path"": ""<XRViveTracker>{Waist}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6c84b45-9e5d-4a59-bf40-381933d2d23c"",
                    ""path"": ""<XRViveTracker>{Waist}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62a66c83-1b57-4df4-96f6-e31eb271b5f1"",
                    ""path"": ""<XRViveTracker>{Chest}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1eb7537-e0ee-48d2-aa86-bf0513f4a7ec"",
                    ""path"": ""<XRViveTracker>{Chest}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3facc05c-6607-4850-af33-ff0cdf671725"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""216cb205-60aa-41b6-a255-beef335a644e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""8c016a72-e18b-4d26-b130-6b3c0ba7c819"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c9f1ae99-38bf-4ac8-a343-301b97b113a1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""37156ad4-09ae-4cbb-b6f6-8c1050fe2186"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a3e2cac4-ef96-4dfb-85c9-a168b47167ab"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f3f73b48-8112-4caf-9507-8aa736dbfcad"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""651dd470-7835-400d-aa81-37c1f97fe870"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""550f6ac4-606d-463e-8b48-1b1e137d6ec8"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8a24df63-1294-448f-a506-188fda92c0a6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2e8df9a5-0cbf-4c68-9329-8eec19bd5cc1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2d6e04ee-282c-4668-bf65-4dc09364d886"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GamepadPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ViveTracker
        m_ViveTracker = asset.FindActionMap("ViveTracker", throwIfNotFound: true);
        m_ViveTracker_Position_1 = m_ViveTracker.FindAction("Position_1", throwIfNotFound: true);
        m_ViveTracker_Rotation_1 = m_ViveTracker.FindAction("Rotation_1", throwIfNotFound: true);
        m_ViveTracker_Position_2 = m_ViveTracker.FindAction("Position_2", throwIfNotFound: true);
        m_ViveTracker_Rotation_2 = m_ViveTracker.FindAction("Rotation_2", throwIfNotFound: true);
        m_ViveTracker_GamepadPosition = m_ViveTracker.FindAction("GamepadPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // ViveTracker
    private readonly InputActionMap m_ViveTracker;
    private List<IViveTrackerActions> m_ViveTrackerActionsCallbackInterfaces = new List<IViveTrackerActions>();
    private readonly InputAction m_ViveTracker_Position_1;
    private readonly InputAction m_ViveTracker_Rotation_1;
    private readonly InputAction m_ViveTracker_Position_2;
    private readonly InputAction m_ViveTracker_Rotation_2;
    private readonly InputAction m_ViveTracker_GamepadPosition;
    public struct ViveTrackerActions
    {
        private @TrackerInputAction m_Wrapper;
        public ViveTrackerActions(@TrackerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position_1 => m_Wrapper.m_ViveTracker_Position_1;
        public InputAction @Rotation_1 => m_Wrapper.m_ViveTracker_Rotation_1;
        public InputAction @Position_2 => m_Wrapper.m_ViveTracker_Position_2;
        public InputAction @Rotation_2 => m_Wrapper.m_ViveTracker_Rotation_2;
        public InputAction @GamepadPosition => m_Wrapper.m_ViveTracker_GamepadPosition;
        public InputActionMap Get() { return m_Wrapper.m_ViveTracker; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ViveTrackerActions set) { return set.Get(); }
        public void AddCallbacks(IViveTrackerActions instance)
        {
            if (instance == null || m_Wrapper.m_ViveTrackerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ViveTrackerActionsCallbackInterfaces.Add(instance);
            @Position_1.started += instance.OnPosition_1;
            @Position_1.performed += instance.OnPosition_1;
            @Position_1.canceled += instance.OnPosition_1;
            @Rotation_1.started += instance.OnRotation_1;
            @Rotation_1.performed += instance.OnRotation_1;
            @Rotation_1.canceled += instance.OnRotation_1;
            @Position_2.started += instance.OnPosition_2;
            @Position_2.performed += instance.OnPosition_2;
            @Position_2.canceled += instance.OnPosition_2;
            @Rotation_2.started += instance.OnRotation_2;
            @Rotation_2.performed += instance.OnRotation_2;
            @Rotation_2.canceled += instance.OnRotation_2;
            @GamepadPosition.started += instance.OnGamepadPosition;
            @GamepadPosition.performed += instance.OnGamepadPosition;
            @GamepadPosition.canceled += instance.OnGamepadPosition;
        }

        private void UnregisterCallbacks(IViveTrackerActions instance)
        {
            @Position_1.started -= instance.OnPosition_1;
            @Position_1.performed -= instance.OnPosition_1;
            @Position_1.canceled -= instance.OnPosition_1;
            @Rotation_1.started -= instance.OnRotation_1;
            @Rotation_1.performed -= instance.OnRotation_1;
            @Rotation_1.canceled -= instance.OnRotation_1;
            @Position_2.started -= instance.OnPosition_2;
            @Position_2.performed -= instance.OnPosition_2;
            @Position_2.canceled -= instance.OnPosition_2;
            @Rotation_2.started -= instance.OnRotation_2;
            @Rotation_2.performed -= instance.OnRotation_2;
            @Rotation_2.canceled -= instance.OnRotation_2;
            @GamepadPosition.started -= instance.OnGamepadPosition;
            @GamepadPosition.performed -= instance.OnGamepadPosition;
            @GamepadPosition.canceled -= instance.OnGamepadPosition;
        }

        public void RemoveCallbacks(IViveTrackerActions instance)
        {
            if (m_Wrapper.m_ViveTrackerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IViveTrackerActions instance)
        {
            foreach (var item in m_Wrapper.m_ViveTrackerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ViveTrackerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ViveTrackerActions @ViveTracker => new ViveTrackerActions(this);
    public interface IViveTrackerActions
    {
        void OnPosition_1(InputAction.CallbackContext context);
        void OnRotation_1(InputAction.CallbackContext context);
        void OnPosition_2(InputAction.CallbackContext context);
        void OnRotation_2(InputAction.CallbackContext context);
        void OnGamepadPosition(InputAction.CallbackContext context);
    }
}
