using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism;
using Prism.Mvvm;

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
        public List<AffixWeight> SpawnWeight { get; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<GroupType> GroupTypes { get; }

        [JsonIgnore]
        public string ListString { get { return GetBonusTypesString(); } }

        [JsonIgnore]
        public string GetBonusCountString { get { return AffixBonuses.Count.ToString(); } }

        public AffixBase()
        {
            AffixBonuses = new List<AffixBonus>();
            SpawnWeight = new List<AffixWeight>();
            GroupTypes = new List<GroupType>();
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

            SpawnWeight = new List<AffixWeight>();
            foreach (var b in a.SpawnWeight)
            {
                AffixWeight w = new AffixWeight
                {
                    type = b.type,
                    weight = b.weight
                };
                SpawnWeight.Add(w);
            }
            GroupTypes = new List<GroupType>(a.GroupTypes);
        }



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

        public static int WeightContainsType(List<AffixWeight> s, GroupType type)
        {
            int i = 0;
            foreach(AffixWeight x in s)
            {
                if (x.type == type)
                    return i;
                i++;
            }
            return -1;
        }

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

    public class AffixWeight
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType type { get; set; }
        [JsonProperty]
        public int weight {get; set;}
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