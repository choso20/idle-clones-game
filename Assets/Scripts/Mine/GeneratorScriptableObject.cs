using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorScriptableObject", menuName = "ScriptableObjects/GeneratorScriptableObject")]
public class GeneratorScriptableObject : ScriptableObject
{
    // Fields for formulas (NextUpgradeCost/ProductionRate)
    public int baseCost = 100;
    public float costGrowthRate = 1.3f;
    public int level = 0;
    public int productionBase = 15;
    public int multiplier = 1;
    public float production = 0;

    // How often a production is done (seconds)
    public int productionRate = 1;

    // Fields for upgrades
    public float nextUpgradeCost = 100;

    public void resetValues()
    {
        level = 0;
        multiplier = 1;
        production = 0;
        nextUpgradeCost = 100;
    }
}
