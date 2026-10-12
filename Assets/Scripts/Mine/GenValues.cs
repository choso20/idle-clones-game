using UnityEngine;

public class GenValues : MonoBehaviour
{
    public float[,] genValuesArr = new float[4, 5];


    // 1.25f 
    
    // numOfGen, baseCost, costGrowthRate, productionBase, productionRate
    public void Awake()
    {
        genValuesArr[0, 0] = 0;
        genValuesArr[0, 1] = 100;
        genValuesArr[0, 2] = 1.1f;
        genValuesArr[0, 3] = 15;
        genValuesArr[0, 4] = 3;

        genValuesArr[1, 0] = 1;
        genValuesArr[1, 1] = 2000;
        genValuesArr[1, 2] = 1.2f;
        genValuesArr[1, 3] = 400;
        genValuesArr[1, 4] = 3;

        genValuesArr[2, 0] = 2;
        genValuesArr[2, 1] = 24000;
        genValuesArr[2, 2] = 1.18f;
        genValuesArr[2, 3] = 3600;
        genValuesArr[2, 4] = 3;

        genValuesArr[3, 0] = 3;
        genValuesArr[3, 1] = 84000;
        genValuesArr[3, 2] = 1.15f;
        genValuesArr[3, 3] = 28800;
        genValuesArr[3, 4] = 3;
    }

}
