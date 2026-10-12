using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;


    private void Start()
    {
        instance = this;
    }


    public GameObject demoInfoCanvas;
    public TextMeshProUGUI demoInfoText;
    
    public void OnInfoCanvas()
    {
        if (demoInfoCanvas.activeInHierarchy == true)
            demoInfoCanvas.SetActive(false);
        else
            demoInfoCanvas.SetActive(true);

        FindObjectOfType<AudioManager>().Play("buttonPress", 0.5f);
    }

    public void onMoney()
    {
        GameManager.Instance.moneyAmount += 10000;
    }
}
