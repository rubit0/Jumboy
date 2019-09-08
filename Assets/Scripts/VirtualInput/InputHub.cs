using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGB;

namespace VirtualInput
{
    public class InputHub : MonoBehaviour
    {
        [Serializable]
        public enum InputMethod
        {
            Keyboard,
            OculusTouch,
            VirtualHands
        }

        [SerializeField]
        private DefaultEmulatorManager emulatorManager;

        [SerializeField]
        private InputMethod standAlonePlatform = InputMethod.Keyboard;
        [SerializeField]
        private InputMethod oculusMobilePlatform = InputMethod.VirtualHands;

        private Dictionary<EmulatorBase.Button, bool> virtualHandButtons = Enum
            .GetValues(typeof(EmulatorBase.Button))
            .Cast<EmulatorBase.Button>()
            .ToDictionary(b => b, b => false);

        private bool emulatorReady;

        private void Awake()
        {
            if (emulatorManager == null)
            {
                emulatorManager = FindObjectOfType<DefaultEmulatorManager>();
            }

            emulatorManager.onEmulatorStarted.AddListener(() => StartCoroutine(CheckInput()));
        }

        public void SetButtonState(EmulatorBase.Button key, bool state)
        {
            virtualHandButtons[key] = state;
        }

        private IEnumerator CheckInput()
        {
            while (gameObject.activeInHierarchy)
            {
                if (Application.isMobilePlatform)
                {
                    switch (oculusMobilePlatform)
                    {
                        case InputMethod.Keyboard:
                            break;
                        case InputMethod.OculusTouch:
                            CheckOvrPlatformInput();
                            break;
                        case InputMethod.VirtualHands:
                            CheckVirtualHandsInput();
                            break;
                    }
                }
                else
                {
                    switch (standAlonePlatform)
                    {
                        case InputMethod.Keyboard:
                            CheckStandalonePlatformInput();
                            break;
                        case InputMethod.OculusTouch:
                            CheckStandalonePlatformInput();
                            break;
                        case InputMethod.VirtualHands:
                            CheckVirtualHandsInput();
                            break;
                    }
                }

                yield return null;
            }
        }

        private void CheckStandalonePlatformInput()
        {
            // Face buttons
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.A, Input.GetKey(KeyCode.Z));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.B, Input.GetKey(KeyCode.X));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Select, Input.GetKey(KeyCode.Space));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Start, Input.GetKey(KeyCode.LeftShift));

            // D-Pad
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Up, Input.GetKey(KeyCode.UpArrow));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Down, Input.GetKey(KeyCode.DownArrow));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Left, Input.GetKey(KeyCode.LeftArrow));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Right, Input.GetKey(KeyCode.RightArrow));
        }

        private void CheckVirtualHandsInput()
        {
            // Face buttons
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.A, virtualHandButtons[EmulatorBase.Button.A]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.B, virtualHandButtons[EmulatorBase.Button.B]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Select, virtualHandButtons[EmulatorBase.Button.Select]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Start, virtualHandButtons[EmulatorBase.Button.Start]);

            // D-Pad
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Up, virtualHandButtons[EmulatorBase.Button.Up]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Right, virtualHandButtons[EmulatorBase.Button.Right]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Down, virtualHandButtons[EmulatorBase.Button.Down]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Left, virtualHandButtons[EmulatorBase.Button.Left]);
        }

        private void CheckOvrPlatformInput()
        {
            // Face buttons
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.A, OVRInput.Get(OVRInput.Button.One));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.B, OVRInput.Get(OVRInput.Button.Two));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Select, OVRInput.Get(OVRInput.Button.Three));
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Start, OVRInput.Get(OVRInput.Button.Four));

            // D-Pad
            var thumbStickDirection = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Up, thumbStickDirection.y > 0.25f);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Down, thumbStickDirection.y < -0.25f);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Left, thumbStickDirection.x < -0.25f);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Right, thumbStickDirection.x > 0.25f);
        }
    }
}
