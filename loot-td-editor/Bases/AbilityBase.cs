using loot_td_editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace loot_td
{
    [Serializable]
    public class AbilityBase : BindableBase, IStringId
    {
        private string idName;
        private AbilityType abilityType;
        private AbilityShotType abilityShotType;
        private float cooldown;
        private float targetRange;
        private AbilityTargetType targetsAllies;

        private float projectileSpeed;
        private float projectileSize;
        private int projectileCount;
        private int projectileSpread;
        private bool projectileDoesNotSpread;
        private float projectileLife;
        private float soulCost;
        private float cooldownTime;
        private bool isSoulAbility;

        private float areaRadius;
        private float areaLength;

        private float hitscanDelay;

        private float baseCritical;

        private float weaponMultiplier;
        private float weaponMultiplierScaling;
        private Dictionary<ElementType, AbilityDamageBase> damageLevels;
        private float flatDamageMultiplier;
        private float baseAbilityPower;
        private float abilityScaling;

        private ObservableCollection<GroupType> groupTypes;
        private ObservableCollection<GroupType> weaponRestrictions;
        private ObservableCollection<GroupType> singleRestrictions;
        private ObservableCollection<ScalingBonusProperty_Float> bonusProperties;
        private string effectSprite;
        private LinkedAbilityData linkedAbility;
        private ObservableCollection<AbilityAppliedEffect> appliedEffects;
        private ObservableCollection<TriggeredEffectProperty> triggeredEffects;
        private bool hasLinkedAbility;
        private bool useWeaponRange;
        private bool useWeaponRangeAoe;
        private bool useBothWeaponsForDual;
        private int hitCount;
        private float hitDamageModifier;
        private float delayBetweenHits;

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityType AbilityType { get => abilityType; set => SetProperty(ref abilityType, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityShotType AbilityShotType { get => abilityShotType; set => SetProperty(ref abilityShotType, value); }

        [JsonProperty]
        public float AttacksPerSec
        {
            get => cooldown; set { SetProperty(ref cooldown, value); RaisePropertyChanged("GetDPS"); }
        }

        [JsonProperty]
        public float TargetRange { get => targetRange; set => SetProperty(ref targetRange, value); }

        [JsonProperty]
        public float BaseCritical { get => baseCritical; set => SetProperty(ref baseCritical, value); }

        [JsonProperty]
        public AbilityTargetType TargetType { get => targetsAllies; set => SetProperty(ref targetsAllies, value); }

        [JsonProperty]
        public float ProjectileSpeed { get => projectileSpeed; set => SetProperty(ref projectileSpeed, value); }

        [JsonProperty]
        public float ProjectileSize { get => projectileSize; set => SetProperty(ref projectileSize, value); }

        [JsonProperty]
        public float ProjectileLifespanMulti { get => projectileLife; set => SetProperty(ref projectileLife, value); }

        [JsonProperty]
        public int ProjectileCount { get => projectileCount; set => SetProperty(ref projectileCount, value); }

        [JsonProperty]
        public int ProjectileSpread { get => projectileSpread; set => SetProperty(ref projectileSpread, value); }

        [JsonProperty]
        public bool DoesProjectileSpread { get => projectileDoesNotSpread; set => SetProperty(ref projectileDoesNotSpread, value); }

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
        public bool IsSoulAbility { get => isSoulAbility; set => SetProperty(ref isSoulAbility, value); }

        [JsonProperty]
        public float SoulCost { get => soulCost; set => SetProperty(ref soulCost, value); }

        [JsonProperty]
        public float CooldownTime { get => cooldownTime; set => SetProperty(ref cooldownTime, value); }

        [JsonProperty]
        public Dictionary<ElementType, AbilityDamageBase> DamageLevels { get => damageLevels; set => SetProperty(ref damageLevels, value); }

        [JsonProperty]
        public float FlatDamageMultiplier { get => flatDamageMultiplier; set => SetProperty(ref flatDamageMultiplier, value); }

        [JsonProperty]
        public float BaseAbilityPower { get => baseAbilityPower; set { SetAndCalc(ref baseAbilityPower, value); RaisePropertyChanged("GetDPS"); } }

        [JsonProperty]
        public float AbilityScaling { get => abilityScaling; set { SetAndCalc(ref abilityScaling, value); RaisePropertyChanged("GetDPS"); } }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public ObservableCollection<GroupType> GroupTypes { get => groupTypes; set => SetProperty(ref groupTypes, value); }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public ObservableCollection<GroupType> RequiredRestrictions { get => weaponRestrictions; set => SetProperty(ref weaponRestrictions, value); }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public ObservableCollection<GroupType> SingleRequireRestrictions { get => singleRestrictions; set => SetProperty(ref singleRestrictions, value); }

        [JsonProperty]
        public ObservableCollection<ScalingBonusProperty_Float> BonusProperties { get => bonusProperties; set => SetProperty(ref bonusProperties, value); }

        [JsonProperty]
        public string EffectSprite { get => effectSprite; set => SetProperty(ref effectSprite, value); }

        [JsonProperty]
        public LinkedAbilityData LinkedAbility { get => linkedAbility; set => SetProperty(ref linkedAbility, value); }

        [JsonProperty]
        public ObservableCollection<AbilityAppliedEffect> AppliedEffects { get => appliedEffects; set => SetProperty(ref appliedEffects, value); }

        [JsonProperty]
        public ObservableCollection<TriggeredEffectProperty> TriggeredEffects { get => triggeredEffects; set => SetProperty(ref triggeredEffects, value); }

        [JsonProperty]
        public bool HasLinkedAbility { get => hasLinkedAbility; set => SetProperty(ref hasLinkedAbility, value); }

        [JsonProperty]
        public bool UseWeaponRangeForTargeting { get => useWeaponRange; set => SetProperty(ref useWeaponRange, value); }

        [JsonProperty]
        public bool UseWeaponRangeForAOE { get => useWeaponRangeAoe; set => SetProperty(ref useWeaponRangeAoe, value); }

        [JsonProperty]
        public bool UseBothWeaponsForDual { get => useBothWeaponsForDual; set => SetProperty(ref useBothWeaponsForDual, value); }

        [JsonProperty]
        public int HitCount { get => hitCount; set { SetProperty(ref hitCount, value); RaisePropertyChanged("GetDPS"); } }

        [JsonProperty]
        public float HitDamageModifier { get => hitDamageModifier; set { SetProperty(ref hitDamageModifier, value); RaisePropertyChanged("GetDPS"); } }

        [JsonProperty]
        public float DelayBetweenHits { get => delayBetweenHits; set => SetProperty(ref delayBetweenHits, value); }

        [JsonIgnore]
        public float GetDPS { get => CalcDPS(); }

        public AbilityBase()
        {
            DamageLevels = new Dictionary<ElementType, AbilityDamageBase>();
            GroupTypes = new ObservableCollection<GroupType>();
            RequiredRestrictions = new ObservableCollection<GroupType>();
            SingleRequireRestrictions = new ObservableCollection<GroupType>();
            BonusProperties = new ObservableCollection<ScalingBonusProperty_Float>();
            AppliedEffects = new ObservableCollection<AbilityAppliedEffect>();
            TriggeredEffects = new ObservableCollection<TriggeredEffectProperty>();
            LinkedAbility = new LinkedAbilityData();
            effectSprite = "";
        }

        [JsonIgnore] public AbilityDamageBase GetPhysical { get => GetElementDamage(ElementType.PHYSICAL); set => damageLevels[ElementType.PHYSICAL] = value; }
        [JsonIgnore] public AbilityDamageBase GetFire { get => GetElementDamage(ElementType.FIRE); set => damageLevels[ElementType.FIRE] = value; }
        [JsonIgnore] public AbilityDamageBase GetCold { get => GetElementDamage(ElementType.COLD); set => damageLevels[ElementType.COLD] = value; }
        [JsonIgnore] public AbilityDamageBase GetLightning { get => GetElementDamage(ElementType.LIGHTNING); set => damageLevels[ElementType.LIGHTNING] = value; }
        [JsonIgnore] public AbilityDamageBase GetEarth { get => GetElementDamage(ElementType.EARTH); set => damageLevels[ElementType.EARTH] = value; }
        [JsonIgnore] public AbilityDamageBase GetDivine { get => GetElementDamage(ElementType.DIVINE); set => damageLevels[ElementType.DIVINE] = value; }
        [JsonIgnore] public AbilityDamageBase GetVoid { get => GetElementDamage(ElementType.VOID); set => damageLevels[ElementType.VOID] = value; }

        [JsonIgnore] public bool HasPhysical { get => damageLevels.ContainsKey(ElementType.PHYSICAL); }
        [JsonIgnore] public bool HasFire { get => damageLevels.ContainsKey(ElementType.FIRE); }
        [JsonIgnore] public bool HasCold { get => damageLevels.ContainsKey(ElementType.COLD); }
        [JsonIgnore] public bool HasLightning { get => damageLevels.ContainsKey(ElementType.LIGHTNING); }
        [JsonIgnore] public bool HasEarth { get => damageLevels.ContainsKey(ElementType.EARTH); }
        [JsonIgnore] public bool HasDivine { get => damageLevels.ContainsKey(ElementType.DIVINE); }
        [JsonIgnore] public bool HasVoid { get => damageLevels.ContainsKey(ElementType.VOID); }

        private void SetAndCalc(ref float r, float v)
        {
            SetProperty(ref r, v);
            if (damageLevels.Count == 0)
                return;
            foreach (AbilityDamageBase b in damageLevels.Values)
            {
                if (b == null)
                    continue;
                b.CalculateDamage(BaseAbilityPower, AbilityScaling);
            }
        }

        private AbilityDamageBase GetElementDamage(ElementType e)
        {
            if (damageLevels.ContainsKey(e))
                return damageLevels[e];
            else
                return null;
        }

        public string GetStringId()
        {
            return this.IdName;
        }

        public float CalcDPS()
        {
            float totalMin = 0;
            float totalMax = 0;
            if (abilityType == AbilityType.SPELL)
            {
                foreach (ElementType e in Enum.GetValues(typeof(ElementType)))
                {
                    AbilityDamageBase b = GetElementDamage(e);
                    if (b != null)
                    {
                        totalMin += b.Damage[50].Min;
                        totalMax += b.Damage[50].Max;
                    }
                }

                float dps = (totalMin + totalMax) / 2f * AttacksPerSec * HitCount * HitDamageModifier;
                return dps;
            }
            else
                return 0f;
        }
    }

    [Serializable]
    public class AbilityDamageBase : BindableBase
    {
        private List<DamageStore> damage;
        private float minMult;
        private float maxMult;

        [JsonProperty]
        public List<DamageStore> Damage { get => damage; set => SetProperty(ref damage, value); }

        [JsonProperty]
        public float MinMult { get => minMult; set => SetProperty(ref minMult, value); }

        [JsonProperty]
        public float MaxMult { get => maxMult; set => SetProperty(ref maxMult, value); }

        public AbilityDamageBase()
        {
            MinMult = 1;
            MaxMult = 1;
            damage = new List<DamageStore>();
        }

        public void CalculateDamage(float basePower, float scaling)
        {
            for (int i = 1; i <= 65; i++)
            {
                int j = i * 2;
                //double scalingfactor = Math.Pow(scaling, j/1.333);
                //double levelfactor = 0.804d;
                //double final = scalingfactor * levelfactor * basePower * j + 8d;
                //damage[i] = (new DamageStore( (float)(final * minMult), (float)(final * MaxMult) ));
                double final = Helpers.AbilityScalingFormula(j, scaling, basePower) + 8d;
                if (damage.Count < i)
                {
                    damage.Add(new DamageStore(0, 0));
                }
                damage[i - 1].Min = (int)Math.Round(final * minMult, MidpointRounding.AwayFromZero);
                damage[i - 1].Max = (int)Math.Round(final * MaxMult, MidpointRounding.AwayFromZero);
            }
        }
    }

    [Serializable]
    public class LinkedAbilityData : BindableBase
    {
        private string abilityId;
        private AbilityLinkType type;
        private float time;
        public bool inheritsDamage;
        public float inheritDamagePercent;
        public float inheritDamagePercentScaling;

        [JsonProperty]
        public string AbilityId { get => abilityId; set => SetProperty(ref abilityId, value); }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityLinkType Type { get => type; set => SetProperty(ref type, value); }

        [JsonProperty]
        public float Time { get => time; set => SetProperty(ref time, value); }

        [JsonProperty]
        public bool InheritsDamage { get => inheritsDamage; set => SetProperty(ref inheritsDamage, value); }

        [JsonProperty]
        public float InheritDamagePercent { get => inheritDamagePercent; set => SetProperty(ref inheritDamagePercent, value); }

        [JsonProperty]
        public float InheritDamagePercentScaling { get => inheritDamagePercentScaling; set => SetProperty(ref inheritDamagePercentScaling, value); }
    }

    public class DamageStore : BindableBase
    {
        private int min;
        private int max;

        public int Min { get => min; set => SetProperty(ref min, value); }
        public int Max { get => max; set => SetProperty(ref max, value); }

        public DamageStore(int a, int b)
        {
            Min = a;
            Max = b;
        }
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
        NONE,
        ON_EVERY_HIT,
        ON_FIRST_HIT,
        ON_FINAL_HIT,
        TIME,
        TIME_REPEAT,
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
        HITSCAN_SINGLE,
        HITSCAN_MULTI,
        ARC_AOE,
        FORWARD_MOVING_ARC,
        RADIAL_AOE,
        FORWARD_MOVING_RADIAL,
        NOVA_AOE,
        NOVA_ARC_AOE,
        LINEAR_AOE,
        FORWARD_MOVING_LINEAR,
        PROJECTILE_NOVA,
    }

    public enum AbilityTargetType
    {
        ENEMY,
        ALLY,
        ALL,
        SELF,
        NONE
    }

    [Serializable]
    public class AbilityAppliedEffect
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public EffectType effectType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public BonusType bonusType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public ModifyType modifyType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty]
        public AbilityTargetType targetType { get; set; }

        [JsonProperty]
        public float chance { get; set; }

        [JsonProperty]
        public float initialValue { get; set; }

        [JsonProperty]
        public float growthValue { get; set; }

        [JsonProperty]
        public float duration { get; set; }

        [JsonProperty]
        public int stacks { get; set; }

        [JsonProperty]
        public bool useLastRoll { get; set; }
    }

    public interface IIdName
    {
        string GetIdName();
    }
}