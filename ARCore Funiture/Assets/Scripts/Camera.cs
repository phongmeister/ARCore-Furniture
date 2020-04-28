using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    private bool CameraAvailable;
    private WebCamTexture RearCamera;
    private Texture DefaultBackground;

    public RawImage background;
    public AspectRatioFitter Fit;

    // Start is called before the first frame update
    void Start()
    {
        DefaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            CameraAvailable = false;
            return;
        }

        for (int i=0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                RearCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(RearCamera == null)
        {
            Debug.Log("Unable to find rear Camera");
            return;
        }

        RearCamera.Play();
        background.texture = RearCamera;

        CameraAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraAvailable)
        {
            return;
        }

        float ratio = (float)RearCamera.width / (float)RearCamera.height;
        Fit.aspectRatio = ratio;

        float scaleY = RearCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -RearCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}
