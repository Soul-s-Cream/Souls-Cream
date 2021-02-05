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
        },
        {
            ""name"": ""Cri"",
            ""id"": ""15a7e9cd-70a7-432e-9396-4fe88100bc35"",
            ""actions"": [
                {
                    ""name"": ""CriUp"",
                    ""type"": ""Button"",
                    ""id"": ""25e5b478-f057-45e2-a7c5-c9f586b6b4b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CriDown"",
                    ""type"": ""Button"",
                    ""id"": ""bc6107db-c0c5-4fa5-a5e5-909ca7f81f5f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cri"",
                    ""type"": ""Button"",
                    ""id"": ""81257850-969f-4c59-a415-bf9010c53253"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2d9c271e-06ee-4772-b4d6-bb426347d34b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CriUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a67b8866-a457-4599-bfb9-464fedb4fa3a"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CriDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""489dee20-827c-407a-9ee1-aa43fb51b41a"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cri"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DEBUG"",
            ""id"": ""634ab154-cd4c-4002-abe5-f8ab7563633f"",
            ""actions"": [
                {
                    ""name"": ""JumpJ2"",
                    ""type"": ""Button"",
                    ""id"": ""b680ddc0-9cea-4409-8c4c-ecb33eed19e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DeplacementJ2"",
                    ""type"": ""Button"",
                    ""id"": ""d641c3aa-e723-4f78-88f9-d16b54e9a25a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cri"",
                    ""type"": ""Button"",
                    ""id"": ""78217ffe-744d-4d57-bbfe-d4f0b276b886"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CriUp"",
                    ""type"": ""Button"",
                    ""id"": ""33702425-ae22-489d-ac73-f77cde5b7ffa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CriDown"",
                    ""type"": ""Button"",
                    ""id"": ""14bd49af-e9ab-4e37-b857-7962b03a7691"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ee7e78a-6a59-4182-afc0-e566c02dd46b"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpJ2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""3b25210e-15d9-49a9-9d65-2c98df305d44"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DeplacementJ2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c91cd003-f6e8-4462-8f81-f48aacc500f3"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DeplacementJ2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2e2e5840-1671-490a-96d6-6b317b2cdde0"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DeplacementJ2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""20f5b5c9-8af9-4441-9d44-aaff9975e316"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cri"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdbf81ce-acfa-43c7-aec8-79634bb49d80"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CriUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e3dcd82-9c36-472b-a2c2-073143cfbafd"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CriDown"",
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
        // Cri
        m_Cri = asset.FindActionMap("Cri", throwIfNotFound: true);
        m_Cri_CriUp = m_Cri.FindAction("CriUp", throwIfNotFound: true);
        m_Cri_CriDown = m_Cri.FindAction("CriDown", throwIfNotFound: true);
        m_Cri_Cri = m_Cri.FindAction("Cri", throwIfNotFound: true);
        // DEBUG
        m_DEBUG = asset.FindActionMap("DEBUG", throwIfNotFound: true);
        m_DEBUG_JumpJ2 = m_DEBUG.FindAction("JumpJ2", throwIfNotFound: true);
        m_DEBUG_DeplacementJ2 = m_DEBUG.FindAction("DeplacementJ2", throwIfNotFound: true);
        m_DEBUG_Cri = m_DEBUG.FindAction("Cri", throwIfNotFound: true);
        m_DEBUG_CriUp = m_DEBUG.FindAction("CriUp", throwIfNotFound: true);
        m_DEBUG_CriDown = m_DEBUG.FindAction("CriDown", throwIfNotFound: true);
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

    // Cri
    private readonly InputActionMap m_Cri;
    private ICriActions m_CriActionsCallbackInterface;
    private readonly InputAction m_Cri_CriUp;
    private readonly InputAction m_Cri_CriDown;
    private readonly InputAction m_Cri_Cri;
    public struct CriActions
    {
        private @Controls m_Wrapper;
        public CriActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CriUp => m_Wrapper.m_Cri_CriUp;
        public InputAction @CriDown => m_Wrapper.m_Cri_CriDown;
        public InputAction @Cri => m_Wrapper.m_Cri_Cri;
        public InputActionMap Get() { return m_Wrapper.m_Cri; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CriActions set) { return set.Get(); }
        public void SetCallbacks(ICriActions instance)
        {
            if (m_Wrapper.m_CriActionsCallbackInterface != null)
            {
                @CriUp.started -= m_Wrapper.m_CriActionsCallbackInterface.OnCriUp;
                @CriUp.performed -= m_Wrapper.m_CriActionsCallbackInterface.OnCriUp;
                @CriUp.canceled -= m_Wrapper.m_CriActionsCallbackInterface.OnCriUp;
                @CriDown.started -= m_Wrapper.m_CriActionsCallbackInterface.OnCriDown;
                @CriDown.performed -= m_Wrapper.m_CriActionsCallbackInterface.OnCriDown;
                @CriDown.canceled -= m_Wrapper.m_CriActionsCallbackInterface.OnCriDown;
                @Cri.started -= m_Wrapper.m_CriActionsCallbackInterface.OnCri;
                @Cri.performed -= m_Wrapper.m_CriActionsCallbackInterface.OnCri;
                @Cri.canceled -= m_Wrapper.m_CriActionsCallbackInterface.OnCri;
            }
            m_Wrapper.m_CriActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CriUp.started += instance.OnCriUp;
                @CriUp.performed += instance.OnCriUp;
                @CriUp.canceled += instance.OnCriUp;
                @CriDown.started += instance.OnCriDown;
                @CriDown.performed += instance.OnCriDown;
                @CriDown.canceled += instance.OnCriDown;
                @Cri.started += instance.OnCri;
                @Cri.performed += instance.OnCri;
                @Cri.canceled += instance.OnCri;
            }
        }
    }
    public CriActions @Cri => new CriActions(this);

    // DEBUG
    private readonly InputActionMap m_DEBUG;
    private IDEBUGActions m_DEBUGActionsCallbackInterface;
    private readonly InputAction m_DEBUG_JumpJ2;
    private readonly InputAction m_DEBUG_DeplacementJ2;
    private readonly InputAction m_DEBUG_Cri;
    private readonly InputAction m_DEBUG_CriUp;
    private readonly InputAction m_DEBUG_CriDown;
    public struct DEBUGActions
    {
        private @Controls m_Wrapper;
        public DEBUGActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @JumpJ2 => m_Wrapper.m_DEBUG_JumpJ2;
        public InputAction @DeplacementJ2 => m_Wrapper.m_DEBUG_DeplacementJ2;
        public InputAction @Cri => m_Wrapper.m_DEBUG_Cri;
        public InputAction @CriUp => m_Wrapper.m_DEBUG_CriUp;
        public InputAction @CriDown => m_Wrapper.m_DEBUG_CriDown;
        public InputActionMap Get() { return m_Wrapper.m_DEBUG; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DEBUGActions set) { return set.Get(); }
        public void SetCallbacks(IDEBUGActions instance)
        {
            if (m_Wrapper.m_DEBUGActionsCallbackInterface != null)
            {
                @JumpJ2.started -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnJumpJ2;
                @JumpJ2.performed -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnJumpJ2;
                @JumpJ2.canceled -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnJumpJ2;
                @DeplacementJ2.started -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnDeplacementJ2;
                @DeplacementJ2.performed -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnDeplacementJ2;
                @DeplacementJ2.canceled -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnDeplacementJ2;
                @Cri.started -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCri;
                @Cri.performed -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCri;
                @Cri.canceled -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCri;
                @CriUp.started -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriUp;
                @CriUp.performed -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriUp;
                @CriUp.canceled -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriUp;
                @CriDown.started -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriDown;
                @CriDown.performed -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriDown;
                @CriDown.canceled -= m_Wrapper.m_DEBUGActionsCallbackInterface.OnCriDown;
            }
            m_Wrapper.m_DEBUGActionsCallbackInterface = instance;
            if (instance != null)
            {
                @JumpJ2.started += instance.OnJumpJ2;
                @JumpJ2.performed += instance.OnJumpJ2;
                @JumpJ2.canceled += instance.OnJumpJ2;
                @DeplacementJ2.started += instance.OnDeplacementJ2;
                @DeplacementJ2.performed += instance.OnDeplacementJ2;
                @DeplacementJ2.canceled += instance.OnDeplacementJ2;
                @Cri.started += instance.OnCri;
                @Cri.performed += instance.OnCri;
                @Cri.canceled += instance.OnCri;
                @CriUp.started += instance.OnCriUp;
                @CriUp.performed += instance.OnCriUp;
                @CriUp.canceled += instance.OnCriUp;
                @CriDown.started += instance.OnCriDown;
                @CriDown.performed += instance.OnCriDown;
                @CriDown.canceled += instance.OnCriDown;
            }
        }
    }
    public DEBUGActions @DEBUG => new DEBUGActions(this);
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
    public interface ICriActions
    {
        void OnCriUp(InputAction.CallbackContext context);
        void OnCriDown(InputAction.CallbackContext context);
        void OnCri(InputAction.CallbackContext context);
    }
    public interface IDEBUGActions
    {
        void OnJumpJ2(InputAction.CallbackContext context);
        void OnDeplacementJ2(InputAction.CallbackContext context);
        void OnCri(InputAction.CallbackContext context);
        void OnCriUp(InputAction.CallbackContext context);
        void OnCriDown(InputAction.CallbackContext context);
    }
}
