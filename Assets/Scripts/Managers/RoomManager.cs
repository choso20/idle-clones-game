using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject CreateRoomPrefab;
    [SerializeField] private ExpansionButton expansion;
    
    public Vector3[] roomLocations = new Vector3[6];

    public int roomButtonOn;



    public int bottleneckLevel;
    public static event Action<int> BottleneckChange;

    // Expansion stuff fields
    private int maxGens;
    private int maxEnergyRooms;
    private int maxFeedRooms;
    public bool reachedGenMax;
    public bool reachedFeedMax;
    public bool reachedEnergyMax;

    // Clone room things
    public float currentCloneAmount;
    public float feedSpaceNeeded;
    public float energySpaceNeeded;
    public float cloneFeedSpace;
    public float cloneEnergySpace;

    private void Update()
    {
        UIManager.Instance.demoInfoText.text = "currentCloneAmount: " + currentCloneAmount.ToString()
            + "\n\nfeedSpaceNeeded: " + feedSpaceNeeded.ToString() + "\nenergySpaceNeeded: " + energySpaceNeeded.ToString() +
            "\n\ncloneFeedSpace: " + cloneFeedSpace.ToString() + "\ncloneEnergySpace: " + cloneEnergySpace.ToString();
    } // temporary

    public void UpdateSpace()
    {
        currentCloneAmount = GameManager.Instance.generatorsActive * 0.03f;
        feedSpaceNeeded = currentCloneAmount / 2;
        energySpaceNeeded = currentCloneAmount / 2;

        cloneFeedSpace = (GameManager.Instance.feedRoomsActive * 0.03f) + 0.03f;
        cloneEnergySpace = (GameManager.Instance.energyRoomsActive * 0.03f) + 0.03f;

        SetBottleneck();
    }

    private void SetBottleneck() // levels 1: minor inconvience, 2: annoyed slow work pace, 3: strikes, minimal work done. 0: no issues
    {
        if (feedSpaceNeeded > cloneFeedSpace)
        {
            if (feedSpaceNeeded - cloneFeedSpace > 0.2){
                bottleneckLevel = 3; BottleneckChange?.Invoke(bottleneckLevel); 
            }
            else if (feedSpaceNeeded - cloneFeedSpace > 0.1) {
                bottleneckLevel = 2; BottleneckChange?.Invoke(bottleneckLevel); 
            }
            else if (feedSpaceNeeded - cloneFeedSpace > 0.05) {
                bottleneckLevel = 1; BottleneckChange?.Invoke(bottleneckLevel);
                Debug.Log("feed space level 1 bottleneck");
            }
        }
        else
        {
            bottleneckLevel = 0;
            BottleneckChange?.Invoke(bottleneckLevel);
        }


        if (energySpaceNeeded > cloneEnergySpace)
        {
            if (energySpaceNeeded - cloneEnergySpace > 0.2) {
                bottleneckLevel = 3; BottleneckChange?.Invoke(bottleneckLevel);
            }
            else if (energySpaceNeeded - cloneEnergySpace > 0.1) { 
                bottleneckLevel = 2; BottleneckChange?.Invoke(bottleneckLevel);
            }
            else if (energySpaceNeeded - cloneEnergySpace > 0.05) { 
                bottleneckLevel = 1; BottleneckChange?.Invoke(bottleneckLevel);
            }
        }
        else
        {
            bottleneckLevel = 0;
            BottleneckChange?.Invoke(bottleneckLevel);
        }
    }

    private void SetRoomLocations()
    {
        roomLocations[0] = new Vector3(-0.460000008f, -1.14300001f, 0);
        roomLocations[1] = new Vector3(-0.460000008f, -3.40899992f, 0);
        roomLocations[2] = new Vector3(-7.44099998f, -1.13999999f, 0); 
        roomLocations[3] = new Vector3(-7.44099998f, -3.40899992f, 0);
        roomLocations[4] = new Vector3(-7.44299984f, 1.12f, 0);
        roomLocations[5] = new Vector3(-7.44299984f, 3.39299989f, 0);
    }

    private void Start()
    {
        UpdateSpace();
        SetRoomLocations();

        roomButtonOn = GameManager.Instance.roomsMade;

        //if (GameManager.Instance.roomsMade >= 1)
        //    roomButtonOn = GameManager.Instance.roomsMade - 1; 
        //else
        //    roomButtonOn = GameManager.Instance.roomsMade;

        expansion.CheckForExpansion();
        UpdateMaxsForExpansion();

        int tocheck = 0;
        if (expansion.currentExpansion == 0)
            tocheck = 2;
        else if (expansion.currentExpansion == 1)
            tocheck = 6;

        for (int i = 0; i < tocheck; i++)
        {
            Debug.Log(i);
            Debug.Log("roomsmade = " + GameManager.Instance.roomsMade);

            if ((string)SaveManager.Instance.saveData.GetType().GetField("room" + i.ToString()).GetValue(SaveManager.Instance.saveData) == "Gen") // Generator
            {
                GameObject gen = Instantiate(CreateRoomPrefab.GetComponent<CreateGenerator>().GeneratorPrefab,
                    roomLocations[(int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData)], Quaternion.identity);
                Generator genScript = gen.GetComponent<Generator>();

                genScript.SetGenNumber((int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "Num").GetValue(SaveManager.Instance.saveData),
                    (int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData));
            }
            else if ((string)SaveManager.Instance.saveData.GetType().GetField("room" + i.ToString()).GetValue(SaveManager.Instance.saveData) == "Kitchen") // Kitchen 
            {
                Debug.Log("Kitchen detected");

                GameObject gen = Instantiate(CreateRoomPrefab.GetComponent<CreateGenerator>().fRoomPrefab,
                    roomLocations[(int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData)], Quaternion.identity);
                FeedRoom genScript = gen.GetComponent<FeedRoom>();

                genScript.SetRoomNumber((int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "Num").GetValue(SaveManager.Instance.saveData),
                    (int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData));
            }
            else if ((string)SaveManager.Instance.saveData.GetType().GetField("room" + i.ToString()).GetValue(SaveManager.Instance.saveData) == "Sleep") // Sleeping quarters
            {
                Debug.Log("Sleep detected");

                GameObject gen = Instantiate(CreateRoomPrefab.GetComponent<CreateGenerator>().eRoomPrefab,
                    roomLocations[(int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData)], Quaternion.identity);
                EnergyRoom genScript = gen.GetComponent<EnergyRoom>();

                genScript.SetRoomNumber((int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "Num").GetValue(SaveManager.Instance.saveData),
                    (int)SaveManager.Instance.saveData.GetType().GetField("room" + i + "LocNum").GetValue(SaveManager.Instance.saveData));
            }

            Debug.Log("got to the end");
        }

        InstantiateButtons();

        SetBottleneck();
    }
    public void InstantiateButtons()
    {
        if (expansion.currentExpansion == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (SaveManager.Instance.saveData.roomLocationsTaken[i] == false)
                {
                    GameObject room = Instantiate(CreateRoomPrefab, roomLocations[i], Quaternion.identity);
                    CreateGenerator roomscript = room.GetComponent<CreateGenerator>();
                    roomscript.PositionOfRoom(roomLocations[i], i);
                }
            }
        }
        else if (expansion.currentExpansion == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                if (SaveManager.Instance.saveData.roomLocationsTaken[i] == false)
                {
                    GameObject room = Instantiate(CreateRoomPrefab, roomLocations[i], Quaternion.identity);
                    CreateGenerator roomscript = room.GetComponent<CreateGenerator>();
                    roomscript.PositionOfRoom(roomLocations[i], i);
                }
            }
        }
    }


    private void InstantiateButtonFake()
    {
        if (SaveManager.Instance.saveData.roomLocationsTaken[roomButtonOn] == false)
        {
            GameObject room = Instantiate(CreateRoomPrefab, roomLocations[roomButtonOn], Quaternion.identity);
            CreateGenerator roomscript = room.GetComponent<CreateGenerator>();
            roomscript.PositionOfRoom(roomLocations[roomButtonOn], roomButtonOn);
        }
        else
            return;

        roomButtonOn++;
    }

    public void UpdateMaxsForExpansion()
    {
        if (GameManager.Instance.expansion.currentExpansion == 0)
        {
            maxGens = 2;
            maxFeedRooms = 0;
            maxEnergyRooms = 0;
            SetMaxBoolsFalse();
        }
        else if (GameManager.Instance.expansion.currentExpansion == 1)
        {
            maxGens = 4;
            maxFeedRooms = 1;
            maxEnergyRooms = 1;
            SetMaxBoolsFalse();
        }

        MaxCheck();
    }
    public void MaxCheck()
    {

        if (GameManager.Instance.generatorsActive >= maxGens)
            reachedGenMax = true;
        if (GameManager.Instance.feedRoomsActive >= maxFeedRooms)
            reachedFeedMax = true;
        if (GameManager.Instance.energyRoomsActive >= maxEnergyRooms)
            reachedEnergyMax = true;
    }
    private void SetMaxBoolsFalse()
    {
        reachedGenMax = false;
        reachedFeedMax = false;
        reachedEnergyMax = false;
    }
}
