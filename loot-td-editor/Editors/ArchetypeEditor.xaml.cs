using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for ArchetypeEditor.xaml
    /// </summary>
    public partial class ArchetypeEditor : UserControl
    {
        public List<ArchetypeBase> Archetypes;
        private int currentID;

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\Archetypes\\archetypes.json";
            Debug.WriteLine("Initialized archetypes");
            if (!System.IO.File.Exists(filePath))
            {
                Archetypes = new List<ArchetypeBase>();
                ArchetypesList.ItemsSource = Archetypes;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Archetypes = JsonConvert.DeserializeObject<List<ArchetypeBase>>(json);
            ArchetypesList.ItemsSource = Archetypes;
            if (Archetypes.Count >= 1)
                currentID = Archetypes[Archetypes.Count - 1].Id + 1;
            else
                currentID = 0;
        }

        public ArchetypeEditor()
        {
            InitializeComponent();
            InitializeList();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ArchetypeBase temp = new ArchetypeBase
            {
                Id = currentID,
                Name = "UNTITLED NEW",
            };
            Archetypes.Add(temp);
            ArchetypesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            ArchetypeBase temp = ArchetypeBase.DeepClone((ArchetypeBase)ArchetypesList.SelectedItem);
            temp.Id = currentID;
            Archetypes.Add(temp);
            ArchetypesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            Archetypes.Remove((ArchetypeBase)ArchetypesList.SelectedItem);
            ArchetypesList.Items.Refresh();
        }

        private void AddButtonClickNode(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            ArchetypeSkillNode temp = new ArchetypeSkillNode
            {
                Id = selected.NodeList.Count,
                Name = "UNTITLED NEW",
            };
            selected.NodeList.Add(temp);
            NodesList.Items.Refresh();
            ChildNumRefresh();
        }

        private void CopyButtonClickNode(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (NodesList.SelectedItem == null)
                return;

            ArchetypeSkillNode selectedNode = (ArchetypeSkillNode)NodesList.SelectedItem;
            ArchetypeSkillNode temp = Helpers.DeepClone(selectedNode);
            selected.NodeList.Add(temp);
            NodesList.Items.Refresh();
            ChildNumRefresh();
        }

        private void RemoveButtonClickNode(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (NodesList.SelectedItem == null)
                return;

            ArchetypeSkillNode selectedNode = (ArchetypeSkillNode)NodesList.SelectedItem;

            selected.NodeList.Remove(selectedNode);

            NodesList.Items.Refresh();
            ChildNumRefresh();
        }

        private void AddButtonClickChild(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            if (NodesList.SelectedItem == null || ChildNum.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            ArchetypeSkillNode selectedNode = (ArchetypeSkillNode)NodesList.SelectedItem;
            if (selectedNode.Children.Contains((int)ChildNum.SelectedItem))
                return;
            selectedNode.Children.Add((int)ChildNum.SelectedItem);
            ChildList.Items.Refresh();
            DrawCanvas();
        }

        private void RemoveButtonClickChild(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (NodesList.SelectedItem == null)
                return;

            ArchetypeSkillNode selectedNode = (ArchetypeSkillNode)NodesList.SelectedItem;

            if (ChildList.SelectedItem == null)
                return;

            selectedNode.Children.Remove((int)ChildList.SelectedItem);
            ChildList.Items.Refresh();
            DrawCanvas();

        }

        private void DrawCanvas()
        {
            int spacing = 100;

            NodeTree.Children.Clear();

            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (selected.NodeList.Count == 0)
                return;

            foreach (ArchetypeSkillNode node in selected.NodeList)
            {
                Rectangle r = new Rectangle
                {
                    Height = 50,
                    Width = 50,
                    Stroke = System.Windows.Media.Brushes.Black,
                    Fill = System.Windows.Media.Brushes.Gray
                };

                TextBlock t = new TextBlock
                {
                    Height = 50,
                    Width = 50,
                    Text = node.Id.ToString(),
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };

                NodeTree.Children.Add(r);
                NodeTree.Children.Add(t);

                Canvas.SetLeft(r, NodeTree.ActualWidth / 2 + node.NodePosition.x * spacing - 25);
                Canvas.SetBottom(r, 0 + node.NodePosition.y * spacing);

                Canvas.SetLeft(t, NodeTree.ActualWidth / 2 + node.NodePosition.x * spacing - 25);
                Canvas.SetBottom(t, 0 + node.NodePosition.y * spacing);

                if (node.Children.Count > 0)
                {
                    ArchetypeSkillNode c = null;
                    foreach (int id in node.Children)
                    {
                        c = selected.NodeList.Find(x => x.Id == id);
                        if (c != null)
                        {
                            Line l = new Line()
                            {
                                Stroke = System.Windows.Media.Brushes.Black,
                                X1 = 0,
                                Y1 = 0,
                                X2 = (c.NodePosition.x - node.NodePosition.x) * spacing,
                                Y2 = 0 + (node.NodePosition.y - c.NodePosition.y) * spacing
                            };
                            
                            NodeTree.Children.Add(l);
                            Canvas.SetLeft(l, NodeTree.ActualWidth / 2 + node.NodePosition.x * spacing);
                            Canvas.SetZIndex(l, -1);
                            if (node.NodePosition.y <= c.NodePosition.y)
                            {
                                Canvas.SetBottom(l, 0 + node.NodePosition.y * spacing + r.Height / 2);
                            }
                            else
                            {
                                Canvas.SetBottom(l, 0 + (node.NodePosition.y - (node.NodePosition.y - c.NodePosition.y)) * spacing + r.Height / 2);
                            }
                        }
                    }
                }
            }
        }

        private void DoubleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DoubleUpDown d = ((DoubleUpDown)sender);

            if (d.Value == null)
                return;

            switch (d.Name)
            {
                case "HpGrow":
                    HealthL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;

                case "SpGrow":
                    SoulL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;

                case "StrGrow":
                    StrL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;

                case "IntGrow":
                    IntL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;

                case "AgiGrow":
                    AgiL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;

                case "WillGrow":
                    WillL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    return;
            }
        }

        private void CanvasEdit(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DrawCanvas();
        }

        private void ArchetypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawCanvas();
        }

        private void ChildNumRefresh()
        {
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            ChildNum.ItemsSource = null;
            ChildNum.ItemsSource = selected.NodeList;
        }

        
    }

    public static class Helpers
    {
        public static T DeepClone<T>(T o)
        {
            string s = JsonConvert.SerializeObject(o);
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}