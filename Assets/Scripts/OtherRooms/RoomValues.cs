using UnityEngine;

public class RoomValues : MonoBehaviour
{
    public float[,] eRoomValuesArr = new float[4, 4];
    public float[,] fRoomValuesArr = new float[4, 4];

    //    0           1         2                 3
    // Num of gen, Basecost, CostGrowthRate, Clonesper
    public void Awake()
    {
        SetERoomValues();
        SetFRoomValues();
    }

    private void SetFRoomValues()
    {
        fRoomValuesArr[0, 0] = 1;
        fRoomValuesArr[0, 1] = 60000;
        fRoomValuesArr[0, 2] = 1.15f;
    }

    private void SetERoomValues()
    {
        eRoomValuesArr[0, 0] = 1;
        eRoomValuesArr[0, 1] = 100000;
        eRoomValuesArr[0, 2] = 1.17f;
    }
}
