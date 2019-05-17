using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace loot_td
{
    public class EnemyBase : BindableBase
    {
        private string idName;
        private int level;
        private int experience;
        private bool isBoss;
        private float healthScaling;
        private float movementSpeed;
        private int[] resistances;
        private EnemyType enemyType;
        private ObservableCollection<EnemyAbilityBase> abilitiesList;
        private string spriteName;
        private int actNumber;

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public int Level { get => level; set => SetProperty(ref level, value); }

        [JsonProperty]
        public int Experience { get => experience; set => SetProperty(ref experience, value); }

        [JsonProperty]
        public bool IsBoss { get => isBoss; set => SetProperty(ref isBoss, value); }

        [JsonProperty]
        public float HealthScaling { get => healthScaling; set => SetProperty(ref healthScaling, value); }

        [JsonProperty]
        public float MovementSpeed { get => movementSpeed; set => SetProperty(ref movementSpeed, value); }

        [JsonProperty]
        public int[] Resistances { get => resistances; set => SetProperty(ref resistances, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EnemyType EnemyType { get => enemyType; set => SetProperty(ref enemyType, value); }

        [JsonProperty]
        public ObservableCollection<EnemyAbilityBase> AbilitiesList { get => abilitiesList; set => SetProperty(ref abilitiesList, value); }

        [JsonProperty]
        public string SpriteName { get => spriteName; set => SetProperty(ref spriteName, value); }

        [JsonProperty]
        public int ActNumber { get => actNumber; set => SetProperty(ref actNumber, value); }

        public EnemyBase()
        {
            IdName = "";
            level = 1;
            experience = 1;
            IsBoss = false;
            healthScaling = 1f;
            movementSpeed = 2.5f;
            resistances = new int[(int)ElementType.COUNT];
            abilitiesList = new ObservableCollection<EnemyAbilityBase>();
            spriteName = "";
            actNumber = 0;
        }
    }

    public enum EnemyType
    {
        NON_ATTACKER,
        ATTACKER,
        HIT_AND_RUN
    }

    public class EnemyAbilityBase : BindableBase
    {
        private string abilityName;
        private float damageMultiplier;
        private float cooldownMultiplier;

        public string AbilityName { get => abilityName; set => SetProperty(ref abilityName, value); }
        public float DamageMultiplier { get => damageMultiplier; set => SetProperty(ref damageMultiplier, value); }
        public float CooldownMultiplier { get => cooldownMultiplier; set => SetProperty(ref cooldownMultiplier, value); }

        public EnemyAbilityBase()
        {
            abilityName = "";
            damageMultiplier = 1f;
            cooldownMultiplier = 1f;
        }
    }

    public class EnemyResistanceBase : BindableBase
    {
        private ElementType element;
        private int resistance;

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ElementType Element { get => element; set => SetProperty(ref element, value); }

        [JsonProperty]
        public int Resistance { get => resistance; set => SetProperty(ref resistance, value); }

        public EnemyResistanceBase(ElementType e, int value)
        {
            element = e;
            resistance = value;
        }
    }
}