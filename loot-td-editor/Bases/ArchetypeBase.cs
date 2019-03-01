﻿using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.Generic;


namespace loot_td
{
    public class ArchetypeBase : BindableBase
    {
        private int id;

        private string name;

        private string text;

        private int stars;

        private int dropLevel;

        private float healthGrowth;

        private float soulPointGrowth;

        private float strengthGrowth;

        private float intelligenceGrowth;

        private float agilityGrowth;

        private float willGrowth;

        [JsonProperty]
        public int Id { get => id; set => SetProperty( ref id , value ); }

        [JsonProperty]
        public string Name { get => name; set =>  SetProperty( ref name, value); }

        [JsonProperty]
        public string Text { get => text; set =>  SetProperty( ref text, value); }

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
        public List<ArchetypeSkillNode> NodeList { get; }


        public ArchetypeBase()
        {
            Text = "";
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