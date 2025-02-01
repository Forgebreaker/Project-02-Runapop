using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatio : MonoBehaviour
{
    private float minAspectRatio = 18f / 9f;
    private float maxAspectRatio = 18f / 9f; 
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        SetCameraAspect();
    }
    void OnPreRender()
    {
        GL.Clear(true, true, Color.black);
    }
    void Update()
    {
        SetCameraAspect();
        GL.Clear(true, true, Color.black);
    }

    void SetCameraAspect()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float targetAspectRatio = Mathf.Lerp(minAspectRatio, maxAspectRatio, (windowAspect - minAspectRatio) / (maxAspectRatio - minAspectRatio));
        AdjustCameraViewport(targetAspectRatio);
    }

    void AdjustCameraViewport(float targetAspectRatio)
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspectRatio;

        if (scaleHeight < 1.0f)
        {
            Rect rect = _camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            _camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = _camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            _camera.rect = rect;
        }
    }

}
