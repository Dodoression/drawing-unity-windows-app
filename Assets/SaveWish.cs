using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System.Collections;
using System;

public class SaveWish : MonoBehaviour
{
    public RenderTexture rt;
    public Camera renderCam;
    public DrawMeshFull drawMeshScript;

    string path = "";

    private void Start()
    {
        StartCoroutine(AskPlayerForFolder());
    }

    IEnumerator AskPlayerForFolder()
    {
        FileBrowser.SetFilters(false, ".png");
        FileBrowser.SetDefaultFilter(".png");

        // Show the folder selection dialog
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Folders ,false, null, "Choose Folder", "Select");

        // Check if user picked a folder
        if (FileBrowser.Success)
        {
            path = FileBrowser.Result[0]; // This will be the full path to the selected folder
            Debug.Log("User selected folder: " + path);
        }
        else
        {
            Debug.LogWarning("User cancelled folder selection.");
        }
    }

    public void SavePNG()
    {
        Texture2D fullScreenshot = ScreenCapture.CaptureScreenshotAsTexture();

        int screenW = fullScreenshot.width;
        int screenH = fullScreenshot.height;

        int cropW = Mathf.Min(900 + 450, screenW);
        int cropH = Mathf.Min(520 + 260, screenH);

        int startX = Mathf.Max(0, (screenW - cropW + 5) / 2);
        int startY = Mathf.Max(0, (screenH - cropH - 50) / 2);

        Debug.Log($"Cropping: startX={startX}, startY={startY}, cropW={cropW}, cropH={cropH}");

        if (startX + cropW > screenW) startX = screenW - cropW;
        if (startY + cropH > screenH) startY = screenH - cropH;

        Debug.Log($"Cropping from: X:{startX}, Y:{startY}, Size: {cropW}x{cropH} of {screenW}x{screenH}");

        Color[] pixels = fullScreenshot.GetPixels(startX, startY, cropW, cropH);
        Texture2D cropped = new Texture2D(cropW, cropH, TextureFormat.RGB24, false);
        cropped.SetPixels(pixels);
        cropped.Apply();

        string timestamp = DateTime.Now.ToString("mmssfff"); // minute, second, millisecond
        string pngName = $"wish_{timestamp}.png";

        string filePath = Path.Combine(path, pngName);
        Debug.Log(filePath);

        File.WriteAllBytes(filePath, cropped.EncodeToPNG());

        drawMeshScript.ClearAllLines();
        Destroy(fullScreenshot);
        Destroy(cropped);
    }
}
