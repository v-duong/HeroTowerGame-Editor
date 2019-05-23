using loot_td;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace loot_td_editor.Editors
{
    public partial class StageEditor : UserControl
    {
        public ObservableCollection<StageInfoCollection> Stages;

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\stages\\stages.json";
            if (!System.IO.File.Exists(filePath))
            {
                Stages = new ObservableCollection<StageInfoCollection>();
                StageListView.ItemsSource = Stages;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Stages = JsonConvert.DeserializeObject<ObservableCollection<StageInfoCollection>>(json);

            foreach (StageInfoCollection k in Stages)
            {
            }

            StageListView.ItemsSource = Stages;
        }

        public StageEditor()
        {
            InitializeComponent();
            InitializeList();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            StageInfoCollection temp = new StageInfoCollection
            {
                IdName = "UNTITLED NEW"
            };
            temp.DifficultyList.Add(new StageInfoBase { Difficulty = DifficultyType.NORMAL });
            temp.DifficultyList.Add(new StageInfoBase { Difficulty = DifficultyType.HARD });
            temp.DifficultyList.Add(new StageInfoBase { Difficulty = DifficultyType.CHAOS });
            Stages.Add(temp);
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            StageInfoCollection temp = Helpers.DeepClone((StageInfoCollection)StageListView.SelectedItem);

            Stages.Add(temp);
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            Stages.Remove((StageInfoCollection)StageListView.SelectedItem);
        }

        private void AddButtonClickWave(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;

            StageInfoBase temp = DifficultyView.SelectedItem as StageInfoBase;
            
            temp.EnemyWaves.Add(new EnemyWave());

            int i = 0;
            foreach (EnemyWave wave in temp.EnemyWaves)
            {
                wave.Id = i++;
            }
        }

        private void CopyButtonClickWave(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;

            StageInfoBase temp = DifficultyView.SelectedItem as StageInfoBase;
            EnemyWave temp2 = Helpers.DeepClone((EnemyWave)EnemyWaveView.SelectedItem);
            temp.EnemyWaves.Add(temp2);

            int i = 0;
            foreach (EnemyWave wave in temp.EnemyWaves)
            {
                wave.Id = i++;
            }
        }

        private void DelButtonClickWave(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;

            StageInfoBase temp = DifficultyView.SelectedItem as StageInfoBase;
            temp.EnemyWaves.Remove((EnemyWave)EnemyWaveView.SelectedItem);

            int i = 0;
            foreach (EnemyWave wave in temp.EnemyWaves)
            {
                wave.Id = i++;
            }
        }

        private void AddButtonClickWaveItem(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;

            EnemyWave temp = EnemyWaveView.SelectedItem as EnemyWave;
            temp.EnemyList.Add(new EnemyWaveItem());
        }

        private void CopyButtonClickWaveItem(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;

            EnemyWaveItem temp2 = Helpers.DeepClone((EnemyWaveItem)WaveItemView.SelectedItem);
            EnemyWave temp = EnemyWaveView.SelectedItem as EnemyWave;
            temp.EnemyList.Add(temp2);
        }

        private void DelButtonClickWaveItem(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (DifficultyView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;

            EnemyWave temp = EnemyWaveView.SelectedItem as EnemyWave;
            temp.EnemyList.Remove((EnemyWaveItem)WaveItemView.SelectedItem);
        }
    }
}