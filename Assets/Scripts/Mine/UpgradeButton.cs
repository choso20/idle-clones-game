using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour
{
    public static event Action<GameObject> OnUpgradeButtonPressed;

    public void UpgradeButtonPress()
    {
        OnUpgradeButtonPressed?.Invoke(EventSystem.current.currentSelectedGameObject);
    }
}
