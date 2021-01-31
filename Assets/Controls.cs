// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Deplacement"",
            ""id"": ""a28fe057-3e62-4751-8c55-9fa668bfdf7a"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1d5e607a-21f9-458b-958e-9cda99221f50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Deplacement"",
                    ""type"": ""Value"",
                    ""id"": ""333d44e5-0b9e-4962-9c96-c5a0292897e4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac656ea5-14d9-46be-9b37-04d9ac08a029"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""df485f6e-bf34-4f1b-9b3f-1532605e7229"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8990a237-dba4-49c0-98d2-801e1ae09ada"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8d9d6f46-4462-462e-a3c1-ffcbe9072c25"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deplacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""NetTest"",
            ""id"": ""404a201e-945c-4225-91e5-d7ae9e098737"",
            ""actions"": [
                {
                    ""name"": ""ChangeScene"",
                    ""type"": ""Button"",
                    ""id"": ""8f2c7179-3cf3-4e41-afbd-d77d0cebeccb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""927d7639-6d6a-4efa-8469-0ef0fa95056b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26ac07b2-36b0-4d6b-974b-01126cf8e28e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""409ac340-ca0d-4f47-822f-54e631b6eda4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Deplacement
        m_Deplacement = asset.FindActionMap("Deplacement", throwIfNotFound: true);
        m_Deplacement_Jump = m_Deplacement.FindAction("Jump", throwIfNotFound: true);
        m_Deplacement_Deplacement = m_Deplacement.FindAction("Deplacement", throwIfNotFound: true);
        // NetTest
        m_NetTest = asset.FindActionMap("NetTest", throwIfNotFound: true);
        m_NetTest_ChangeScene = m_NetTest.FindAction("ChangeScene", throwIfNotFound: true);
        m_NetTest_Jump = m_NetTest.FindAction("Jump", throwIfNotFound: true);
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

    // Deplacement
    private readonly InputActionMap m_Deplacement;
    private IDeplacementActions m_DeplacementActionsCallbackInterface;
    private readonly InputAction m_Deplacement_Jump;
    private readonly InputAction m_Deplacement_Deplacement;
    public struct DeplacementActions
    {
        private @Controls m_Wrapper;
        public DeplacementActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Deplacement_Jump;
        public InputAction @Deplacement => m_Wrapper.m_Deplacement_Deplacement;
        public InputActionMap Get() { return m_Wrapper.m_Deplacement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DeplacementActions set) { return set.Get(); }
        public void SetCallbacks(IDeplacementActions instance)
        {
            if (m_Wrapper.m_DeplacementActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnJump;
                @Deplacement.started -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnDeplacement;
                @Deplacement.performed -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnDeplacement;
                @Deplacement.canceled -= m_Wrapper.m_DeplacementActionsCallbackInterface.OnDeplacement;
            }
            m_Wrapper.m_DeplacementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Deplacement.started += instance.OnDeplacement;
                @Deplacement.performed += instance.OnDeplacement;
                @Deplacement.canceled += instance.OnDeplacement;
            }
        }
    }
    public DeplacementActions @Deplacement => new DeplacementActions(this);

    // NetTest
    private readonly InputActionMap m_NetTest;
    private INetTestActions m_NetTestActionsCallbackInterface;
    private readonly InputAction m_NetTest_ChangeScene;
    private readonly InputAction m_NetTest_Jump;
    public struct NetTestActions
    {
        private @Controls m_Wrapper;
        public NetTestActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeScene => m_Wrapper.m_NetTest_ChangeScene;
        public InputAction @Jump => m_Wrapper.m_NetTest_Jump;
        public InputActionMap Get() { return m_Wrapper.m_NetTest; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NetTestActions set) { return set.Get(); }
        public void SetCallbacks(INetTestActions instance)
        {
            if (m_Wrapper.m_NetTestActionsCallbackInterface != null)
            {
                @ChangeScene.started -= m_Wrapper.m_NetTestActionsCallbackInterface.OnChangeScene;
                @ChangeScene.performed -= m_Wrapper.m_NetTestActionsCallbackInterface.OnChangeScene;
                @ChangeScene.canceled -= m_Wrapper.m_NetTestActionsCallbackInterface.OnChangeScene;
                @Jump.started -= m_Wrapper.m_NetTestActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_NetTestActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_NetTestActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_NetTestActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeScene.started += instance.OnChangeScene;
                @ChangeScene.performed += instance.OnChangeScene;
                @ChangeScene.canceled += instance.OnChangeScene;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public NetTestActions @NetTest => new NetTestActions(this);
    public interface IDeplacementActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnDeplacement(InputAction.CallbackContext context);
    }
    public interface INetTestActions
    {
        void OnChangeScene(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
