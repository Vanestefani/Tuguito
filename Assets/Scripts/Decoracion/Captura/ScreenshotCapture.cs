using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenshotCapture : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private bool incluirUI = true;
    [SerializeField] private bool guardarEnPersistent = true;

    public void CaptureScreenshot()
    {
        if (!incluirUI && uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(false);
        }

        // Usar método simple que guarda directo
        string filename = $"screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string folderPath = guardarEnPersistent
            ? Application.persistentDataPath
            : Application.streamingAssetsPath;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fullPath = Path.Combine(folderPath, filename);
        ScreenCapture.CaptureScreenshot(fullPath);

        Debug.Log($"Screenshot guardado en: {fullPath}");

        if (!incluirUI && uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(true);
        }
    }
}