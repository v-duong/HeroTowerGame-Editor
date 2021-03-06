﻿using loot_td_editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

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
        public int MinDamage { get => _minDamage; set { SetProperty(ref _minDamage, value); RaisePropertyChanged("PotentialDps"); } }

        [JsonProperty]
        public int MaxDamage { get => _maxDamage; set { SetProperty(ref _maxDamage, value); RaisePropertyChanged("PotentialDps"); } }

        [JsonProperty]
        public float CriticalChance { get => _criticalChance; set => SetProperty(ref _criticalChance, value); }

        [JsonProperty]
        public float AttackSpeed { get => _attackSpeed; set { SetProperty(ref _attackSpeed, value); RaisePropertyChanged("PotentialDps"); } }

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

        [JsonIgnore]
        public string PotentialDps { get => CalcPotentialDps(); }

        public EquipmentBase()
        {
            Description = "";
            IdName = "";
        }

        public string CalcPotentialDps()
        {
            float additionalMin, additionalMax, multiplier, speedMulti;

            switch (Group)
            {
                case GroupType.ONE_HANDED_SWORD:
                case GroupType.ONE_HANDED_AXE:
                case GroupType.ONE_HANDED_MACE:
                case GroupType.ONE_HANDED_GUN:
                case GroupType.WAND:
                case GroupType.CROSSBOW:
                case GroupType.FIST:
                    additionalMax = 85;
                    additionalMin = 43;
                    multiplier = 3.00f;
                    speedMulti = 1.40f;
                    break;
                case GroupType.BOW:
                case GroupType.SPEAR:
                case GroupType.STAFF:
                case GroupType.TWO_HANDED_SWORD:
                case GroupType.TWO_HANDED_MACE:
                case GroupType.TWO_HANDED_GUN:
                case GroupType.TWO_HANDED_AXE:
                    additionalMax = 140;
                    additionalMin = 85;
                    multiplier = 3.55f;
                    speedMulti = 1.27f;
                    break;
                default:
                    additionalMax = 0;
                    additionalMin = 0;
                    multiplier = 0;
                    speedMulti = 0;
                    break;
            }
            float min = (MinDamage + additionalMin) * multiplier;
            float max = (MaxDamage + additionalMax) * multiplier;
            float min2 = (MinDamage + additionalMin) * (multiplier + 0.55f);
            float max2 = (MaxDamage + additionalMax) * (multiplier + 0.55f);
            float damageAvg = (min + max) / 2f;
            float damageAvg2 = (min2 + max2) / 2f;
            return (damageAvg * AttackSpeed * speedMulti).ToString("N2") + "    " + (damageAvg2 * AttackSpeed * speedMulti).ToString("N2");
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

    public class UniqueBase : EquipmentBase
    {
        private ObservableCollection<AffixBase> fixedUniqueAffixes;

        private ObservableCollection<AffixBase> randomUniqueAffixes;

        private int randomAffixesToSpawn;

        private int uniqueVersion;

        [JsonProperty]
        public ObservableCollection<AffixBase> FixedUniqueAffixes { get => fixedUniqueAffixes; set => SetProperty(ref fixedUniqueAffixes, value); }

        [JsonProperty]
        public ObservableCollection<AffixBase> RandomUniqueAffixes { get => randomUniqueAffixes; set => SetProperty(ref randomUniqueAffixes, value); }

        [JsonProperty]
        public int RandomAffixesToSpawn { get => randomAffixesToSpawn; set => SetProperty(ref randomAffixesToSpawn, value); }

        [JsonProperty]
        public int UniqueVersion { get => uniqueVersion; set => SetProperty(ref uniqueVersion, value); }

        [JsonIgnore]
        public string ItemAffixString => BuildItemAffixString();

        private string BuildItemAffixString()
        {
            string s = "";
            foreach (AffixBase affixBase in FixedUniqueAffixes)
            {
                s += affixBase.GetAffixString();
            }
            return s;
        }

        public void RaisePropertyAffixString_(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("ItemAffixString");
        }

        public void RaisePropertyAffixString(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= RaisePropertyAffixString_;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += RaisePropertyAffixString_;
                }
            }
        }

        public UniqueBase()
        {
            FixedUniqueAffixes = new ObservableCollection<AffixBase>();
            RandomUniqueAffixes = new ObservableCollection<AffixBase>();
            uniqueVersion = 0;
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