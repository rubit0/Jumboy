using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityGB;

public class DefaultEmulatorManager : MonoBehaviour
{
    public bool EmulatorStarted { get; private set; }

    [SerializeField]
	private Renderer screenRenderer;
    [SerializeField]
    private DefaultAudioOutput audio;

    public UnityEvent onEmulatorStarted;


    public EmulatorBase Emulator
	{
		get;
		private set;
	}

    private void Awake()
	{
		// Init
		IVideoOutput drawable = new DefaultVideoOutput();
        IAudioOutput audioOutput = audio;
		ISaveMemory saveMemory = new DefaultSaveMemory();
		Emulator = new Emulator(drawable, audioOutput, saveMemory);
		screenRenderer.material.mainTexture = ((DefaultVideoOutput) Emulator.Video).Texture;
		audio.GetComponent<AudioSource>().enabled = false;
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
        EmulatorStarted = true;
        onEmulatorStarted?.Invoke();
		while (gameObject.activeInHierarchy)
		{
			// Run
			Emulator.RunNextStep();

			yield return null;
		}
	}
}
