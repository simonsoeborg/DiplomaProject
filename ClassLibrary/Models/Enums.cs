namespace ClassLibrary.Models
{
    public enum QualityType
    {
        Undefined = 0,
        FirstQuality = 1,
        SecondQuality = 2,
        ThirdQuality = 3,
    }

    public enum ConditionType
    {
        Undefined = 0,
        NoShards = 1,
        FewShards = 2,
        ManyShards = 3,
    }

    public enum MaterialType
    {
        undefined = 0,
        porcelain = 1,
        steel = 2,
        glass = 3,
        gold = 4,
        silver = 5,
        ceramics = 6,
        stoneware = 7,
        fajance = 8,
        // etc..
    }
}