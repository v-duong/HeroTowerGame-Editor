using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;

namespace loot_td
{
    public class EquipmentBase : BindableBase, IStringId
    {
        private string _idName;
        private string _description;
        private int _willReq;
        private int _agilityReq;
        private int _intelligenceReq;
        private int _strengthReq;
        private float _weaponRange;
        private float _attackSpeed;
        private int _armor;
        private int _shield;
        private int _dodge;
        private int _magicDodge;
        private int _regen;
        private int _minDamage;
        private int _maxDamage;
        private float _criticalChance;
        private EquipSlotType _equipSlot;
        private GroupType _group;
        private bool _hasInnate;
        private string _innateAffixId;
        private int _dropLevel;
        private int _spawnWeight;


        [JsonProperty]
        public string IdName { get => _idName; set => SetProperty(ref _idName, value); }

        [JsonProperty]
        public string Description { get => _description; set => SetProperty(ref _description, value); }

        [JsonProperty]
        public int DropLevel { get => _dropLevel; set => SetProperty(ref _dropLevel, value); }

        [JsonProperty]
        public int Armor { get => _armor; set => SetProperty(ref _armor, value); }

        [JsonProperty]
        public int Shield { get => _shield; set => SetProperty(ref _shield, value); }

        [JsonProperty]
        public int DodgeRating { get => _dodge; set => SetProperty(ref _dodge, value); }

        [JsonProperty]
        public int ResolveRating { get => _magicDodge; set => SetProperty(ref _magicDodge, value); }

        [JsonProperty]
        public int SellValue { get => _regen; set => SetProperty(ref _regen, value); }

        [JsonProperty]
        public int MinDamage { get => _minDamage; set => SetProperty(ref _minDamage, value); }

        [JsonProperty]
        public int MaxDamage { get => _maxDamage; set => SetProperty(ref _maxDamage, value); }

        [JsonProperty]
        public float CriticalChance { get => _criticalChance; set => SetProperty(ref _criticalChance, value); }
        [JsonProperty]
        public float AttackSpeed { get => _attackSpeed; set => SetProperty(ref _attackSpeed, value); }

        [JsonProperty]
        public float WeaponRange { get => _weaponRange; set => SetProperty(ref _weaponRange, value); }

        [JsonProperty]
        public int StrengthReq { get => _strengthReq; set => SetProperty(ref _strengthReq, value); }

        [JsonProperty]
        public int IntelligenceReq { get => _intelligenceReq; set => SetProperty(ref _intelligenceReq, value); }

        [JsonProperty]
        public int AgilityReq { get => _agilityReq; set => SetProperty(ref _agilityReq, value); }

        [JsonProperty]
        public int WillReq { get => _willReq; set => SetProperty(ref _willReq, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EquipSlotType EquipSlot { get => _equipSlot; set => SetProperty(ref _equipSlot, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public GroupType Group { get => _group; set => SetProperty(ref _group, value); }

        [JsonProperty]
        public bool HasInnate { get => _hasInnate; set => SetProperty(ref _hasInnate, value); }

        [JsonProperty]
        public string InnateAffixId { get => _innateAffixId; set => SetProperty(ref _innateAffixId, value); }

        [JsonProperty]
        public int SpawnWeight { get => _spawnWeight; set => SetProperty(ref _spawnWeight, value); }

        public EquipmentBase()
        {
            Description = "";
            IdName = "";
        }

        public EquipmentBase(EquipmentBase a)
        {
            IdName = a.IdName;
            Description = a.Description;
            DropLevel = a.DropLevel;
            Armor = a.Armor;
            Shield = a.Shield;
            DodgeRating = a.DodgeRating;
            ResolveRating = a.ResolveRating;
            SellValue = a.SellValue;
            MinDamage = a.MinDamage;
            MaxDamage = a.MaxDamage;
            CriticalChance = a.CriticalChance;
            WeaponRange = a.WeaponRange;
            StrengthReq = a.StrengthReq;
            IntelligenceReq = a.IntelligenceReq;
            AgilityReq = a.AgilityReq;
            WillReq = a.WillReq;
            EquipSlot = a.EquipSlot;
            Group = a.Group;
            InnateAffixId = a.InnateAffixId;
        }

        public string GetStringId()
        {
            return this.IdName;
        }
    }

    public enum EquipSlotType
    {
        WEAPON,
        OFF_HAND,
        BODY_ARMOR,
        HEADGEAR,
        GLOVES,
        BOOTS,
        BELT,
        RING,
        NECKLACE
    }
}