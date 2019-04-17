using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Windows;
using Prism.Mvvm;

namespace loot_td
{
    public class ArchetypeSkillNode : BindableBase
    {
        private int _id;
        private string _idName;
        private int _initialLevel;
        private int _maxLevel;
        private NodeType _type;
        private List<ScalingBonusProperty> _bonuses;
        private string _ability;
        private Vector2 _nodePosition;
        private string _iconPath;
        private List<int> _requirements;

        [JsonProperty]
        public int Id { get => _id; set =>  SetProperty( ref _id, value); }
        [JsonProperty]
        public string IdName { get => _idName; set => SetProperty(ref _idName, value); }

        [JsonProperty]
        public int InitialLevel { get => _initialLevel; set =>  SetProperty( ref _initialLevel, value); }

        [JsonProperty]
        public int MaxLevel { get => _maxLevel; set =>  SetProperty( ref _maxLevel, value); }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public NodeType Type { get => _type; set =>  SetProperty( ref _type, value); }

        [JsonProperty]
        public List<ScalingBonusProperty> Bonuses { get => _bonuses; set =>  SetProperty( ref _bonuses, value); }

        [JsonProperty]
        public string AbilityId { get => _ability; set =>  SetProperty( ref _ability, value); }

        [JsonProperty]
        public Vector2 NodePosition { get => _nodePosition; set =>  SetProperty( ref _nodePosition, value); }

        [JsonProperty]
        public string IconPath { get => _iconPath; set =>  SetProperty( ref _iconPath, value); }

        [JsonProperty]
        public List<int> Children { get => _requirements; set =>  SetProperty( ref _requirements, value); }

        public ArchetypeSkillNode()
        {
            NodePosition = new Vector2(0,0);
            Children = new List<int>();
            Bonuses = new List<ScalingBonusProperty>();
            IconPath = "";
            MaxLevel = 1;
        }
    }

    public class ScalingBonusProperty
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType bonusType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType modifyType { get; set; }

        [JsonProperty]
        public int initialValue { get; set; }

        [JsonProperty]
        public int growthValue { get; set; }
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