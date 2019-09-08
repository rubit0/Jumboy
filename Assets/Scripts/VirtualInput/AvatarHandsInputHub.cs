using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGB;

namespace VirtualInput
{
    public class AvatarHandsInputHub : MonoBehaviour
    {
        [SerializeField]
        private DefaultEmulatorManager emulatorManager;
        private Dictionary<EmulatorBase.Button, bool> buttonsState;

        private void Awake()
        {
            buttonsState = Enum
                .GetValues(typeof(EmulatorBase.Button))
                .Cast<EmulatorBase.Button>()
                .ToDictionary(b => b, b => false);
        }

        public bool Get(EmulatorBase.Button key)
        {
            return buttonsState[key];
        }

        public void SetButtonState(EmulatorBase.Button key, bool state)
        {
            buttonsState[key] = state;
        }

        private void Update()
        {
            // Face buttons
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.A, buttonsState[EmulatorBase.Button.A]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.B, buttonsState[EmulatorBase.Button.B]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Select, buttonsState[EmulatorBase.Button.Select]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Start, buttonsState[EmulatorBase.Button.Start]);

            // D-Pad
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Up, buttonsState[EmulatorBase.Button.Up]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Right, buttonsState[EmulatorBase.Button.Right]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Down, buttonsState[EmulatorBase.Button.Down]);
            emulatorManager.Emulator.SetInput(EmulatorBase.Button.Left, buttonsState[EmulatorBase.Button.Left]);
        }
    }
}
