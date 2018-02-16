using UnityEngine;
using System.Collections;
using System.IO;

public class DemoScreen : MonoBehaviour {

    public GameObject rigCamera;

    public GameObject overlay;
    public GameObject openButton;

    private bool shouldTakeScreenshot;

    public virtual void Start()
    {
        if (PlayerPrefs.GetInt("isCamMenuOpen") == 1)
        {
            OpenPressed();
        } else
        {
            ClosePressed();
        }

        if (PlayerPrefs.GetInt("isRigCam") == 1)
        {
            RigCameraSwitch();
        } else
        {
            HeadCameraSwitch();
        }
    }

    public virtual void OpenPressed()
    {
        overlay.SetActive(true);
        openButton.SetActive(false);
        PlayerPrefs.SetInt("isCamMenuOpen", 1);
    }

    public virtual void HeadCameraSwitch()
    {
        rigCamera.SetActive(false);
        PlayerPrefs.SetInt("isRigCam", 0);
    }

    public virtual void RigCameraSwitch()
    {
        rigCamera.SetActive(true);
        PlayerPrefs.SetInt("isRigCam", 1);
    }

    public virtual void ClosePressed()
    {
        overlay.SetActive(false);
        openButton.SetActive(true);
        PlayerPrefs.SetInt("isCamMenuOpen", 0);
    }

    public virtual void ScreenshotPressed()
    {
        shouldTakeScreenshot = true;
    }

    void LateUpdate()
    {
        if (shouldTakeScreenshot)
        {
            Camera camera;

            if (rigCamera.activeInHierarchy)
            {
                camera = rigCamera.GetComponent<Camera>();
            }
            else
            {
                camera = rigCamera.transform.parent.transform.parent.Find("Camera (eye)").GetComponent<Camera>();
            }

            takeScreenshot(camera);

            shouldTakeScreenshot = false;
        }

    }

    // maybe should be a function of game manager?
    public static void takeScreenshot(Camera camera)
    {
        string time = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string screenshotName = "ScreenshotAt" + time + ".png";

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotName, bytes);
    }

}
