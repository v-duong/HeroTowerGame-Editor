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
        private int sceneAct;
        private int sceneStage;
        private DifficultyType difficulty;
        private int stageLevel;
        private int baseExperience;
        private ObservableCollection<WeightBase> equipmentDropList;
        private ObservableCollection<WeightBase> archetypeDropList;
        private ObservableCollection<string> stageProperties;
        private float expMultiplier;
        private int equipmentDropCountMin;
        private int equipmentDropCountMax;
        private int consumableDropCountMin;
        private int consumableDropCountMax;
        private ObservableCollection<EnemyWave> enemyWaves;
        private ObservableCollection<SpawnerInfo> spawnerInfos;
        private string requiredToUnlock;

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
        public ObservableCollection<string> StageProperties { get => stageProperties; set => SetProperty(ref stageProperties, value); }

        [JsonProperty]
        public float ExpMultiplier { get => expMultiplier; set => SetProperty(ref expMultiplier, value); }

        [JsonProperty]
        public int EquipmentDropCountMin { get => equipmentDropCountMin; set => SetProperty(ref equipmentDropCountMin, value); }

        [JsonProperty]
        public int EquipmentDropCountMax { get => equipmentDropCountMax; set => SetProperty(ref equipmentDropCountMax, value); }

        [JsonProperty]
        public int ConsumableDropCountMin { get => consumableDropCountMin; set => SetProperty(ref consumableDropCountMin, value); }

        [JsonProperty]
        public int ConsumableDropCountMax { get => consumableDropCountMax; set => SetProperty(ref consumableDropCountMax, value); }

        [JsonProperty]
        public ObservableCollection<EnemyWave> EnemyWaves { get => enemyWaves; set => SetProperty(ref enemyWaves, value); }

        [JsonProperty]
        public ObservableCollection<SpawnerInfo> SpawnerInfos { get => spawnerInfos; set => SetProperty(ref spawnerInfos, value); }

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public int Act { get => act; set => SetProperty(ref act, value); }

        [JsonProperty]
        public int Stage { get => stage; set => SetProperty(ref stage, value); }

        [JsonProperty]
        public int SceneAct { get => sceneAct; set => SetProperty(ref sceneAct, value); }

        [JsonProperty]
        public int SceneStage { get => sceneStage; set => SetProperty(ref sceneStage, value); }

        [JsonProperty]
        public string RequiredToUnlock { get => requiredToUnlock; set => SetProperty(ref requiredToUnlock, value); }

        public StageInfoCollection()
        {
            MonsterLevel = 1;
            BaseExperience = 0;
            ExpMultiplier = 1;
            EquipmentDropCountMin = 1;
            consumableDropCountMin = 1;
            EquipmentDropList = new ObservableCollection<WeightBase>();
            ArchetypeDropList = new ObservableCollection<WeightBase>();
            StageProperties = new ObservableCollection<string>();
            EnemyWaves = new ObservableCollection<EnemyWave>();
            SpawnerInfos = new ObservableCollection<SpawnerInfo>();
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
        private ObservableCollection<string> bonusProperties;
        private ObservableCollection<string> abilityOverrides;
        private bool isBossOverride;
        private int enemyCount;
        private int spawnerIndex; // which spawn point to spawn from
        private int goalIndex;
        private float startDelay;

        [JsonProperty]
        public string EnemyName { get => enemyType; set => SetProperty(ref enemyType, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public RarityType EnemyRarity { get => enemyRarity; set => SetProperty(ref enemyRarity, value); }

        [JsonProperty]
        public ObservableCollection<string> BonusProperties { get => bonusProperties; set => SetProperty(ref bonusProperties, value); }

        [JsonProperty]
        public ObservableCollection<string> AbilityOverrides { get => abilityOverrides; set => SetProperty(ref abilityOverrides, value); }

        [JsonProperty]
        public bool IsBossOverride { get => isBossOverride; set => SetProperty(ref isBossOverride, value); }

        [JsonProperty]
        public float StartDelay { get => startDelay; set => SetProperty(ref startDelay, value); }

        [JsonProperty]
        public int EnemyCount { get => enemyCount; set => SetProperty(ref enemyCount, value); }

        [JsonProperty]
        public int SpawnerIndex { get => spawnerIndex; set => SetProperty(ref spawnerIndex, value); }

        [JsonProperty]
        public int GoalIndex { get => goalIndex; set => SetProperty(ref goalIndex, value); }

        public EnemyWaveItem()
        {
            BonusProperties = new ObservableCollection<string>();
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

    public class SpawnerInfo : BindableBase
    {
        private int spawner;
        private ObservableCollection<int> possibleGoals;

        [JsonProperty]
        public int Spawner { get => spawner; set => SetProperty(ref spawner, value); }

        [JsonProperty]
        public ObservableCollection<int> PossibleGoals { get => possibleGoals; set => SetProperty(ref possibleGoals, value); }

        public SpawnerInfo(int i)
        {
            spawner = i;
            possibleGoals = new ObservableCollection<int>();
        }
    }
}