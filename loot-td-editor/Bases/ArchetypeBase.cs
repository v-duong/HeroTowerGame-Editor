using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace loot_td
{
    public class ArchetypeBase : BindableBase, IStringId
    {
        private string idName;
        private int stars;
        private int dropLevel;
        private int spawnWeight;
        private float healthGrowth;
        private float soulPointGrowth;
        private float strengthGrowth;
        private float intelligenceGrowth;
        private float agilityGrowth;
        private float willGrowth;
        private string soulAbilityId;
        private ObservableCollection<ArchetypeSkillNode> nodeList;
        private ObservableCollection<string> infusionAffixes;
        private ObservableCollection<AffixBase> infusionAffixes_edit;

        [JsonProperty]
        public string IdName { get => idName; set => SetProperty(ref idName, value); }

        [JsonProperty]
        public int Stars { get => stars; set => SetProperty(ref stars, value); }

        [JsonProperty]
        public int DropLevel { get => dropLevel; set => SetProperty(ref dropLevel, value); }

        [JsonProperty]
        public float HealthGrowth { get => healthGrowth; set => SetProperty(ref healthGrowth, value); }

        [JsonProperty]
        public float SoulPointGrowth { get => soulPointGrowth; set => SetProperty(ref soulPointGrowth, value); }

        [JsonProperty]
        public float StrengthGrowth { get => strengthGrowth; set => SetProperty(ref strengthGrowth, value); }

        [JsonProperty]
        public float IntelligenceGrowth { get => intelligenceGrowth; set => SetProperty(ref intelligenceGrowth, value); }

        [JsonProperty]
        public float AgilityGrowth { get => agilityGrowth; set => SetProperty(ref agilityGrowth, value); }

        [JsonProperty]
        public float WillGrowth { get => willGrowth; set => SetProperty(ref willGrowth, value); }

        [JsonProperty]
        public string SoulAbilityId{ get => soulAbilityId; set => SetProperty(ref soulAbilityId, value); }

        [JsonProperty]
        public int SpawnWeight { get => spawnWeight; set => SetProperty(ref spawnWeight, value); }

        [JsonProperty]
        public ObservableCollection<ArchetypeSkillNode> NodeList { get => nodeList; set => SetProperty(ref nodeList, value); }
        [JsonProperty]
        public ObservableCollection<string> InfusionAffixes { get => infusionAffixes; set => SetProperty(ref infusionAffixes, value); }
        [JsonIgnore]
        public ObservableCollection<AffixBase> InfusionAffixes_Editor { get => infusionAffixes_edit; set => SetProperty(ref infusionAffixes_edit, value); }

        public ArchetypeBase()
        {
            IdName = "";
            SoulAbilityId = "";
            NodeList = new ObservableCollection<ArchetypeSkillNode>();
            InfusionAffixes = new ObservableCollection<string>();
            InfusionAffixes_Editor = new ObservableCollection<AffixBase>();
            DropLevel = 1;
            Stars = 1;
        }

        public static ArchetypeBase DeepClone(ArchetypeBase a)
        {
            string s = JsonConvert.SerializeObject(a);
            return JsonConvert.DeserializeObject<ArchetypeBase>(s);
        }

        public string GetStringId()
        {
            return this.IdName;
        }
    }
}