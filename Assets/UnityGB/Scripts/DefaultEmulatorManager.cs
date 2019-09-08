using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityGB;

public class DefaultEmulatorManager : MonoBehaviour
{
    [SerializeField]
	private Renderer screenRenderer;
    [SerializeField]
    private DefaultAudioOutput audio;
    [SerializeField]
    private bool useControllerInput;

	public EmulatorBase Emulator
	{
		get;
		private set;
	}

	private Dictionary<KeyCode, EmulatorBase.Button> _keyMapping;

    // Use this for initialization
    void Start()
	{
		// Init Keyboard mapping
        _keyMapping = CreateStandalonePlatformKeyMapping();

		// Load emulator
		IVideoOutput drawable = new DefaultVideoOutput();
        IAudioOutput audioOutput = audio;
		ISaveMemory saveMemory = new DefaultSaveMemory();
		Emulator = new Emulator(drawable, audioOutput, saveMemory);
		screenRenderer.material.mainTexture = ((DefaultVideoOutput) Emulator.Video).Texture;
		audio.GetComponent<AudioSource>().enabled = false;
    }

	void Update()
	{
        // Input
        if (useControllerInput)
        {
#if UNITY_EDITOR
            CheckStandalonePlatformInput();
#else
        CheckOvrPlatformInput();
#endif
        }
    }

    private void CheckStandalonePlatformInput()
    {
        foreach (KeyValuePair<KeyCode, EmulatorBase.Button> entry in _keyMapping)
        {
            if (UnityEngine.Input.GetKeyDown(entry.Key))
                Emulator.SetInput(entry.Value, true);
            else if (UnityEngine.Input.GetKeyUp(entry.Key))
                Emulator.SetInput(entry.Value, false);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            byte[] screenshot = ((DefaultVideoOutput)Emulator.Video).Texture.EncodeToPNG();
            File.WriteAllBytes("./screenshot.png", screenshot);
            Debug.Log("Screenshot saved.");
        }
    }

    private void CheckOvrPlatformInput()
    {
        // Face buttons
        Emulator.SetInput(EmulatorBase.Button.A, OVRInput.Get(OVRInput.Button.One));
        Emulator.SetInput(EmulatorBase.Button.B, OVRInput.Get(OVRInput.Button.Two));
        Emulator.SetInput(EmulatorBase.Button.Select, OVRInput.Get(OVRInput.Button.Three));
        Emulator.SetInput(EmulatorBase.Button.Start, OVRInput.Get(OVRInput.Button.Four));

        // D-Pad
        var thumbStickDirection = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Emulator.SetInput(EmulatorBase.Button.Up, thumbStickDirection.y > 0.25f);
        Emulator.SetInput(EmulatorBase.Button.Down, thumbStickDirection.y < -0.25f);
        Emulator.SetInput(EmulatorBase.Button.Right, thumbStickDirection.x > 0.25f);
        Emulator.SetInput(EmulatorBase.Button.Left, thumbStickDirection.x < -0.25f);
    }

    public void LoadRom(byte[] rom)
    {
        if (rom != null && rom.Length < 1)
        {
            return;
        }

        Emulator.LoadRom(rom);
        StartCoroutine(Run());
    }

	private IEnumerator Run()
	{
		audio.GetComponent<AudioSource>().enabled = true;
		while (true)
		{
			// Run
			Emulator.RunNextStep();

			yield return null;
		}
	}

    private Dictionary<KeyCode, EmulatorBase.Button> CreateStandalonePlatformKeyMapping()
    {
        var keyMapping = new Dictionary<KeyCode, EmulatorBase.Button>();
        keyMapping.Add(KeyCode.UpArrow, EmulatorBase.Button.Up);
        keyMapping.Add(KeyCode.DownArrow, EmulatorBase.Button.Down);
        keyMapping.Add(KeyCode.LeftArrow, EmulatorBase.Button.Left);
        keyMapping.Add(KeyCode.RightArrow, EmulatorBase.Button.Right);
        keyMapping.Add(KeyCode.Z, EmulatorBase.Button.A);
        keyMapping.Add(KeyCode.X, EmulatorBase.Button.B);
        keyMapping.Add(KeyCode.Space, EmulatorBase.Button.Start);
        keyMapping.Add(KeyCode.LeftShift, EmulatorBase.Button.Select);

        return keyMapping;
    }
}
