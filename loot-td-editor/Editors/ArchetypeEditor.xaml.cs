using loot_td;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace loot_td_editor.Editors
{
    /// <summary>
    /// Interaction logic for ArchetypeEditor.xaml
    /// </summary>
    public partial class ArchetypeEditor : UserControl
    {
        public ObservableCollection<ArchetypeBase> Archetypes;
        private int currentID;

        public void InitializeList()
        {
            if (Properties.Settings.Default.JsonLoadPath == "")
                return;
            string filePath = Properties.Settings.Default.JsonLoadPath + "\\Archetypes\\archetypes.json";
            Debug.WriteLine("Initialized archetypes");
            if (!System.IO.File.Exists(filePath))
            {
                Archetypes = new ObservableCollection<ArchetypeBase>();
                ArchetypesList.ItemsSource = Archetypes;
                return;
            }
            string json = System.IO.File.ReadAllText(filePath);
            Archetypes = JsonConvert.DeserializeObject<ObservableCollection<ArchetypeBase>>(json);

            foreach (ArchetypeBase k in Archetypes)
            {
                if (k.IdName == null)
                    k.IdName = "";
            }

            ArchetypesList.ItemsSource = Archetypes;
        }

        public ArchetypeEditor()
        {
            InitializeComponent();
            InitializeList();
            ScrollViewer.ScrollToBottom();
            ScrollViewer.ScrollToHorizontalOffset(270);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ArchetypeBase temp = new ArchetypeBase
            {
                IdName = "UNTITLED NEW",
            };
            Archetypes.Add(temp);
            //ArchetypesList.Items.Refresh();
            currentID++;
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            ArchetypeBase temp = ArchetypeBase.DeepClone((ArchetypeBase)ArchetypesList.SelectedItem);

            Archetypes.Add(temp);
            //ArchetypesList.Items.Refresh();
            currentID++;
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            Archetypes.Remove((ArchetypeBase)ArchetypesList.SelectedItem);
            //ArchetypesList.Items.Refresh();
        }

        private void AddButtonClickNode(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            ArchetypeSkillNode temp = new ArchetypeSkillNode
            {
                IdName = "UNTITLED NEW",
                Id = selected.NodeList.Count,
            };
            selected.NodeList.Add(temp);
            //NodesList.Items.Refresh();
            //ChildNumRefresh();
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
            temp.Id++;
            selected.NodeList.Add(temp);
            //NodesList.Items.Refresh();
            //ChildNumRefresh();
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

            //NodesList.Items.Refresh();
            //ChildNumRefresh();
        }

        private void AddButtonClickChild(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            if (NodesList.SelectedItem == null || ChildNum.SelectedValue == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            ArchetypeSkillNode selectedNode = (ArchetypeSkillNode)NodesList.SelectedItem;
            if (selectedNode.Children.Contains((int)ChildNum.SelectedValue))
                return;
            selectedNode.Children.Add((int)ChildNum.SelectedValue);
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

        private static int GridSpacing = 75;

        private void DrawGrid()
        {
            for (int i = 0; i < 18; i++)
            {
                Line l = new Line()
                {
                    Stroke = System.Windows.Media.Brushes.LightBlue,
                    X1 = 0,
                    Y1 = 0,
                    X2 = NodeTree.ActualWidth,
                    Y2 = 0
                };
                NodeTree.Children.Add(l);
                Canvas.SetLeft(l, 0);
                Canvas.SetBottom(l, 0 + 25 + i * GridSpacing);
                Canvas.SetZIndex(l, -2);
            }

            for (int i = -8; i < 9; i++)
            {
                Line l = new Line()
                {
                    Stroke = System.Windows.Media.Brushes.LightBlue,
                    X1 = 0,
                    Y1 = 0,
                    X2 = 0,
                    Y2 = -NodeTree.ActualHeight
                };
                NodeTree.Children.Add(l);
                Canvas.SetLeft(l, NodeTree.ActualWidth / 2 + i * GridSpacing);
                Canvas.SetBottom(l, 0);
                Canvas.SetZIndex(l, -2);
            }
        }

        private void DrawCanvas()
        {
            int spacing = GridSpacing;

            NodeTree.Children.Clear();

            DrawGrid();

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
                    Fill = System.Windows.Media.Brushes.LightGray
                };

                r.MouseLeftButtonDown += mouseDown;
                r.MouseLeftButtonUp += mouseUp;
                r.MouseMove += rect_MouseMove;

                r.DataContext = node;

                TextBlock t = new TextBlock
                {
                    Height = 50,
                    Width = 50,
                    Text = node.Id.ToString() + "\n" + node.IdName + "\nMax: " + node.MaxLevel,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    IsHitTestVisible = false
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
                        c = selected.NodeList.ToList().Find(x => x.Id == id);
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

        private void mouseDown(object sender, MouseEventArgs e)
        {
            var element = (Rectangle)sender;
            element.CaptureMouse();
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();

            if (rect.DataContext != null)
            {
                ArchetypeSkillNode a = (ArchetypeSkillNode)rect.DataContext;
                a.NodePosition.x = (int)Math.Round((Canvas.GetLeft(rect) - NodeTree.ActualWidth / 2) / GridSpacing, 0);
                a.NodePosition.y = (int)Math.Round(Canvas.GetBottom(rect) / GridSpacing, 0);
            }

            DrawCanvas();
        }

        private void rect_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = (Rectangle)sender;

            if (!rect.IsMouseCaptured) return;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(NodeTree);

            // center the rect on the mouse
            double left = mousePos.X - (rect.ActualWidth / 2);
            double top = -mousePos.Y + (NodeTree.ActualHeight) + (rect.ActualHeight / 2);

            Canvas.SetLeft(rect, Math.Round((left + 10) / GridSpacing, 0) * GridSpacing - rect.ActualWidth / 2);
            Canvas.SetBottom(rect, Math.Round((top - 50) / GridSpacing, 0) * GridSpacing);
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
                    break;

                case "SpGrow":
                    SoulL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    break;

                case "StrGrow":
                    StrL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    break;

                case "IntGrow":
                    IntL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    break;

                case "AgiGrow":
                    AgiL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    break;

                case "WillGrow":
                    WillL.Content = Math.Round((double)d.Value * 100f, 1, MidpointRounding.AwayFromZero);
                    break;
            }

            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            double totalstats = 0, totalhpsp = 0, total = 0;
            totalstats = selected.StrengthGrowth + selected.IntelligenceGrowth + selected.AgilityGrowth + selected.WillGrowth;
            totalhpsp = selected.HealthGrowth + selected.SoulPointGrowth * 50;
            total = totalhpsp + totalstats;
            TotalL.Content = totalstats.ToString("F2") + " + " + totalhpsp.ToString("F2") + " = " + total.ToString("F2");
        }

        private void CanvasEdit(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DrawCanvas();
        }

        private void ArchetypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawCanvas();
            CalculateTotalSkillPoints();
        }

        private void ChildNumRefresh()
        {
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            ChildNum.ItemsSource = null;
            ChildNum.ItemsSource = selected.NodeList;
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedValue == null || ArchetypesList.SelectedItem == null || NodesList.SelectedItem == null)
                return;
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            ArchetypeSkillNode node = (ArchetypeSkillNode)NodesList.SelectedItem;
            if ((NodeType)e.AddedItems[0] == NodeType.ABILITY)
            {
                NodeAbilityList.IsReadOnly = false;
                NodeAbilityList.IsHitTestVisible = true;
                NodeAbilityList.Focusable = true;
            }
            else
            {
                NodeAbilityList.IsHitTestVisible = false;
                NodeAbilityList.Focusable = false;
                NodeAbilityList.IsReadOnly = true;
                NodeAbilityList.SelectedItem = null;
                node.AbilityId = null;
            }
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (selected.NodeList.Count == 0)
                return;

            TextBlock s = sender as TextBlock;
            if (s.Text == null || s.Text == "")
                return;

            if (int.TryParse(s.Text, out int x))
                s.Text = x + "\t" + selected.NodeList[x].IdName;
        }

        private void MaxLevelChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CalculateTotalSkillPoints();
        }

        private void CalculateTotalSkillPoints()
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            int total = 0;
            foreach (ArchetypeSkillNode n in selected.NodeList)
            {
                total += n.MaxLevel;
                total -= n.InitialLevel;
            }
            TotalSkillPoints.Content = total.ToString();
        }

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            var dataView =
              (ListCollectionView)CollectionViewSource.GetDefaultView(ArchetypesList.ItemsSource);

            dataView.SortDescriptions.Clear();
            if (sortBy == "IdName")
            {
                dataView.CustomSort = new NaturalStringComparer();
            }
            else
            {
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
            }
            dataView.Refresh();
        }

    }
}