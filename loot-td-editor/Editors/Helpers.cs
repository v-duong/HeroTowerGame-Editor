using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace loot_td_editor
{
    public static class Helpers
    {
        public static double SCALING_FACTOR = 1.042;
        public static double LEVEL_SCALING_FACTOR = 0.402;
        public static double ENEMY_SCALING = 1.012;

        public static T DeepClone<T>(T o)
        {
            string s = JsonConvert.SerializeObject(o);
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static double EquipScalingFormula(double level)
        {
            // formula
            // Scaling^(level/2) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(SCALING_FACTOR, level / 2) * level * LEVEL_SCALING_FACTOR + level * 2;

            return ret1;
        }

        public static double EnemyScalingFormula(double level)
        {
            // formula
            // (Scaling*(EnemyScaling))^(level/1.5 - 22) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(SCALING_FACTOR * ENEMY_SCALING, level / 1.5 - 23) * level * LEVEL_SCALING_FACTOR + level * 2;

            return ret1;
        }

        public static double AbilityScalingFormula(double level, double abilityScaling, double abilityBaseFactor)
        {
            // formula
            // Scaling^(level/2) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(abilityScaling, level / 1.333) * level * LEVEL_SCALING_FACTOR * 2 * abilityBaseFactor;

            return ret1;
        }

        public static bool ErrorCheckArchetypes(List<ArchetypeBase> list, ErrorLog errorLog)
        {
            bool flag = true;
            HashSet<string> archetypeName = new HashSet<string>();
            HashSet<string> nodeName = new HashSet<string>();
            HashSet<string> nodePosition = new HashSet<string>();
            HashSet<int> nodeIdHash = new HashSet<int>();
            foreach (ArchetypeBase a in list)
            {
                if (a.IdName == "" || a.IdName == null)
                {
                    errorLog.Add("Archetype has null IdName");
                    flag = false;
                } 
                if (a.IdName == "UNTITLED NEW")
                {
                    errorLog.Add("Archetype has default IdName");
                    flag = false;
                }
                if (!archetypeName.Add(a.IdName))
                {
                    errorLog.Add("Archetype has duplicate IdName: " + a.IdName);
                    flag = false;
                }

                nodeName.Clear();
                nodePosition.Clear();
                nodeIdHash.Clear();
                foreach (ArchetypeSkillNode n in a.NodeList)
                {
                    if (n.IdName == "" || n.IdName == null)
                    {
                        errorLog.Add("ArchetypeNode in " + a.IdName  + " has null IdName");
                        flag = false;
                    }
                    if (n.IdName == "UNTITLED NEW")
                    {
                        errorLog.Add("ArchetypeNode in " + a.IdName + " has default IdName");
                        flag = false;
                    }
                    if (!nodeName.Add(n.IdName))
                    {
                        errorLog.Add("ArchetypeNode in " + a.IdName + " has duplicate IdName: " + n.IdName);
                        flag = false;
                    }

                    if (!nodeIdHash.Add(n.Id))
                    {
                        errorLog.Add("ArchetypeNode in " + a.IdName + " has duplicate Id: " + n.IdName);
                        flag = false;
                    }

                    if (!nodePosition.Add(n.NodePosition.x + "," + n.NodePosition.y))
                    {
                        errorLog.Add("ArchetypeNode in " + a.IdName + " has overlapping nodes: " + n.IdName);
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static bool ErrorCheckAffixes(List<AffixBase> list, ErrorLog errorLog)
        {
            bool flag = true;
            HashSet<string> affixNameHash = new HashSet<string>();
            foreach (AffixBase a in list)
            {
                if (a.IdName == "" || a.IdName == null)
                {
                    errorLog.Add("Affix has null IdName");
                    flag = false;
                }
                if (a.IdName == "UNTITLED NEW")
                {
                    errorLog.Add("Affix has default IdName");
                    flag = false;
                }
                if (!affixNameHash.Add(a.IdName))
                {
                    errorLog.Add("Affix has duplicate IdName: " + a.IdName);
                    flag = false;
                }
            }
            return flag;
        }

        public static bool ErrorCheckAbilities(List<AbilityBase> list, ErrorLog errorLog)
        {
            bool flag = true;
            HashSet<string> abilityNameHash = new HashSet<string>();
            foreach (AbilityBase a in list)
            {
                if (a.IdName == "" || a.IdName == null)
                {
                    errorLog.Add("Ability has null IdName");
                    flag = false;
                }
                if (a.IdName == "UNTITLED NEW")
                {
                    errorLog.Add("Ability has default IdName");
                    flag = false;
                }
                if (!abilityNameHash.Add(a.IdName))
                {
                    errorLog.Add("Ability has duplicate IdName: " + a.IdName);
                    flag = false;
                }
            }
            return flag;
        }

        public static bool ErrorCheckEquipment(List<EquipmentBase> list, ErrorLog errorLog)
        {
            bool flag = true;
            HashSet<string> nameHash = new HashSet<string>();
            foreach (EquipmentBase a in list)
            {
                if (a.IdName == "" || a.IdName == null)
                {
                    errorLog.Add("Equipment has null IdName");
                    flag = false;
                }
                if (a.IdName == "UNTITLED NEW")
                {
                    errorLog.Add("Equipment has default IdName");
                    flag = false;
                }
                if (!nameHash.Add(a.IdName))
                {
                    errorLog.Add("Equipment has duplicate IdName: " + a.IdName);
                    flag = false;
                }
            }
            return flag;
        }

        public static bool ErrorCheckEnemies(List<EnemyBase> list, ErrorLog errorLog)
        {
            bool flag = true;
            HashSet<string> nameHash = new HashSet<string>();
            foreach (EnemyBase a in list)
            {
                if (a.IdName == "" || a.IdName == null)
                {
                    errorLog.Add("Enemy has null IdName");
                    flag = false;
                }
                if (a.IdName == "UNTITLED NEW")
                {
                    errorLog.Add("Enemy has default IdName");
                    flag = false;
                }
                if (!nameHash.Add(a.IdName))
                {
                    errorLog.Add("Enemy has duplicate IdName: " + a.IdName);
                    flag = false;
                }
            }
            return flag;
        }

        public class ErrorLog
        {
            public Dictionary<string, int> dict = new Dictionary<string, int>();

            public int Count => dict.Count;

            public void Add(string s)
            {
                if (dict.ContainsKey(s))
                {
                    dict[s]++;
                } else
                {
                    dict.Add(s, 1);
                }
            }

            public void Clear()
            {
                dict.Clear();
            }
        }
    }
}

[SuppressUnmanagedCodeSecurity]
internal static class SafeNativeMethods
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    public static extern int StrCmpLogicalW(string psz1, string psz2);
}

public sealed class NaturalStringComparer : IComparer
{
    public int Compare(object a, object b)
    {
        var lhs = (IStringId)a;
        var rhs = (IStringId)b;
        return SafeNativeMethods.StrCmpLogicalW(lhs.GetStringId(), rhs.GetStringId());
    }

    public int Compare(string x, string y)
    {
        return SafeNativeMethods.StrCmpLogicalW(x, y);
    }

}

public interface IStringId
{
    string GetStringId();
}