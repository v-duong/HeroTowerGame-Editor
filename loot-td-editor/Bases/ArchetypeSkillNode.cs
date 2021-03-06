﻿using loot_td_editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace loot_td
{
    public class ArchetypeSkillNode : BindableBase, IStringId
    {
        private int _id;
        private string _idName;
        private int _initialLevel;
        private int _maxLevel;
        private NodeType _type;
        private ObservableCollection<ScalingBonusProperty_Int> _bonuses;
        private string _ability;
        private Vector2 _nodePosition;
        private string _iconPath;
        private bool _hasError;
        private ObservableCollection<int> _requirements;
        private ObservableCollection<ArchetypeSkillNode> _requirements2;
        private ObservableCollection<TriggeredEffectProperty> addedEffectBonuses;

        [JsonProperty]
        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                RaisePropertyChanged("NumIdName");
            }
        }

        [JsonProperty]
        public string IdName { get => _idName; set { SetProperty(ref _idName, value); RaisePropertyChanged("NumIdName"); } }

        [JsonProperty]
        public int InitialLevel { get => _initialLevel; set => SetProperty(ref _initialLevel, value); }

        [JsonProperty]
        public int MaxLevel { get => _maxLevel; set => SetProperty(ref _maxLevel, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public NodeType Type { get => _type; set => SetProperty(ref _type, value); }

        [JsonProperty]
        public ObservableCollection<ScalingBonusProperty_Int> Bonuses { get => _bonuses; set => SetProperty(ref _bonuses, value); }

        [JsonProperty]
        public string AbilityId { get => _ability; set => SetProperty(ref _ability, value); }

        [JsonProperty]
        public Vector2 NodePosition { get => _nodePosition; set => SetProperty(ref _nodePosition, value); }

        [JsonProperty]
        public string IconPath { get => _iconPath; set => SetProperty(ref _iconPath, value); }

        [JsonProperty]
        public ObservableCollection<int> Children { get => _requirements; set => SetProperty(ref _requirements, value); }

        [JsonProperty]
        public ObservableCollection<ArchetypeSkillNode> ChildrenEditor { get => _requirements2; set => SetProperty(ref _requirements2, value); }

        [JsonProperty]
        public ObservableCollection<TriggeredEffectProperty> TriggeredEffects { get => addedEffectBonuses; set => SetProperty(ref addedEffectBonuses, value); }

        [JsonIgnore]
        public string NumIdName { get => Id + " " + IdName; }

        [JsonIgnore]
        public bool HasError { get => _hasError; set => SetProperty(ref _hasError, value); }

        [JsonIgnore]
        public string BonusString => GetBonusString();

        public ArchetypeSkillNode()
        {
            IdName = "";
            NodePosition = new Vector2(0, 0);
            Children = new ObservableCollection<int>();
            ChildrenEditor = new ObservableCollection<ArchetypeSkillNode>();
            Bonuses = new ObservableCollection<ScalingBonusProperty_Int>();
            TriggeredEffects = new ObservableCollection<TriggeredEffectProperty>();
            IconPath = "";
            MaxLevel = 1;
            HasError = false;
        }

        public string GetStringId()
        {
            return this.IdName;
        }

        private string GetBonusString()
        {
            string s = "";
            foreach (ScalingBonusProperty_Int b in Bonuses)
            {
                float val;
                if (MaxLevel > 1)
                    val = b.growthValue * (MaxLevel - 1) + b.finalLevelValue;
                else
                    val = b.growthValue;
                s += Localization.GetBonusTypeString(b.bonusType, b.modifyType, val, val, b.restriction);
            }

            foreach(TriggeredEffectProperty added in TriggeredEffects)
            {
                s += Localization.GetLocalizationText_TriggeredEffect(added, added.EffectMinValue);
            }

            return s;
        }
    }

    public class ScalingBonusProperty_Int : BindableBase
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType bonusType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType modifyType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType restriction { get; set; }

        [JsonProperty]
        public float growthValue { get; set; }

        [JsonProperty]
        public float finalLevelValue { get; set; }

        private float _sum;
        private float _perpoint;

        [JsonIgnore]
        public float sum { get => _sum; set => SetProperty(ref _sum, value); }

        [JsonIgnore]
        public float perPoint { get => _perpoint; set => SetProperty(ref _perpoint, value); }
    }

    public class ScalingBonusProperty_Float
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType bonusType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType modifyType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public GroupType restriction { get; set; }

        [JsonProperty]
        public float initialValue { get; set; }

        [JsonProperty]
        public float growthValue { get; set; }
    }

    public enum NodeType
    {
        LESSER,
        GREATER,
        MASTER,
        ABILITY
    }

    public class NodeLevel
    {
        public int level;
        public int bonusLevels;
    }

    public class Vector2 : BindableBase
    {
        [JsonProperty]
        public int x { get => _x; set => SetProperty(ref _x, value); }

        [JsonProperty]
        public int y { get => _y; set => SetProperty(ref _y, value); }

        private int _x;
        private int _y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}