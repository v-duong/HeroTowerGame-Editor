using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace loot_td_editor.Editors
{
    public partial class StageEditor : UserControl
    {
        public IList<DifficultyType> DifficultyTypes { get { return Enum.GetValues(typeof(DifficultyType)).Cast<DifficultyType>().ToList<DifficultyType>(); } }
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
            DifficultyBox.ItemsSource = DifficultyTypes;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            StageInfoCollection temp = new StageInfoCollection
            {
                IdName = "UNTITLED NEW"
            };
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

            string name = ((StageInfoCollection)StageListView.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
                return;

            Stages.Remove((StageInfoCollection)StageListView.SelectedItem);
        }

        private void AddButtonClickWave(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;

            StageInfoCollection temp = StageListView.SelectedItem as StageInfoCollection;

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
            if (EnemyWaveView.SelectedItem == null)
                return;

            StageInfoCollection temp = StageListView.SelectedItem as StageInfoCollection;
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
            if (EnemyWaveView.SelectedItem == null)
                return;

            StageInfoCollection temp = StageListView.SelectedItem as StageInfoCollection;
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
            if (EnemyWaveView.SelectedItem == null)
                return;

            EnemyWave temp = EnemyWaveView.SelectedItem as EnemyWave;
            temp.EnemyList.Add(new EnemyWaveItem());
        }

        private void CopyButtonClickWaveItem(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
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
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;

            EnemyWave temp = EnemyWaveView.SelectedItem as EnemyWave;
            temp.EnemyList.Remove((EnemyWaveItem)WaveItemView.SelectedItem);
        }

        private void AddPropertiesClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (StagePropertiesComboBox.SelectedItem == null)
                return;

            StageInfoCollection s = StageListView.SelectedItem as StageInfoCollection;
            AffixBase a = StagePropertiesComboBox.SelectedItem as AffixBase;
            if (s.StageProperties.Contains(a.IdName))
                return;
            s.StageProperties.Add(a.IdName);
        }

        private void RemovePropertiesClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (StagePropertiesList.SelectedItem == null)
                return;

            StageInfoCollection s = StageListView.SelectedItem as StageInfoCollection;
            string a = StagePropertiesList.SelectedItem as string;
            s.StageProperties.Remove(a);
        }

        private void AddWavePropertiesClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;
            if (WaveModComboBox.SelectedItem == null)
                return;

            EnemyWaveItem s = WaveItemView.SelectedItem as EnemyWaveItem;
            AffixBase a = WaveModComboBox.SelectedItem as AffixBase;
            if (s.BonusProperties.Contains(a.IdName))
                return;
            s.BonusProperties.Add(a.IdName);
        }

        private void RemoveWavePropertiesClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;
            if (WavePropertiesList.SelectedItem == null)
                return;

            EnemyWaveItem s = WaveItemView.SelectedItem as EnemyWaveItem;
            string a = WavePropertiesList.SelectedItem as string;
            s.BonusProperties.Remove(a);
        }

        private void AddSpawnerClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (spawnerBox.Value == null)
                return;

            StageInfoCollection s = StageListView.SelectedItem as StageInfoCollection;
            int selectedValue = (int)spawnerBox.Value;
            if (s.SpawnerInfos.Where(x => x.Spawner == selectedValue).ToList().Count > 0)
                return;
            SpawnerInfo info = new SpawnerInfo(selectedValue);
            s.SpawnerInfos.Add(info);
            spawnerList.SelectedItem = info;
        }

        private void RemoveSpawnerClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (spawnerList.SelectedItem == null)
                return;

            StageInfoCollection s = StageListView.SelectedItem as StageInfoCollection;
            s.SpawnerInfos.Remove(spawnerList.SelectedItem as SpawnerInfo);
        }

        private void AddGoalClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (spawnerList.SelectedItem == null)
                return;
            if (goalBox.Value == null)
                return;

            SpawnerInfo spawnerInfo = spawnerList.SelectedItem as SpawnerInfo;
            int selectedValue = (int)goalBox.Value;
            if (spawnerInfo.PossibleGoals.Contains(selectedValue))
                return;
            spawnerInfo.PossibleGoals.Add(selectedValue);
        }

        private void RemoveGoalClick(object sender, RoutedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (spawnerList.SelectedItem == null)
                return;
            if (goalList.SelectedItem == null)
                return;

            SpawnerInfo spawnerInfo = spawnerList.SelectedItem as SpawnerInfo;
            int selected = (int)goalList.SelectedItem;
            spawnerInfo.PossibleGoals.Remove(selected);
        }

        private void SpawnersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StageListView.SelectedItem == null)
                return;
            if (EnemyWaveView.SelectedItem == null)
                return;
            if (WaveItemView.SelectedItem == null)
                return;
            if (SpawnersComboBox.SelectedItem == null)
                return;

            SpawnerInfo spawnerInfo = SpawnersComboBox.SelectedItem as SpawnerInfo;
            GoalComboBox.ItemsSource = spawnerInfo.PossibleGoals;
        }

        private void EnemyWaveView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnemyWaveItemOptions.IsEnabled = EnemyWaveView.SelectedItem != null;
        }

        private void WaveItemView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnemyListItemOptions.IsEnabled = WaveItemView.SelectedItem != null;
        }
    }
}