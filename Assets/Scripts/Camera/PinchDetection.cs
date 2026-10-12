using System.Collections;
using UnityEngine; 

public class PinchDetection : MonoBehaviour
{

    private TouchInput controls;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        controls = new TouchInput();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
        controls.Touch.PrimaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());

            // Detection
            if (distance > previousDistance) // Zooming out
            {
                Camera.main.orthographicSize -= 0.4f; 
            }
            else if (distance < previousDistance) // Zooming in
            {
                Camera.main.orthographicSize += 0.4f;
            }

            // Keep track of previous distance for next loop
            previousDistance = distance;
            yield return null;
        }
    }
}
