using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace loot_td
{

    public class AffixBase
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
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
            SpawnWeight.Add(GroupType.NO_GROUP, 0);
        }

        public AffixBase(AffixBase a)
        {
            Id = a.Id;
            Name = a.Name;
            AffixType = a.AffixType;
            Tier = a.Tier;
            SpawnLevel = a.SpawnLevel;

            AffixBonuses = new List<AffixBonus>();
            foreach (var b in a.AffixBonuses)
            {
                AffixBonus bonus = new AffixBonus
                {
                    BonusType = b.BonusType,
                    ModifyType = b.ModifyType,
                    MinValue = b.MinValue,
                    MaxValue = b.MaxValue
                };
                AffixBonuses.Add(bonus);
            }

            SpawnWeight = new Dictionary<GroupType, int>(a.SpawnWeight);
            GroupTypes = new List<GroupType>(a.GroupTypes);
        }

        [JsonIgnore]
        public string ListString { get { return GetBonusTypesString(); } }

        public string GetBonusTypesString()
        {
            string x = "";
            foreach (var a in AffixBonuses)
            {
                x += a.BonusType.ToString();
                x += ", ";
            }
            x = x.TrimEnd(' ');
            x = x.TrimEnd(',');
            return x;
        }

        [JsonIgnore]
        public string GetBonusCountString { get { return AffixBonuses.Count.ToString(); } }
    }

    public class AffixBonus
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType BonusType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType ModifyType { get; set; }
        [JsonProperty]
        public int MinValue { get; set; }
        [JsonProperty]
        public int MaxValue { get; set; }
    }

    public enum AffixType
    {
        PREFIX,
        SUFFIX,
        ENCHANTMENT,
        INNATE
    }

    public enum ModifyType
    {
        ADDITIVE,       //all sources add together before modifying
        MULTIPLY,       //all sources multiply together before modifying
        SET,            //sets value to modifier value, ignores all other increases
        FLAT_ADDITION   //adds to base before any other calculation
    }

}