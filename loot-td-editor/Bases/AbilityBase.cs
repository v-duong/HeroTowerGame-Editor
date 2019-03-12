using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace loot_td
{

    [Serializable]
    public class AbilityBase : BindableBase
    {
        private int id;
        private string idName;
        private string name;
        private string description;
        private AbilityType abilityType;
        private AbilityShotType abilityShotType;
        private float cooldown;
        private float targetRange;
        private AbilityTargetType targetsAllies;

        private float projectileSpeed;
        private float projectileSize;
        private int projectileCount;

        private float areaRadius;
        private float areaLength;

        private float hitscanDelay;

        private float weaponMultiplier;
        private float weaponMultiplierScaling;
        private List<AbilityDamageBase> damageLevels;
        private float flatDamageMultiplier;
        private float baseAbilityPower;
        private float abilityScaling;

        private List<GroupType> groupTypes;
        private List<GroupType> weaponRestrictions;
        private List<ScalingBonusProperty> bonusProperties;
        private string effectSprite;
        private LinkedAbilityData linkedAbility;
        private List<AbilityEffectData> appliedEffects;

        [JsonProperty]
        public int Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public string Name { get => name; set => SetProperty(ref name, value); }

        [JsonProperty]
        public string Description { get => description; set => SetProperty(ref description, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityType AbilityType { get => abilityType; set => SetProperty(ref abilityType, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityShotType AbilityShotType { get => abilityShotType; set => SetProperty(ref abilityShotType, value); }

        [JsonProperty]
        public float Cooldown { get => cooldown; set => SetProperty(ref cooldown, value); }

        [JsonProperty]
        public float TargetRange { get => targetRange; set => SetProperty(ref targetRange, value); }

        [JsonProperty]
        public AbilityTargetType TargetsAllies { get => targetsAllies; set => SetProperty(ref targetsAllies, value); }

        [JsonProperty]
        public float ProjectileSpeed { get => projectileSpeed; set => SetProperty(ref projectileSpeed, value); }

        [JsonProperty]
        public float ProjectileSize { get => projectileSize; set => SetProperty(ref projectileSize, value); }

        [JsonProperty]
        public int ProjectileCount { get => projectileCount; set => SetProperty(ref projectileCount, value); }

        [JsonProperty]
        public float AreaRadius { get => areaRadius; set => SetProperty(ref areaRadius, value); }

        [JsonProperty]
        public float AreaLength { get => areaLength; set => SetProperty(ref areaLength, value); }

        [JsonProperty]
        public float HitscanDelay { get => hitscanDelay; set => SetProperty(ref hitscanDelay, value); }

        [JsonProperty]
        public float WeaponMultiplier { get => weaponMultiplier; set => SetProperty(ref weaponMultiplier, value); }

        [JsonProperty]
        public float WeaponMultiplierScaling { get => weaponMultiplierScaling; set => SetProperty(ref weaponMultiplierScaling, value); }

        [JsonProperty]
        public List<AbilityDamageBase> DamageLevels { get => damageLevels; set => SetProperty(ref damageLevels, value); }

        [JsonProperty]
        public float FlatDamageMultiplier { get => flatDamageMultiplier; set => SetProperty(ref flatDamageMultiplier, value); }

        [JsonProperty]
        public float BaseAbilityPower { get => baseAbilityPower; set => SetProperty(ref baseAbilityPower, value); }

        [JsonProperty]
        public float AbilityScaling { get => abilityScaling; set => SetProperty(ref abilityScaling, value); }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<GroupType> GroupTypes { get => groupTypes; set => SetProperty(ref groupTypes, value); }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<GroupType> WeaponRestrictions { get => weaponRestrictions; set => SetProperty(ref weaponRestrictions, value); }

        [JsonProperty]
        public List<ScalingBonusProperty> BonusProperties { get => bonusProperties; set => SetProperty(ref bonusProperties, value); }

        [JsonProperty]
        public string EffectSprite { get => effectSprite; set => SetProperty(ref effectSprite, value); }

        [JsonProperty]
        public LinkedAbilityData LinkedAbility { get => linkedAbility; set => SetProperty(ref linkedAbility, value); }

        [JsonProperty]
        public List<AbilityEffectData> AppliedEffects { get => appliedEffects; set => SetProperty(ref appliedEffects, value); }

        public AbilityBase()
        {
            DamageLevels = new List<AbilityDamageBase>();
            GroupTypes = new List<GroupType>();
            WeaponRestrictions = new List<GroupType>();
            BonusProperties = new List<ScalingBonusProperty>();
            AppliedEffects = new List<AbilityEffectData>();
            effectSprite = "";
            Description = "";
        }

    }

    [Serializable]
    public class AbilityDamageBase : BindableBase
    {
        private ElementType elementType;
        private List<Vector2> damage;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ElementType ElementType { get => elementType; set => SetProperty(ref elementType, value); }

        [JsonProperty]
        public List<Vector2> Damage { get => damage; set => SetProperty(ref damage, value); }
    }

    [Serializable]
    public class LinkedAbilityData : BindableBase
    {
        private string abilityId;
        private AbilityLinkType type;
        private float time;

        [JsonProperty]
        public string AbilityId { get => abilityId; set => SetProperty(ref abilityId, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityLinkType Type { get => type; set => SetProperty(ref type, value); }

        [JsonProperty]
        public float Time { get => time; set => SetProperty(ref time, value); }
    }

    [Serializable]
    public class AbilityEffectData : BindableBase
    {
        private EffectType effect;
        private float chanceToApply;
        private float value1;

        // private float value2;
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EffectType Effect { get => effect; set => SetProperty(ref effect, value); }

        [JsonProperty]
        public float ChanceToApply { get => chanceToApply; set => SetProperty(ref chanceToApply, value); }

        [JsonProperty]
        public float EffectPower { get => value1; set => SetProperty(ref value1, value); }

        //[JsonProperty]
        //public float Value2 { get => value2; set => SetProperty( ref value2, value); }
    }

    public enum AbilityType
    {
        ATTACK,
        SPELL,
        AURA,
        SELF_BUFF,
    }

    public enum DamageType
    {
        DIRECT,
        DOT,
        PURE
    }

    public enum AbilityLinkType
    {
        ON_HIT,
        TIME,
        ON_FADE,
    }

    public enum AbilityLinkInheritType
    {
        NO_INHERITANCE,
        INHERIT_DAMAGE
    }

    public enum AbilityShotType
    {
        PROJECTILE,
        HITSCAN,
        RADIAL_AOE,
        LINEAR_AOE
    }

    public enum AbilityTargetType
    {
        ENEMY,
        ALLY,
        ALL,
        NONE
    }
}