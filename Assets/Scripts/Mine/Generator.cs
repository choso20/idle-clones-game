using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    // Visual upgrades to change to at certain levels, put sprites in generator script under generator prefab when got them
    [SerializeField] private Sprite[] genVisualSprites;

    //Updating placeholder text info 
    public TextMeshProUGUI nextUpgCostText;
    public TextMeshProUGUI currentProdText;
    public TextMeshProUGUI levelUpText;

    private GeneratorData genData = new GeneratorData(100, 1.3f, 15, 1);

    public int genLevel;
    public int baseCostgrab;

    public int genNumber;
    public int roomNumber;

    public GameObject demoClonePrefab;

    public void SetGenNumber(int genNum, int roomNum)
    {
        genNumber = genNum;
        roomNumber = roomNum;
    }

    private void Start()
    {
        SetValues();

        genLevel = (int)SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber + "Level").GetValue(SaveManager.Instance.saveData);
        genData.level = genLevel;
        genData.nextUpgradeCost = NextUpgradeCost();
        genData.production = ProductionRate();

        SetGeneratorText();
        SetMultiplier();
        InvokeRepeating("AddMoney", 0.001f, genData.productionRate);

        gameObject.SetActive(true);

        InstantiateAndPositionClone();
        //InitialLevelAddClones();
    }

    private void OnEnable()
    {
        RoomManager.BottleneckChange += Bottleneck;
    }

    
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

    /*
    private void IncrementLevelAddClones()
    {
        if (genData.level == 5)
        {
            InstantiateAndPositionClone();
        }

        if (genData.level == 10)
        {
            InstantiateAndPositionClone();
        }
    }
    private void InitialLevelAddClones()
    {
        if (genData.level >= 10)
        {
            InstantiateAndPositionClone();
            InstantiateAndPositionClone();
        }
        else if (genData.level >= 5)
        {
            InstantiateAndPositionClone();
        }
    }
    */

    private void OnDisable()
    {
        RoomManager.BottleneckChange -= Bottleneck;
    }

    private void Bottleneck(int bottleneckLevel)
    {
        if (bottleneckLevel == 3)
            genData.production = genData.production / 6;
        else if (bottleneckLevel == 2)
            genData.production = genData.production / 3;
        else if (bottleneckLevel == 1)
        {
            genData.production = genData.production / 1.5f;
        }
        else if (bottleneckLevel == 0)
            genData.production = genData.production / 1;
    }

    private void SetValues()
    {
        genData.baseCost = (int)GameManager.Instance.genValues.genValuesArr[genNumber, 1];
        genData.costGrowthRate = GameManager.Instance.genValues.genValuesArr[genNumber, 2];
        genData.productionBase = (int)GameManager.Instance.genValues.genValuesArr[genNumber, 3];
        genData.productionRate = (int)GameManager.Instance.genValues.genValuesArr[genNumber, 4];
    }

    private void SetMultiplier()
    {
        if (genData.level >= 100)
            genData.multiplier = 16;
        else if (genData.level >= 75)
            genData.multiplier = 8;
        else if (genData.level >= 50)
            genData.multiplier = 4;
        else if (genData.level >= 25)
            genData.multiplier = 2;
        else if (genData.level >= 10)
            genData.multiplier = 1;
    }

    private void AddMoney()
    {
        if (GameManager.Instance == null)
            Debug.Log("gamemanager null");

        GameManager.Instance.moneyAmount += genData.production;
    }

    private void IncrementLevel()
    {
        genData.level++;
        genLevel = genData.level;
        SetMultiplier();

        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber + "Level").SetValue(SaveManager.Instance.saveData, genLevel);

        GameManager.Instance.roomManager.UpdateSpace();
    }

    private float NextUpgradeCost()
    {
        float upgradeCost;
        upgradeCost = genData.baseCost * Mathf.Pow(genData.costGrowthRate, genData.level);

        //Rounding to 2 d.p (for 1 d.p change the 100.0f to 10.0f and change 0.01f to 0.1f)
        upgradeCost = Mathf.Round(upgradeCost * 100.0f) * 0.01f;

        return upgradeCost;
    }

    private float ProductionRate()
    {
        float prodRate = 0;

        prodRate = (genData.productionBase * genData.level) * genData.multiplier;

        //testing to balance
        //prodRate = genData.productionBase * Mathf.Pow(genData.level, 2f);

        //Rounding to 2 d.p (for 1 d.p change the 100.0f to 10.0f and change 0.01f to 0.1f)
        prodRate = Mathf.Round(prodRate * 100.0f) * 0.01f;

        return prodRate;
    }

    private void SetGeneratorText()
    {
        nextUpgCostText.text = "Next Upg Cost: " + GameManager.Instance.FormatNumber(genData.nextUpgradeCost);
        currentProdText.text = "Current Prod: " + GameManager.Instance.FormatNumber(genData.production);
        levelUpText.text = "Level " + genData.level.ToString();
    }

    public void UpgradeGenerator()
    {
        //Debug.Log("Button pressed on " + gameObject.name);

        // Make thing where checks name of gen that upg gen pressed on then run functions n update values for that gen
        //Debug.Log(gameObject.transform.parent.parent.parent.name);


        if (GameManager.Instance.moneyAmount > genData.nextUpgradeCost)
        {
            Debug.Log("Generator upgraded");

            GameManager.Instance.moneyAmount -= genData.nextUpgradeCost;

            IncrementLevel();

            genData.nextUpgradeCost = NextUpgradeCost();
            genData.production = ProductionRate();

            SetGeneratorText();

            FindObjectOfType<AudioManager>().Play("buildingSound", 0.4f);
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);

            SaveManager.Instance.SaveNow();

            //IncrementLevelAddClones();
        }
        else
        {
            Debug.Log("upgrade denied at: " + GameManager.Instance.moneyAmount + " monies");
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
        }
    }

    public void CreateGenerator()
    {
        gameObject.SetActive(true);

        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString()).SetValue(SaveManager.Instance.saveData, "Gen");
        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString() + "Num").SetValue(SaveManager.Instance.saveData, genNumber);
        SaveManager.Instance.saveData.GetType().GetField("room" + roomNumber.ToString() + "LocNum").SetValue(SaveManager.Instance.saveData, roomNumber);

        SetValues();
        IncrementLevel();

        genData.nextUpgradeCost = NextUpgradeCost();
        genData.production = ProductionRate();

        SetGeneratorText();

        InvokeRepeating("AddMoney", 0.001f, genData.productionRate);
    }

    public float MoneyGeneratedWhileIdle(int minutesPassed)
    {
        //float produced = ProductionRate();
        //int timesProdPerMinute = 60 / genData.productionRate;
        //float prodInAMinute = produced * timesProdPerMinute;
        //float producedInTimePassed = prodInAMinute * minutesPassed;

        float totalProducedInTimePassed = (ProductionRate() * (60 / genData.productionRate)) * minutesPassed;

        if (minutesPassed != 0)
            Debug.LogWarning("MinutesPassed = 0");

        return totalProducedInTimePassed;
    }
}
