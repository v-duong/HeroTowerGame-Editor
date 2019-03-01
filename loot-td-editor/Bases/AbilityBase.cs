using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class AbilityBase
{
    [JsonProperty]
    public int id;
    [JsonProperty]
    public string name;
    [JsonConverter(typeof(StringEnumConverter))][JsonProperty]
    public AbilityType abilityType;
    [JsonConverter(typeof(StringEnumConverter))][JsonProperty]
    public AbilityShotType abilityShotType;
    [JsonProperty]
    public float baseCooldown;
    [JsonProperty]
    public float baseTargetRange;
    [JsonProperty]
    public float baseProjectileSpeed;
    [JsonProperty]
    public float baseProjectileSize;
    [JsonProperty]
    public float baseWeaponMultiplier;
    [JsonProperty]
    public float weaponMultiplierScaling;
    [JsonProperty]
    public List<AbilityBaseDamageEntry> baseDamage;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<GroupType> groupTypes;
}

[Serializable]
public class AbilityBaseDamageEntry
{
    [JsonConverter(typeof(StringEnumConverter))][JsonProperty]
    public ElementType elementType;
    [JsonProperty]
    public int baseMin;
    [JsonProperty]
    public int baseMax;
}


public enum AbilityType
{
    ATTACK,
    SPELL,
    AURA,
    BUFF
}

public enum AbilityShotType
{
    PROJECTILE,
    PROJECTILE_NOVA,
    HITSCAN,
    RADIAL_AOE,
    LINEAR_AOE
}
