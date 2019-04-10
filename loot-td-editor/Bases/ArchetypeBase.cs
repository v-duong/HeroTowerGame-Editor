using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.Generic;


namespace loot_td
{
    public class ArchetypeBase : BindableBase
    {
        private int id;

        private string idName;

        private string name;

        private string text;

        private int stars;

        private int dropLevel;

        private int spawnWeight;

        private float healthGrowth;

        private float soulPointGrowth;

        private float strengthGrowth;

        private float intelligenceGrowth;

        private float agilityGrowth;

        private float willGrowth;


        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public int Stars { get => stars; set =>  SetProperty( ref stars, value); }

        [JsonProperty]
        public int DropLevel { get => dropLevel; set =>  SetProperty( ref dropLevel, value); }

        [JsonProperty]
        public float HealthGrowth { get => healthGrowth; set =>  SetProperty( ref healthGrowth, value); }

        [JsonProperty]
        public float SoulPointGrowth { get => soulPointGrowth; set =>  SetProperty( ref soulPointGrowth, value); }

        [JsonProperty]
        public float StrengthGrowth { get => strengthGrowth; set =>  SetProperty( ref strengthGrowth, value); }

        [JsonProperty]
        public float IntelligenceGrowth { get => intelligenceGrowth; set =>  SetProperty( ref intelligenceGrowth, value); }

        [JsonProperty]
        public float AgilityGrowth { get => agilityGrowth; set =>  SetProperty( ref agilityGrowth, value); }

        [JsonProperty]
        public float WillGrowth { get => willGrowth; set =>  SetProperty( ref willGrowth, value); }

        [JsonProperty]
        public int SpawnWeight { get => spawnWeight; set => SetProperty(ref spawnWeight, value); }

        [JsonProperty]
        public List<ArchetypeSkillNode> NodeList { get; }


        public ArchetypeBase()
        {
            IdName = "";
            NodeList = new List<ArchetypeSkillNode>();
            DropLevel = 1;
            Stars = 1;
            
        }

        public static ArchetypeBase DeepClone(ArchetypeBase a)
        {
            string s = JsonConvert.SerializeObject(a);
            return JsonConvert.DeserializeObject<ArchetypeBase>(s);
        }
    }
}