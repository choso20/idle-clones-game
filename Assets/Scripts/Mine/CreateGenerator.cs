using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateGenerator : MonoBehaviour
{
    public GameObject GeneratorPrefab;
    public GameObject eRoomPrefab;
    public GameObject fRoomPrefab;

    private Vector3 positionOfCreate;
    private int roomLocNum;

    // Gen rooms
    [SerializeField] private TextMeshProUGUI costText;
    
    public GameObject creationCanvasPrefab;
    private GameObject creationCanvas;
    private TextMeshProUGUI genCostText;

    public void PositionOfRoom(Vector3 pos, int roomLocNumber)
    {
        positionOfCreate = pos;
        roomLocNum = roomLocNumber;
    }

    public void OpenCreationCanvas()
    {
        GameManager.Instance.cameraMovement = false;
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.5f);

        creationCanvas = Instantiate(creationCanvasPrefab);

        if (!GameManager.Instance.roomManager.reachedGenMax)
        {
            genCostText = creationCanvas.transform.Find("MenuBox").transform.Find("GenRoomContainer").transform.Find("GenCostText").GetComponent<TextMeshProUGUI>();
            genCostText.text = "Cost: " + GameManager.Instance.genValues.genValuesArr[GameManager.Instance.generatorsActive, 1].ToString();
            creationCanvas.transform.Find("MenuBox").transform.Find("GenRoomContainer").GetComponent<Button>().onClick.AddListener(OnCreateGeneratorRoom);
        }
        else
        {
            genCostText = creationCanvas.transform.Find("MenuBox").transform.Find("GenRoomContainer").transform.Find("GenCostText").GetComponent<TextMeshProUGUI>();
            genCostText.text = "Max built, need to expand";
            creationCanvas.transform.Find("MenuBox").transform.Find("GenRoomContainer").GetComponent<Button>().onClick.AddListener(buttonPress);
        }

        if (!GameManager.Instance.roomManager.reachedFeedMax)
        {
            costText = creationCanvas.transform.Find("MenuBox").transform.Find("FeedRoomContainer").transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            costText.text = "Cost: " + GameManager.Instance.roomValues.fRoomValuesArr[GameManager.Instance.feedRoomsActive, 1].ToString();
            creationCanvas.transform.Find("MenuBox").transform.Find("FeedRoomContainer").GetComponent<Button>().onClick.AddListener(OnCreateFeedRoom);
        }
        else
        {
            costText = creationCanvas.transform.Find("MenuBox").transform.Find("FeedRoomContainer").transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            costText.text = "Max built, need to expand";
            creationCanvas.transform.Find("MenuBox").transform.Find("EnergyRoomContainer").GetComponent<Button>().onClick.AddListener(buttonPress);
        }

        if (!GameManager.Instance.roomManager.reachedEnergyMax)
        {
            costText = creationCanvas.transform.Find("MenuBox").transform.Find("EnergyRoomContainer").transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            costText.text = "Cost: " + GameManager.Instance.roomValues.eRoomValuesArr[GameManager.Instance.energyRoomsActive, 1].ToString();
            creationCanvas.transform.Find("MenuBox").transform.Find("EnergyRoomContainer").GetComponent<Button>().onClick.AddListener(OnCreateEnergyRoom);
        }
        else
        {
            costText = creationCanvas.transform.Find("MenuBox").transform.Find("EnergyRoomContainer").transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            costText.text = "Max built, need to expand";
            creationCanvas.transform.Find("MenuBox").transform.Find("FeedRoomContainer").GetComponent<Button>().onClick.AddListener(buttonPress);
        }


        creationCanvas.SetActive(true);
    }

    private void buttonPress()
    {
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
    }

    public void OnCreateGeneratorRoom()
    {
        if (GameManager.Instance.moneyAmount > GameManager.Instance.genValues.genValuesArr[GameManager.Instance.generatorsActive, 1])
        {
            GameManager.Instance.moneyAmount -= GameManager.Instance.genValues.genValuesArr[GameManager.Instance.generatorsActive, 1];

            GameObject gen = Instantiate(GeneratorPrefab, positionOfCreate, Quaternion.identity);
            Generator genScript = gen.GetComponent<Generator>();

            genScript.SetGenNumber(GameManager.Instance.generatorsActive, roomLocNum); 
            genScript.CreateGenerator();

            FindObjectOfType<AudioManager>().Play("buttonPress", 1f);
            FindObjectOfType<AudioManager>().Play("buildingSound", 0.6f);
            FindObjectOfType<AudioManager>().bgVolChange();

            GameManager.Instance.generatorsActive++; GameManager.Instance.roomsMade++;

            Destroy(gameObject);
            Destroy(creationCanvas);
            GameManager.Instance.cameraMovement = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
            Debug.Log("Not enough monies to create");
        }

        HouseKeeping();
    }

    public void OnCreateFeedRoom()
    {
        if (GameManager.Instance.moneyAmount > GameManager.Instance.roomValues.fRoomValuesArr[GameManager.Instance.feedRoomsActive, 1])
        {
            GameManager.Instance.moneyAmount -= GameManager.Instance.roomValues.eRoomValuesArr[GameManager.Instance.feedRoomsActive, 1];

            GameObject room = Instantiate(fRoomPrefab, positionOfCreate, Quaternion.identity);
            FeedRoom roomScript = room.GetComponent<FeedRoom>();

            roomScript.SetRoomNumber(GameManager.Instance.feedRoomsActive, roomLocNum); 
            roomScript.CreateRoom();

            FindObjectOfType<AudioManager>().Play("buildingSound", 0.6f);
            FindObjectOfType<AudioManager>().bgVolChange();

            GameManager.Instance.feedRoomsActive++; GameManager.Instance.roomsMade++;

            Destroy(gameObject);
            Destroy(creationCanvas);
            GameManager.Instance.cameraMovement = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
            Debug.Log("Not enough monies to create");
        }

        HouseKeeping();
    }

    public void OnCreateEnergyRoom()
    {
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);

        if (GameManager.Instance.moneyAmount > GameManager.Instance.roomValues.eRoomValuesArr[GameManager.Instance.energyRoomsActive, 1])
        {
            GameManager.Instance.moneyAmount -= GameManager.Instance.roomValues.eRoomValuesArr[GameManager.Instance.energyRoomsActive, 1];

            GameObject room = Instantiate(eRoomPrefab, positionOfCreate, Quaternion.identity);
            EnergyRoom roomScript = room.GetComponent<EnergyRoom>();

            roomScript.SetRoomNumber(GameManager.Instance.energyRoomsActive, roomLocNum); 
            roomScript.CreateRoom();

            FindObjectOfType<AudioManager>().Play("buildingSound", 0.4f);
            FindObjectOfType<AudioManager>().bgVolChange();

            GameManager.Instance.energyRoomsActive++; GameManager.Instance.roomsMade++;

            Destroy(gameObject);
            Destroy(creationCanvas);
            GameManager.Instance.cameraMovement = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonPress", 0.4f);
            Debug.Log("Not enough monies to create");
        }

        HouseKeeping();
    }

    private void HouseKeeping()
    {
        GameManager.Instance.expansion.CheckForExpansion();
        GameManager.Instance.roomManager.UpdateMaxsForExpansion();

        SaveManager.Instance.saveData.roomLocationsTaken[roomLocNum] = true;

        SaveManager.Instance.SaveNow();
    }
}