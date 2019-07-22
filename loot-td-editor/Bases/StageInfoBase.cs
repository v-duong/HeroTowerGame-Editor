using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace loot_td
{
    public class StageInfoCollection : BindableBase
    {
        private string idName;
        private int act;
        private int stage;
        private DifficultyType difficulty;
        private int stageLevel;
        private int baseExperience;
        private ObservableCollection<WeightBase> equipmentDropList;
        private ObservableCollection<WeightBase> archetypeDropList;
        private ObservableCollection<ScalingBonusProperty> stageProperties;
        private float expMultiplier;
        private float equipmentDropRateMultiplier;
        private float consumableDropRateMultiplier;
        private ObservableCollection<EnemyWave> enemyWaves;

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public DifficultyType Difficulty { get => difficulty; set => SetProperty(ref difficulty, value); }

        [JsonProperty]
        public int MonsterLevel { get => stageLevel; set => SetProperty(ref stageLevel, value); }

        [JsonProperty]
        public int BaseExperience { get => baseExperience; set => SetProperty(ref baseExperience, value); }

        [JsonProperty]
        public ObservableCollection<WeightBase> EquipmentDropList { get => equipmentDropList; set => SetProperty(ref equipmentDropList, value); }

        [JsonProperty]
        public ObservableCollection<WeightBase> ArchetypeDropList { get => archetypeDropList; set => SetProperty(ref archetypeDropList, value); }

        [JsonProperty]
        public ObservableCollection<ScalingBonusProperty> StageProperties { get => stageProperties; set => SetProperty(ref stageProperties, value); }

        [JsonProperty]
        public float ExpMultiplier { get => expMultiplier; set => SetProperty(ref expMultiplier, value); }

        [JsonProperty]
        public float EquipmentDropRateMultiplier { get => equipmentDropRateMultiplier; set => SetProperty(ref equipmentDropRateMultiplier, value); }

        [JsonProperty]
        public float ConsumableDropRateMultiplier { get => consumableDropRateMultiplier; set => SetProperty(ref consumableDropRateMultiplier, value); }

        [JsonProperty]
        public ObservableCollection<EnemyWave> EnemyWaves { get => enemyWaves; set => SetProperty(ref enemyWaves, value); }

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }
        [JsonProperty]
        public int Act { get => act; set => SetProperty(ref act, value); }
        [JsonProperty]
        public int Stage { get => stage; set => SetProperty(ref stage, value); }

        public StageInfoCollection()
        {
            MonsterLevel = 1;
            BaseExperience = 0;
            ExpMultiplier = 1;
            EquipmentDropRateMultiplier = 1;
            consumableDropRateMultiplier = 1;
            EquipmentDropList = new ObservableCollection<WeightBase>();
            ArchetypeDropList = new ObservableCollection<WeightBase>();
            StageProperties = new ObservableCollection<ScalingBonusProperty>();
            EnemyWaves = new ObservableCollection<EnemyWave>();
        }
    }

    public class EnemyWave : BindableBase
    {
        private ObservableCollection<EnemyWaveItem> enemyList; // list of enemy types and number to spawn
        private float delayBetweenSpawns;
        private float delayUntilNextWave;
        private int id;

        [JsonProperty]
        public int Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty]
        public ObservableCollection<EnemyWaveItem> EnemyList { get => enemyList; set => SetProperty(ref enemyList, value); }

        [JsonProperty]
        public float DelayBetweenSpawns { get => delayBetweenSpawns; set => SetProperty(ref delayBetweenSpawns, value); }

        [JsonProperty]
        public float DelayUntilNextWave { get => delayUntilNextWave; set => SetProperty(ref delayUntilNextWave, value); }

        public EnemyWave()
        {
            EnemyList = new ObservableCollection<EnemyWaveItem>();
        }
    }

    public class EnemyWaveItem : BindableBase
    {
        private string enemyType;
        private RarityType enemyRarity;
        private ObservableCollection<ScalingBonusProperty> bonusProperties;
        private ObservableCollection<string> abilityOverrides;
        private bool isBossOverride;
        private int enemyCount;
        private int spawnerIndex; // which spawn point to spawn from
        private int goalIndex;

        [JsonProperty]
        public string EnemyName { get => enemyType; set => SetProperty(ref enemyType, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public RarityType EnemyRarity { get => enemyRarity; set => SetProperty(ref enemyRarity, value); }

        [JsonProperty]
        public ObservableCollection<ScalingBonusProperty> BonusProperties { get => bonusProperties; set => SetProperty(ref bonusProperties, value); }

        [JsonProperty]
        public ObservableCollection<string> AbilityOverrides { get => abilityOverrides; set => SetProperty(ref abilityOverrides, value); }

        [JsonProperty]
        public bool IsBossOverride { get => isBossOverride; set => SetProperty(ref isBossOverride, value); }

        [JsonProperty]
        public int EnemyCount { get => enemyCount; set => SetProperty(ref enemyCount, value); }

        [JsonProperty]
        public int SpawnerIndex { get => spawnerIndex; set => SetProperty(ref spawnerIndex, value); }

        [JsonProperty]
        public int GoalIndex { get => goalIndex; set => SetProperty(ref goalIndex, value); }

        public EnemyWaveItem()
        {
            BonusProperties = new ObservableCollection<ScalingBonusProperty>();
            AbilityOverrides = new ObservableCollection<string>();
        }
    }

    public class WeightBase : BindableBase
    {
        private string idName;
        private int weight;

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public int Weight { get => weight; set => SetProperty(ref weight, value); }
    }
}