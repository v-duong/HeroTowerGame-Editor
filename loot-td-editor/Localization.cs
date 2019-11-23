using loot_td;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace loot_td_editor
{
    internal static class Localization
    {
        private static Dictionary<string, string> localization;

        public static string GetLocalizationText_TriggeredEffect(TriggeredEffectProperty triggeredEffect, float value)
        {
            localization.TryGetValue("triggerType." + triggeredEffect.TriggerType.ToString(), out string s);
            string restrictionString = "";
            if (triggeredEffect.Restriction != GroupType.NO_GROUP)
            {
                restrictionString = GetGroupTypeRestriction(triggeredEffect.Restriction);
                string[] split = restrictionString.Split(' ');
                restrictionString = restrictionString.Replace(split[0], split[0].ToLower());
            }

            string effectTypeString;

            switch (triggeredEffect.TriggerType)
            {
                case TriggerType.WHEN_HITTING:
                    effectTypeString = GetBonusTypeString(triggeredEffect.StatBonusType, triggeredEffect.StatModifyType, value, value, GroupType.NO_GROUP).TrimEnd('\n');
                    break;

                default:
                    localization.TryGetValue("effectType.bonusProp." + triggeredEffect.EffectType.ToString(), out effectTypeString);
                    effectTypeString = effectTypeString.Replace("{TARGET}", triggeredEffect.EffectTargetType.ToString());
                    effectTypeString = effectTypeString.Replace("{VALUE}", "(" + triggeredEffect.EffectMinValue + "-" + triggeredEffect.EffectMaxValue + ")");

                    if (triggeredEffect.EffectType != EffectType.BUFF && triggeredEffect.EffectType != EffectType.DEBUFF)
                    {
                        effectTypeString = effectTypeString.Replace("{ELEMENT}", triggeredEffect.EffectElement.ToString());
                    }
                    else
                    {
                        effectTypeString = effectTypeString.Replace("{BONUS}", GetBonusTypeString(triggeredEffect.StatBonusType, triggeredEffect.StatModifyType, value, value, GroupType.NO_GROUP).TrimEnd('\n'));
                    }

                    if (triggeredEffect.EffectDuration > 0)
                    {
                        effectTypeString += " for " + triggeredEffect.EffectDuration.ToString("N1") + "s";
                    }

                    break;
            }

            if (triggeredEffect.TriggerChance < 1)
            {
                s = triggeredEffect.TriggerChance.ToString("P0") + " to " + s;
            }

            s = string.Format(s, restrictionString, effectTypeString) + '\n';

            return s;
        }

        public static string GetBonusTypeString(BonusType b, ModifyType m, float min, float max, GroupType restriction)
        {
            if (localization == null)
                LoadStrings();
            string key = "bonusType." + b.ToString();
            if (localization.TryGetValue(key, out string s))
            {
                if (s == "")
                    s = b.ToString();
            }
            else
                s = b.ToString();

            if (restriction != GroupType.NO_GROUP)
            {
                s = GetGroupTypeRestriction(restriction) + ", " + s;
            }

            string val;
            string sign = " +";
            if (min == max)
            {
                if (min < 0)
                    sign = " ";
                val = min.ToString();
            }
            else
            {
                val = "(" + min + "-" + max + ")";
            }

            switch (m)
            {
                case ModifyType.FLAT_ADDITION:
                    s += sign + val;
                    break;

                case ModifyType.ADDITIVE:
                    s += sign + val + "%";
                    break;

                case ModifyType.MULTIPLY:
                    {
                        if (min == max)
                            s += " x" + (1 + min / 100d).ToString("F2");
                        else
                        {
                            string minString = (1 + min / 100d).ToString("F2");
                            string maxString = (1 + max / 100d).ToString("F2");
                            s += " x(" + minString + "-" + maxString + ")";
                        }

                        break;
                    }

                case ModifyType.FIXED_TO:
                    s += " is " + val;
                    break;
            }

            s += "\n";

            return s;
        }

        public static string GetGroupTypeRestriction(GroupType g)
        {
            if (localization == null)
                LoadStrings();
            string stringId = g.ToString();
            if (localization.TryGetValue("groupType." + stringId + ".restriction", out string value))
            {
                if (value == "")
                    return stringId;

                if (value.Contains("{plural}"))
                    value = value.Replace("{plural}", GetGroupTypePlural(g));
                else if (value.Contains("{single}"))
                    value = value.Replace("{single}", GetGroupType(g));

                return value;
            }
            else
            {
                return stringId;
            }
        }

        public static string GetGroupType(GroupType g)
        {
            if (localization == null)
                LoadStrings();
            string stringId = g.ToString();
            if (localization.TryGetValue("groupType." + stringId, out string value))
            {
                if (value == "")
                    return stringId;
                return value;
            }
            else
            {
                return stringId;
            }
        }

        public static string GetGroupTypePlural(GroupType g)
        {
            if (localization == null)
                LoadStrings();
            string stringId = g.ToString();
            if (localization.TryGetValue("groupType." + stringId + ".plural", out string value))
            {
                if (value == "")
                    return stringId;
                return value;
            }
            else
            {
                return stringId;
            }
        }

        public static string GetLocalizationText(string stringId)
        {
            if (localization == null)
                LoadStrings();
            if (localization.TryGetValue(stringId, out string value))
            {
                if (value == "")
                    return stringId;
                return value;
            }
            else
            {
                return stringId;
            }
        }

        public static void LoadStrings()
        {
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\localization\\common.en-US.json";

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            string json = System.IO.File.ReadAllText(filePath);
            localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}