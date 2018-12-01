
public class DragonData
{
    public int DailyAppetitite { get; set; }
    public int FightingStrength { get; set; }

    public DragonData()
    {
        DailyAppetitite = 5;
        FightingStrength = 10;
    }

    public DragonData(int dailyAppetitite, int fightingStrength)
    {
        DailyAppetitite = dailyAppetitite;
        FightingStrength = fightingStrength;
    }
}
