using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private ExpansionButton expansion;
    [SerializeField] private CreateGenerator createGen;

    Vector3 touchStart;
    private float zoomOutMin = 5;
    private float zoomOutMax;

    [SerializeField] private GameManager manager;

    [SerializeField] private SpriteRenderer cameraZone;

    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        /*
        minX = cameraZone.transform.position.x - cameraZone.bounds.size.x / 2f;
        maxX = cameraZone.transform.position.x + cameraZone.bounds.size.x / 2f;

        minY = cameraZone.transform.position.y - cameraZone.bounds.size.y / 2f;
        maxY = cameraZone.transform.position.y + cameraZone.bounds.size.y / 2f; */

        Debug.Log("aspect = " + Camera.main.aspect);

        if (expansion.currentExpansion == 1) // || GameManager.Instance.roomsMade == 2
            Camera.main.transform.position = new Vector3(-0.69f, 1, -10);
        else if (expansion.currentExpansion == 0)
            Camera.main.transform.position = new Vector3(-3.96000004f, 1.23000002f, -10);
    }

    void Update()
    {
        if (manager.cameraMovement == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position = ClampCamera(Camera.main.transform.position) + direction;
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);

        Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float minX_ = minX + camWidth;
        float maxX_ = maxX - camWidth;
        float minY_ = minY + camHeight;
        float maxY_ = maxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX_, maxX_);
        float newY = Mathf.Clamp(targetPosition.y, minY_, maxY_);

        return new Vector3(newX, newY, targetPosition.z);
    }

    private string CheckDevice()
    {
        if (Camera.main.aspect > 0.6f)
        {
            return "Tablet";
        }
        else if (Camera.main.aspect > 0.5f)
        {
            return "iPhone 7";
        }
        else
        {
            return "iPhone 14";
        }
    }

    private void SetCameraZone(float zoomOutMax_, Vector3 position, Vector3 scale)
    {
        zoomOutMax = zoomOutMax_;

        cameraZone.transform.position = position;
        cameraZone.transform.localScale = scale;

        minX = cameraZone.transform.position.x - cameraZone.bounds.size.x / 2f;
        maxX = cameraZone.transform.position.x + cameraZone.bounds.size.x / 2f;

        minY = cameraZone.transform.position.y - cameraZone.bounds.size.y / 2f;
        maxY = cameraZone.transform.position.y + cameraZone.bounds.size.y / 2f;
    }

    public void ExpansionZoneChange()
    {
        if (expansion.currentExpansion == 0)
        {
            if (CheckDevice() == "Tablet")
                SetCameraZone(6.64f, new Vector3(-0.716099977f, 1.36010003f, 0), new Vector3(9.9795332f, 13.346755f, 1));
            else if (CheckDevice() == "iPhone 7")
                SetCameraZone(6.8f, new Vector3(-0.681400001f, 1.01680005f, 0), new Vector3(8.92092705f, 13.7337008f, 1));
            else if (CheckDevice() == "iPhone 14")
                SetCameraZone(6.8f, new Vector3(-0.681400001f, 1.01680005f, 0), new Vector3(8.92092705f, 13.7337008f, 1));
        }
        if (expansion.currentExpansion == 1 || GameManager.Instance.generatorsActive == 2)
        {
            if (CheckDevice() == "Tablet")
                SetCameraZone(8.23f, new Vector3(-3.95619988f, 1.22000003f, 0), new Vector3(12.3845119f, 16.5958271f, 1));
            else if (CheckDevice() == "iPhone 7")
                SetCameraZone(10.98f, new Vector3(-3.95190001f, 1.22539997f, 0), new Vector3(12.3931808f, 22.0627804f, 1));
            else if (CheckDevice() == "iPhone 14")
                SetCameraZone(13.41f, new Vector3(-3.95449996f, 1.25349998f, 0), new Vector3(12.3931808f, 26.9518032f, 1));
        }
    }
}