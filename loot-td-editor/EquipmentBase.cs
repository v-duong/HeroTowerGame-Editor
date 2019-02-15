using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism;
using Prism.Mvvm;

namespace loot_td
{
    public class EquipmentBase : BindableBase
    {
        private int _id;
        private string _name;
        private int _willReq;
        private int _agilityReq;
        private int _intelligenceReq;
        private int _strengthReq;
        private float _weaponRange;
        private int _armor;
        private int _shield;
        private int _dodge;
        private int _magicDodge;
        private float _regen;
        private int _minDamage;
        private int _maxDamage;
        private float _criticalChance;
        private EquipSlotType _equipSlot;
        private GroupType _group;
        private bool _hasInnate;
        private int _innateAffixId;
        private int _dropLevel;

        [JsonProperty]
        public int Id { get => _id; set => SetProperty(ref _id, value); }
        [JsonProperty]
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        [JsonProperty]
        public int DropLevel { get => _dropLevel; set => SetProperty(ref _dropLevel , value); }
        [JsonProperty]
        public int Armor { get => _armor; set => SetProperty(ref _armor, value); }
        [JsonProperty]
        public int Shield { get => _shield; set => SetProperty(ref _shield, value); }
        [JsonProperty]
        public int Dodge { get => _dodge; set => SetProperty(ref _dodge, value); }
        [JsonProperty]
        public int MagicDodge { get => _magicDodge; set => SetProperty(ref _magicDodge, value); }
        [JsonProperty]
        public float Regen { get => _regen; set => SetProperty(ref _regen, value); }
        [JsonProperty]
        public int MinDamage { get => _minDamage; set => SetProperty(ref _minDamage, value); }
        [JsonProperty]
        public int MaxDamage { get => _maxDamage; set => SetProperty(ref _maxDamage, value); }
        [JsonProperty]
        public float CriticalChance { get => _criticalChance; set => SetProperty(ref _criticalChance, value); }
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
        public int InnateAffixId { get => _innateAffixId; set => SetProperty(ref _innateAffixId, value); }

        public EquipmentBase()
        {

        }

        public EquipmentBase(EquipmentBase a)
        {
            Id = a.Id;
            Name = a.Name;
            DropLevel = a.DropLevel;
            Armor = a.Armor;
            Shield = a.Shield;
            Dodge = a.Dodge;
            MagicDodge = a.MagicDodge;
            Regen = a.Regen;
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