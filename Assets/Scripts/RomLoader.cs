using System.IO;
using UnityEngine;

public static class RomLoader
{
    public static string[] Discover()
    {
        var path = $"{Application.persistentDataPath}/Roms";
        Debug.Log(path);
        var files = Directory.GetFiles(path, "*.gb");

        return files;
    }

    public static byte[] Load(string path)
    {
        try
        {
            return File.ReadAllBytes(path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("File couldn't be found.");

            Debug.LogException(ex);
            throw ex;
        }
    }
}
