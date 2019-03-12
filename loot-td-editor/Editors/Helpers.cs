using Newtonsoft.Json;
using System;

namespace loot_td_editor
{
    public static class Helpers
    {
        public static double SCALING_FACTOR = 1.042;
        public static double LEVEL_SCALING_FACTOR = 0.402;
        public static double ENEMY_SCALING = 1.1;

        public static T DeepClone<T>(T o)
        {
            string s = JsonConvert.SerializeObject(o);
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static double EquipScalingFormula(double level)
        {
            // formula
            // Scaling^(level/2) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(SCALING_FACTOR, level/2) * level * LEVEL_SCALING_FACTOR + level*2;

            return ret1;
        }


        public static double EnemyScalingFormula(double level)
        {
            // formula
            // (Scaling*(EnemyScaling))^(level/1.5 - 22) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(SCALING_FACTOR*ENEMY_SCALING, level / 1.5 - 22) * level * LEVEL_SCALING_FACTOR + level * 2;

            return ret1;
        }

        public static double AbilityScalingFormula(double level, double abilityScaling, double abilityBaseFactor)
        {
            // formula
            // Scaling^(level/2) * (level*levelFactor) + level*2
            double ret1 = Math.Pow(abilityScaling, level / 2) * level * LEVEL_SCALING_FACTOR * 2 * abilityBaseFactor;

            return ret1;
        }
    }
}