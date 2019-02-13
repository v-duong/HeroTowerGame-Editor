using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace loot_td {

public class AffixBase
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string Name { get; set; }
    [JsonConverter(typeof(StringEnumConverter))] [JsonProperty]
    public AffixType AffixType { get; set; }
    [JsonProperty]
    public int Tier { get; set; }
    [JsonProperty]
    public int SpawnLevel { get; set; }
    [JsonProperty]
    public List<AffixBonus> AffixBonuses { get; }
    [JsonProperty]
    public Dictionary<GroupType, int> SpawnWeight { get; }
    [JsonProperty]
    public List<GroupType> GroupTypes { get; }

    public AffixBase()
    {
        AffixBonuses = new List<AffixBonus>();
        SpawnWeight = new Dictionary<GroupType, int>();
        GroupTypes = new List<GroupType>();
    }

    public string ListString { get { return GetBonusTypesString(); } }

    public string GetBonusTypesString()
    {
        string x = "(";
        foreach (var a in AffixBonuses)
        {
            x += a.bonusType.ToString();
            x += ", ";
        }
        x.TrimEnd(',');
        x += ")";
        return x;
    }
}

public class AffixBonus
{
    [JsonConverter(typeof(StringEnumConverter))] [JsonProperty]
    public BonusType bonusType;
    [JsonConverter(typeof(StringEnumConverter))] [JsonProperty]
    public ModifyType modifyType;
    [JsonProperty]
    public int minValue;
    [JsonProperty]
    public int maxValue;
}

public enum AffixType
{
    PREFIX,
    SUFFIX,
    ENCHANTMENT
}

public enum ModifyType
{
    ADDITIVE,
    MULTIPLY,
    SET
}

}