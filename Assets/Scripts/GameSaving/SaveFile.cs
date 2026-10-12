public class SaveFile
{
    public float money;

    #region Room Save Values

    public bool[] roomLocationsTaken = new bool[6];

    // (first string 'room0' =) rooms can be "Null" for nothing, "Gen", "Kitchen", "Sleep".
    // LocNum = Location Number but also general room number
    // Just 'Num' refers to the type of room num so the number that that is in the generators or feed rooms
    public string room0;
    public int room0Num;
    public int room0Level;
    public int room0LocNum;

    public string room1;
    public int room1Num;
    public int room1Level;
    public int room1LocNum;

    public string room2;
    public int room2Num;
    public int room2Level;
    public int room2LocNum;

    public string room3;
    public int room3Num;
    public int room3Level;
    public int room3LocNum;

    public string room4;
    public int room4Num;
    public int room4Level;
    public int room4LocNum;

    public string room5;
    public int room5Num;
    public int room5Level;
    public int room5LocNum;

    #endregion

    public int currentExpansion;
    public int roomsMade;
    public int generatorsActive;
    public int energyRoomsActive;
    public int feedRoomsActive;

    public string lastLoginDateTime;
}
