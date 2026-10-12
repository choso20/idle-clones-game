public class GeneratorData
{
    // Fields for formulas (NextUpgradeCost/ProductionRate)
    public int baseCost;
    public float costGrowthRate;
    public int level = 0;
    public int productionBase;
    public float multiplier = 1;
    public float production = 0;

    // How often a production is done (seconds)
    public int productionRate;

    // Fields for upgrades
    public float nextUpgradeCost;

    // Clone amount
    public float genCloneAmount;

    public GeneratorData(int baseCost_, float costGrowthRate_, int productionBase_, int productionRate_)
    {
        baseCost = baseCost_;
        costGrowthRate = costGrowthRate_;
        productionBase = productionBase_;
        productionRate = productionRate_;

        nextUpgradeCost = baseCost_;
    }
}