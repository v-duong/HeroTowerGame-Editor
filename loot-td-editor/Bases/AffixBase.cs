using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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
        private ObservableCollection<TriggeredEffectProperty> addedEffects;
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
        public ObservableCollection<AffixBonus> AffixBonuses { get => _affixBonuses; set { SetProperty(ref _affixBonuses, value); } }

        [JsonProperty]
        public ObservableCollection<TriggeredEffectProperty> TriggeredEffects { get => addedEffects; set { SetProperty(ref addedEffects, value); } }

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
            TriggeredEffects = new ObservableCollection<TriggeredEffectProperty>();
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

            TriggeredEffects = new ObservableCollection<TriggeredEffectProperty>();

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

        public void RaiseListStringChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= RaiseListStringChanged_;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += RaiseListStringChanged_;
                }
            }
        }

        public void RaiseListStringChanged_(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("ListString");
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
        private float _minValue;
        private float _maxValue;
        private GroupType _restriction;
        private bool _readAsFloat;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType BonusType { get => _bonusType; set { SetProperty(ref _bonusType, value); } }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType ModifyType { get => _modifyType; set => SetProperty(ref _modifyType, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType Restriction { get => _restriction; set => SetProperty(ref _restriction, value); }

        [JsonProperty]
        public float MinValue { get => _minValue; set => SetProperty(ref _minValue, value); }

        [JsonProperty]
        public float MaxValue { get => _maxValue; set => SetProperty(ref _maxValue, value); }

        [JsonProperty]
        public bool ReadAsFloat { get => _readAsFloat; set => SetProperty(ref _readAsFloat, value); }

        public AffixBonus()
        {
        }
    }

    public class TriggeredEffectProperty : BindableBase
    {
        private TriggerType applicationType;

        private float applicationTriggerValue;

        private GroupType restriction;

        private AbilityTargetType effectTargetType;

        private EffectType effectType;

        private BonusType statBonusType;

        private ModifyType statModifyType;

        private float applicationChance;

        private float effectMinValue;

        private float effectMaxValue;

        private float effectDuration;

        private ElementType effectElement;

        private bool _readAsFloat;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public TriggerType TriggerType { get => applicationType; set => SetProperty(ref applicationType, value); }

        [JsonProperty]
        public float TriggerValue { get => applicationTriggerValue; set => SetProperty(ref applicationTriggerValue, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType Restriction { get => restriction; set => SetProperty(ref restriction, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public AbilityTargetType EffectTargetType { get => effectTargetType; set => SetProperty(ref effectTargetType, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public EffectType EffectType { get => effectType; set => SetProperty(ref effectType, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType StatBonusType { get => statBonusType; set => SetProperty(ref statBonusType, value); }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType StatModifyType { get => statModifyType; set => SetProperty(ref statModifyType, value); }

        [JsonProperty]
        public float TriggerChance { get => applicationChance; set => SetProperty(ref applicationChance, value); }

        [JsonProperty]
        public float EffectMinValue { get => effectMinValue; set => SetProperty(ref effectMinValue, value); }

        [JsonProperty]
        public float EffectMaxValue { get => effectMaxValue; set => SetProperty(ref effectMaxValue, value); }

        [JsonProperty]
        public float EffectDuration { get => effectDuration; set => SetProperty(ref effectDuration, value); }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ElementType EffectElement { get => effectElement; set => SetProperty(ref effectElement, value); }

        [JsonProperty]
        public bool ReadAsFloat { get => _readAsFloat; set => SetProperty(ref _readAsFloat, value); }

    }

    public class AffixWeight: BindableBase
    {
        public AffixWeight()
        {
        }

        private GroupType _type;

        [JsonProperty]
        private int _weight;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType type { get => _type; set => SetProperty(ref _type, value); }

        [JsonProperty]
        public int weight { get => _weight; set => SetProperty(ref _weight, value); }


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
        MONSTERMOD,
        UNIQUE
    }

    public enum TriggerType
    {
        ON_HIT,
        WHEN_HIT_BY,
        WHEN_HITTING,
        ON_KILL,
        ON_HIT_KILL,
        HEALTH_THRESHOLD,
        SHIELD_THRESHOLD,
        SOULPOINT_THRESHOLD,
        ON_BLOCK,
        ON_DODGE,
        ON_PARRY,
        ON_PHASING,
        ON_DEATH,
    }

    public enum ModifyType
    {
        ADDITIVE,       //all sources add together before modifying
        MULTIPLY,       //all sources multiply together before modifying
        FIXED_TO,            //sets value to modifier value, ignores all other increases
        FLAT_ADDITION   //adds to base before any other calculation
    }
}