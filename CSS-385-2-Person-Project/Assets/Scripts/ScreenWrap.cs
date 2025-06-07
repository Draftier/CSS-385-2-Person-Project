using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera mainCamera;
    private float screenLeft, screenRight, screenTop, screenBottom;

    void Start()
    {
        mainCamera = Camera.main;

        // For orthographic camera:
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        screenLeft = mainCamera.transform.position.x - camWidth / 2;
        screenRight = mainCamera.transform.position.x + camWidth / 2;
        screenBottom = mainCamera.transform.position.y - camHeight / 2;
        screenTop = mainCamera.transform.position.y + camHeight / 2;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x < screenLeft)
        {
            pos.x = screenRight;
        }
        else if (pos.x > screenRight)
        {
            pos.x = screenLeft;
        }

        if (pos.y < screenBottom)
        {
            pos.y = screenTop;
        }
        else if (pos.y > screenTop)
        {
            pos.y = screenBottom;
        }

        transform.position = pos;
    }
}
