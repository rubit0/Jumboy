using System.Collections;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    private DefaultEmulatorManager emulatorManager;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return null;
        var files = RomLoader.Discover();
        var rom = RomLoader.Load(files[0]);
        emulatorManager.LoadRom(rom);
    }
}
