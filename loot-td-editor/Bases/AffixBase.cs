using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace loot_td
{
    public class AffixBase : BindableBase, IStringId
    {
        private string _idName;
        private AffixType _affixType;
        private int _tier;
        private int _spawnLevel;
        private ObservableCollection<AffixBonus> _affixBonuses;
        private ObservableCollection<AffixWeight> _spawnWeight;
        private ObservableCollection<GroupType> _groupTypes;

        [JsonProperty]
        public string IdName { get => _idName; set => SetProperty(ref _idName, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public AffixType AffixType { get => _affixType; set => SetProperty(ref _affixType, value); }

        [JsonProperty]
        public int Tier { get => _tier; set => SetProperty(ref _tier, value); }

        [JsonProperty]
        public int SpawnLevel { get => _spawnLevel; set => SetProperty(ref _spawnLevel, value); }

        [JsonProperty]
        public ObservableCollection<AffixBonus> AffixBonuses { get => _affixBonuses; set => SetProperty(ref _affixBonuses, value); }

        [JsonProperty]
        public ObservableCollection<AffixWeight> SpawnWeight { get => _spawnWeight; set => SetProperty(ref _spawnWeight, value); }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public ObservableCollection<GroupType> GroupTypes { get => _groupTypes; set => SetProperty(ref _groupTypes, value); }

        [JsonIgnore]
        public string ListString { get { return GetBonusTypesString(); } }

        [JsonIgnore]
        public string GetBonusCountString { get { return AffixBonuses.Count.ToString(); } }

        public AffixBase()
        {
            IdName = "";
            AffixBonuses = new ObservableCollection<AffixBonus>();
            SpawnWeight = new ObservableCollection<AffixWeight>();
            GroupTypes = new ObservableCollection<GroupType>();
        }

        public AffixBase(AffixBase a)
        {
            IdName = a.IdName;
            AffixType = a.AffixType;
            Tier = a.Tier;
            SpawnLevel = a.SpawnLevel;

            AffixBonuses = new ObservableCollection<AffixBonus>();
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

            SpawnWeight = new ObservableCollection<AffixWeight>();
            foreach (var b in a.SpawnWeight)
            {
                AffixWeight w = new AffixWeight
                {
                    type = b.type,
                    weight = b.weight
                };
                SpawnWeight.Add(w);
            }
            GroupTypes = new ObservableCollection<GroupType>(a.GroupTypes);
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

        public static int WeightContainsType(ObservableCollection<AffixWeight> s, GroupType type)
        {
            
            int i = 0;
            foreach (AffixWeight x in s)
            {
                if (x.type == type)
                    return i;
                i++;
            }
            return -1;
        }

        public string GetStringId()
        {
            return this.IdName;
        }
    }

    public class AffixBonus : BindableBase
    {

        private BonusType _bonusType;
        private ModifyType _modifyType;
        private int _minValue;
        private int _maxValue;


        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType BonusType { get => _bonusType; set => SetProperty(ref _bonusType, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType ModifyType { get => _modifyType; set => SetProperty(ref _modifyType, value); }

        [JsonProperty]
        public int MinValue { get => _minValue; set => SetProperty(ref _minValue, value); }

        [JsonProperty]
        public int MaxValue { get => _maxValue; set => SetProperty(ref _maxValue, value); }
    }

    public class AffixWeight
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType type { get; set; }

        [JsonProperty]
        public int weight { get; set; }

        public AffixWeight CloneWeight()
        {
            return (AffixWeight)this.MemberwiseClone();
        }
    }

    public enum AffixType
    {
        PREFIX,
        SUFFIX,
        ENCHANTMENT,
        INNATE,
        MONSTERMOD
    }

    public enum ModifyType
    {
        ADDITIVE,       //all sources add together before modifying
        MULTIPLY,       //all sources multiply together before modifying
        SET,            //sets value to modifier value, ignores all other increases
        FLAT_ADDITION   //adds to base before any other calculation
    }
}