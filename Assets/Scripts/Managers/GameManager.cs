using TMPro;
using UnityEngine;

// abc abc test code commment
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public float moneyAmount;

    public TextMeshProUGUI moneyAmountText;

    public bool cameraMovement;
    
    // Class references
    public ExpansionButton expansion;
    public RoomManager roomManager;
    public GenValues genValues;
    public RoomValues roomValues;

    // Room stuff
    public int generatorsActive;
    public int energyRoomsActive;
    public int feedRoomsActive;
    public int roomsMade;

    AudioManager audioManager;

    private void Start()
    {
        instance = this;
        InvokeRepeating("UpdateMonies", 0.01f, 0.5f);

        cameraMovement = true;
    }

    private void UpdateMonies()
    {
        moneyAmountText.text = "Cash: " + FormatNumber(moneyAmount);
    }

    public string FormatNumber(float num)
    {
        float numToFormat = num;

        if (numToFormat < 1000)
        {
            //Rounding to 2 d.p (for 1 d.p change the 100.0f to 10.0f and change 0.01f to 0.1f)
            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString();
        }
        else if (numToFormat < Mathf.Pow(10,6)) // Less than a million (for thousands)
        {
            numToFormat = numToFormat / 1000; // Div by thousand

            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString() + "K";
        }
        else if (numToFormat < Mathf.Pow(10, 9))// Less than a billion (for millions)
        {
            numToFormat = numToFormat / Mathf.Pow(10, 6); // div by million

            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString() + "M";
        }
        else if (numToFormat < Mathf.Pow(10, 12)) // Less than a trillion (for billions)
        {
            numToFormat = numToFormat / Mathf.Pow(10, 9); // Div by billion

            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString() + "B";
        }
        else if (numToFormat < Mathf.Pow(10, 15)) // Less than a Quadrillion (for trillions)
        {
            numToFormat = numToFormat / Mathf.Pow(10, 12); // Div by trillion

            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString() + "T";
        }
        else if (numToFormat < Mathf.Pow(10, 18)) // Less than a Quintillion (for Quadrillion)
        {
            numToFormat = numToFormat / Mathf.Pow(10, 15); // Div by quadrillion

            numToFormat = Mathf.Round(numToFormat * 100.0f) * 0.01f;

            return numToFormat.ToString() + "q";
        }
        else
        {
            return "Too much lol";
        }
    }
}
