using UnityEngine;
using System;
using BayatGames.SaveGameFree;
using System.Security.Cryptography;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    private GameManager gManagerDirect;

    public SaveFile saveData = new SaveFile();

    [SerializeField] private ExpansionButton expansion;

    private DateTime timeNow = DateTime.Now;
    private DateTime lastLogin;
    private DateTime lastLoginToCompare;

    [SerializeField] private TextMeshProUGUI idleGeneratedMoniesText;
    [SerializeField] private GameObject idleGeneratedCanvas;

    private void Start()
    {
        instance = this;
        gManagerDirect = GetComponent<GameManager>();

        Application.quitting += OnQuitting;

        LoadData();

        float moneyProducedWhileIdle = MoneyProducedWhileIdle();
        if (moneyProducedWhileIdle > 0)
        {
            //idleGeneratedMoniesText.text = moneyProducedWhileIdle.ToString();
            idleGeneratedMoniesText.text = gManagerDirect.FormatNumber(moneyProducedWhileIdle);
            idleGeneratedCanvas.gameObject.SetActive(true);
            gManagerDirect.moneyAmount += moneyProducedWhileIdle;
            SaveNow();
        }
        else
            return;

        

        InvokeRepeating("AutoSave", 5, 30);
    }

    public void AutoSave()
    {
        SaveNow();

        Debug.Log("AutoSaved at " + DateTime.Now);
    }

    public void SaveNow()
    {
        saveData.money = gManagerDirect.moneyAmount;

        lastLogin = DateTime.Now;
        saveData.lastLoginDateTime = lastLogin.ToString();

        saveData.currentExpansion = expansion.currentExpansion;
        saveData.roomsMade = gManagerDirect.roomsMade;
        saveData.generatorsActive = gManagerDirect.generatorsActive;
        saveData.feedRoomsActive = gManagerDirect.feedRoomsActive;
        saveData.energyRoomsActive = gManagerDirect.energyRoomsActive;

        SaveGame.Save<SaveFile>("SaveFile", saveData);
        Debug.Log("Game data saved to SaveFile");
    }

    public void FirstSave()
    {
        saveData.money = 150;
        gManagerDirect.moneyAmount = saveData.money;
        //gManagerDirect.moneyAmount = 10000;
        saveData.lastLoginDateTime = DateTime.Now.ToString();
        saveData.currentExpansion = 0;
        //Debug.Log(saveData.lastLoginDateTime + " first save lastlogin");

        saveData.currentExpansion = 0;
        saveData.roomsMade = 0;
        saveData.generatorsActive = 0;
        saveData.feedRoomsActive = 0;
        saveData.energyRoomsActive = 0;

        for (int i = 0; i < saveData.roomLocationsTaken.Length; i++)
        {
            saveData.roomLocationsTaken[i] = false;
        }

        for (int i = 0; i < 6; i++)
        {
            saveData.GetType().GetField("room" + i.ToString()).SetValue(saveData, "Null");
        }

        SaveGame.Save<SaveFile>("SaveFile", saveData);
        Debug.Log("First save created with zero values");

    }

    public void LoadData()
    {
        if (SaveGame.Exists("SaveFile"))
        {
            Debug.Log("Save file found n loaded");
            saveData = SaveGame.Load<SaveFile>("SaveFile");

            gManagerDirect.moneyAmount = saveData.money;

            if (saveData.lastLoginDateTime == null)
                Debug.LogWarning("saveData.lastLoginDateTime is null");
            else
                lastLoginToCompare = DateTime.Parse(saveData.lastLoginDateTime);

            expansion.currentExpansion = saveData.currentExpansion;
            gManagerDirect.roomsMade = saveData.roomsMade;
            gManagerDirect.generatorsActive = saveData.generatorsActive;
            gManagerDirect.feedRoomsActive = saveData.feedRoomsActive;
            gManagerDirect.energyRoomsActive = saveData.energyRoomsActive;
        }
        else
        {
            Debug.Log("No save file found");
            FirstSave();
        }
    }

    public void ResetSave()
    {
        SaveGame.Delete("SaveFile");
        Debug.Log("SaveReset");

        Application.quitting -= OnQuitting;
    }

    private void OnQuitting()
    {
        SaveNow();
    }

    private float CalculateGeneratorIncome(int genNumber, int level, int minutesIdle)
    {
        float baseProd = GameManager.Instance.genValues.genValuesArr[genNumber, 3];
        float rate = GameManager.Instance.genValues.genValuesArr[genNumber, 4];
        float production = baseProd * level;  // Simplified model, adjust with actual logic

        float productionPerMinute = production / rate;
        float totalProduced = productionPerMinute * minutesIdle;

        return totalProduced;
    }

    private float MoneyProducedWhileIdle()
    {
        TimeSpan timePassedWhileIdle = timeNow - lastLoginToCompare;
        int minutesPassedWhileIdle = (int)timePassedWhileIdle.TotalMinutes;
        Debug.Log(minutesPassedWhileIdle + " minutes passed while idle");

        if (minutesPassedWhileIdle == 0)
        {
            return 0f;
        }

        float moniesGeneratedWhileIdle = 0;
        for (int i = 0; i < saveData.roomsMade; i++)
        {
            string roomType = (string)saveData.GetType().GetField("room" + i).GetValue(saveData);
            if (roomType == "Gen")
            {
                int genNumber = (int)saveData.GetType().GetField("room" + i + "Num").GetValue(saveData);
                int genLevel = (int)saveData.GetType().GetField("room" + i + "Level").GetValue(saveData);
                moniesGeneratedWhileIdle += CalculateGeneratorIncome(genNumber, genLevel, minutesPassedWhileIdle);
            }
        }

        //Rounding to 1 d.p (for 2 d.p change the 10.0f to 100.0f)
        moniesGeneratedWhileIdle = Mathf.Round(moniesGeneratedWhileIdle * 10.0f) * 0.1f;

        moniesGeneratedWhileIdle = moniesGeneratedWhileIdle / 4;

        return moniesGeneratedWhileIdle;
     
    

    }
}
