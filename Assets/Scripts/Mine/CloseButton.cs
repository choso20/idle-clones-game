using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void OnCloseButton()
    {
        transform.parent.parent.gameObject.SetActive(false);
        GameManager.Instance.cameraMovement = true;
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.5f);
    }

    public void OnCloseCreationCanvas()
    {
        Destroy(transform.parent.gameObject);
        GameManager.Instance.cameraMovement = true;
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.5f);
    }
}
