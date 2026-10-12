using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class EnergyRoom : MonoBehaviour
{
    private float baseCost;
    private float costGrowthRate;
    private float nextUpgradeCost;
    private int clonesPer;

    public int roomLevel;
    private int eRoomNum;
    public int roomNumber;
 

    //Updating placeholder text info 
    public TextMeshProUGUI nextUpgCostText;
    public TextMeshProUGUI levelUpText;

    
    public GameObject demoClonePrefab;
    private void InstantiateAndPositionClone()
    {
        // Instantiate the demo clone from the prefab
        GameObject demoClone = Instantiate(demoClonePrefab);

        // Set the demo clone's parent to the InsideRoomArea Transform with worldPositionStays set to false
        Transform insideRoomAreaTransform = transform.Find("InsideRoomArea");
        if (insideRoomAreaTransform != null)
        {
            demoClone.transform.SetParent(insideRoomAreaTransform, false);
        }

        // Since demoClonePrefab is now separate, we'll set its position using its own script
        CloneAnimation cloneAnimationScript = demoClone.GetComponent<CloneAnimation>();
        if (cloneAnimationScript != null)
        {
            cloneAnimationScript.RandomSpawn();
        }
        else
        {
            Debug.LogWarning("CloneAnimation script not found on the demo clone prefab!");
        }
    }

    /*private void IncrementLevelAddClones()
    {
        if (roomLevel == 5)
        {
            InstantiateAndPositionClone();
        }

        if (roomLevel == 10)
        {
            InstantiateAndPositionClone();
        }
    }
    private void InitialLevelAddClones()
    {
        if (roomLevel >= 10)
        {
            InstantiateAndPositionClone();
            InstantiateAndPositionClone();
        }
        else if (roomLevel >= 5)
        {
            InstantiateAndPositionClone();
        }
    }
    */

    public void SetRoomNumber(int num, int roomNum)
    {
        eRoomNum = num;
        roomNumber = roomNum;
    }

    private void Start()
    {
        SetValues();

        roomLevel = (int)SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber + "Level").GetValue(SaveManager.Instance.saveData);
        nextUpgradeCost = NextUpgradeCost();

        SetRoomText();

        InstantiateAndPositionClone();
        //InitialLevelAddClones();
    }

    private void IncrementLevel()
    {
        roomLevel++;
        GameManager.Instance.roomManager.UpdateSpace();

        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber + "Level").SetValue(SaveManager.Instance.saveData, roomLevel);

        //IncrementLevelAddClones();
    }

    private void SetValues()
    {
        baseCost = GameManager.Instance.roomValues.eRoomValuesArr[eRoomNum, 1];
        costGrowthRate = GameManager.Instance.roomValues.eRoomValuesArr[eRoomNum, 2];
        clonesPer = (int)GameManager.Instance.roomValues.eRoomValuesArr[eRoomNum, 3];
    }

    private float NextUpgradeCost()
    {
        float upgradeCost;
        upgradeCost = baseCost * Mathf.Pow(costGrowthRate, roomLevel);

        //Rounding to 2 d.p (for 1 d.p change the 100.0f to 10.0f and change 0.01f to 0.1f)
        upgradeCost = Mathf.Round(upgradeCost * 100.0f) * 0.01f;

        return upgradeCost;
    }

    public void UpgradeRoom()
    {
        if (GameManager.Instance.moneyAmount > nextUpgradeCost)
        {
            Debug.Log("Room upgraded");

            GameManager.Instance.moneyAmount -= nextUpgradeCost;

            IncrementLevel();

            nextUpgradeCost = NextUpgradeCost();

            SetRoomText();

            FindObjectOfType<AudioManager>().Play("buildingSound", 0.4f);
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);

            SaveManager.Instance.SaveNow();
        }
        else
        {
            Debug.Log("upgrade denied at: " + GameManager.Instance.moneyAmount + " monies");
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
        }
    }

    public void CreateRoom()
    {
        Debug.Log("room" + roomNumber);

        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString()).SetValue(SaveManager.Instance.saveData, "Sleep");
        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString() + "Num").SetValue(SaveManager.Instance.saveData, eRoomNum);
        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString() + "LocNum").SetValue(SaveManager.Instance.saveData, roomNumber);

        Debug.Log(SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString()).GetValue(SaveManager.Instance.saveData));

        gameObject.SetActive(true);
        SetValues();

        IncrementLevel();

        nextUpgradeCost = NextUpgradeCost();

        SetRoomText();
    }

    private void SetRoomText()
    {
        nextUpgCostText.text = "Next Upg Cost: " + GameManager.Instance.FormatNumber(nextUpgradeCost);
        levelUpText.text = "Level " + roomLevel.ToString();
    }
}
