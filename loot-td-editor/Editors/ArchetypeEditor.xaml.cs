using loot_td;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
        private IList<BonusType> _bonusTypes;
        public IList<GroupType> GroupTypes { get { return Enum.GetValues(typeof(GroupType)).Cast<GroupType>().ToList<GroupType>(); } }

        public IList<BonusType> BonusTypes
        {
            get
            {
                if (_bonusTypes == null)
                    _bonusTypes = Enum.GetValues(typeof(BonusType)).Cast<BonusType>().ToList<BonusType>();
                return _bonusTypes;
            }
        }

        public ObservableCollection<ArchetypeBase> Archetypes;
        private int currentID;
        private int selectedNodeId = -1;

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
            Archetypes = JsonConvert.DeserializeObject<ObservableCollection<ArchetypeBase>>(json,
                new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        System.Windows.MessageBox.Show(args.ErrorContext.Error.Message, "Confirmation", MessageBoxButton.YesNo);
                        args.ErrorContext.Handled = true;
                    }
                });

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
            Helpers.archetypeEditor = this;
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

            string name = ((ArchetypeBase)ArchetypesList.SelectedItem).IdName;

            MessageBoxResult res = System.Windows.MessageBox.Show("Delete " + name + "?", "Confirmation", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.No)
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
            temp.Id = selected.NodeList.Count;
            string s = new string(temp.IdName.Where(c => char.IsDigit(c)).ToArray());
            if (s.Length > 0)
            {
                if (Int32.TryParse(new string(temp.IdName.Where(c => char.IsDigit(c)).ToArray()), out int res))
                {
                    temp.IdName = temp.IdName.TrimEnd(s.ToCharArray());
                    temp.IdName += (res + 1);
                }
            }
            else
            {
                temp.IdName += 1.ToString();
            }
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
            for (int i = 0; i < 23; i++)
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

            for (int i = -12; i < 13; i++)
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

            Rectangle r;

            foreach (ArchetypeSkillNode node in selected.NodeList)
            {
                r = new Rectangle
                {
                    Height = 50,
                    Width = 50,
                    Stroke = System.Windows.Media.Brushes.Black,
                    Fill = System.Windows.Media.Brushes.LightGray
                };
                if (node.Type == NodeType.ABILITY)
                {
                    r.Fill = System.Windows.Media.Brushes.PaleTurquoise;
                }
                if (node.InitialLevel > 0)
                {
                    r.Fill = System.Windows.Media.Brushes.LightGreen;
                }
                if (node.Type == NodeType.GREATER)
                {
                    r.Fill = System.Windows.Media.Brushes.PaleGoldenrod;
                }

                if (SearchBox.Text != "" && node.Bonuses.ToList().FindAll(x => x.bonusType.ToString().ToLower().Contains(SearchBox.Text.ToLower())).Any())
                {
                    r.Fill = System.Windows.Media.Brushes.LightSkyBlue;
                }

                if (node.Id == selectedNodeId)
                {
                    r.Stroke = System.Windows.Media.Brushes.Red;
                }

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
                NodesList.SelectedItem = a;
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

            Canvas.SetLeft(rect, Math.Round((left + 50) / GridSpacing, 0) * GridSpacing - rect.ActualWidth / 2 - 25);
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
            //totalhpsp = selected.HealthGrowth + selected.SoulPointGrowth * 50;
            totalhpsp = selected.HealthGrowth;
            total = totalhpsp + totalstats;
            TotalL.Content = totalstats.ToString("F2") + " + " + totalhpsp.ToString("F2") + " = " + total.ToString("F2");
        }

        private void CanvasEdit(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;
            foreach (ArchetypeSkillNode node in selected.NodeList)
            {
                foreach (int index in node.Children)
                {
                    if (selected.NodeList.Where(y => y.Id == index).Count() == 0)
                        node.HasError = true;
                    else
                        node.HasError = false;
                }
            }
            DrawCanvas();
        }

        private void ArchetypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawCanvas();
            CalculateTotalSkillPoints();
        }

        private void ChildNumRefresh()
        {
            if (ArchetypesList.SelectedItem == null)
                return;
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
            if (NodesList.SelectedItem == null)
                return;

            ArchetypeBase selected = (ArchetypeBase)ArchetypesList.SelectedItem;

            if (selected.NodeList.Count == 0)
                return;
            ArchetypeSkillNode selectedNode = NodesList.SelectedItem as ArchetypeSkillNode;
            TextBlock s = sender as TextBlock;
            if (s.Text == null || s.Text == "")
                return;

            if (int.TryParse(s.Text, out int x))
            {
                IEnumerable<ArchetypeSkillNode> targetNode = selected.NodeList.Where(y => y.Id == x);
                if (targetNode.Count() == 0)
                {
                    s.Text = x + "\t ERROR";
                }
                else
                {
                    s.Text = x + "\t" + targetNode.First().IdName;
                }
            }
        }

        private void MaxLevelChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CalculateTotalSkillPoints();
            BonusGridSumValues();
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

        private void NodesList_Selected(object sender, RoutedEventArgs e)
        {
            ListView view = sender as ListView;
            ArchetypeSkillNode archetypeSkillNode = view.SelectedItem as ArchetypeSkillNode;
            if (archetypeSkillNode == null)
            {
                selectedNodeId = -1;
                return;
            }
            selectedNodeId = archetypeSkillNode.Id;
            DrawCanvas();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            box.IsTextSearchEnabled = false;
            box.ItemsSource = BonusTypes.ToList();
            var view = (ListCollectionView)CollectionViewSource.GetDefaultView(box.ItemsSource);
        }

        private void BonusGridSumValues()
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            if (NodesList.SelectedItem == null)
                return;
            if (BonusGrid == null)
                return;

            ArchetypeSkillNode node = NodesList.SelectedItem as ArchetypeSkillNode;
            for (int i = 0; i < BonusGrid.Items.Count; ++i)
            {
                var k = BonusGrid.Items[i];
                if (k is ScalingBonusProperty_Int)
                {
                    ScalingBonusProperty_Int o = k as ScalingBonusProperty_Int;
                    SetBonusEditorValues(o, node);
                }
            }
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            BonusGridSumValues();
        }

        private void BonusGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;
            if (NodesList.SelectedItem == null)
                return;
            ArchetypeSkillNode node = NodesList.SelectedItem as ArchetypeSkillNode;
            DataGridRow r = e.Row;
            int i = e.Row.GetIndex();
            var k = e.Row.Item;
            if (k is ScalingBonusProperty_Int)
            {
                ScalingBonusProperty_Int o = k as ScalingBonusProperty_Int;
                SetBonusEditorValues(o, node);
            }
        }

        private void SetBonusEditorValues(ScalingBonusProperty_Int bonus, ArchetypeSkillNode node)
        {
            if (node.MaxLevel == 1)
            {
                bonus.sum = bonus.growthValue;
            }
            else
            {
                bonus.sum = bonus.growthValue * (node.MaxLevel - 1) + bonus.finalLevelValue;
            }
            bonus.perPoint = (float)bonus.sum / node.MaxLevel;
        }

        private void GroupComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            box.ItemsSource = GroupTypes.ToList();
        }

        private void ShiftAllClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase archetypeBase = ArchetypesList.SelectedItem as ArchetypeBase;
            int xShift = (int)ShiftX.Value;
            int yShift = (int)ShiftY.Value;

            foreach (ArchetypeSkillNode node in archetypeBase.NodeList)
            {
                node.NodePosition.x += xShift;
                node.NodePosition.y += yShift;
            }
            DrawCanvas();
        }

        private void ArchetypeTotalsClick(object sender, RoutedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            ArchetypeBase archetypeBase = ArchetypesList.SelectedItem as ArchetypeBase;

            Dictionary<string, NodeTotalInfo> nodeDict = new Dictionary<string, NodeTotalInfo>();

            foreach (ArchetypeSkillNode node in archetypeBase.NodeList)
            {
                foreach (ScalingBonusProperty_Int prop in node.Bonuses)
                {
                    string s = prop.bonusType + "#" + prop.modifyType + "#" + prop.restriction;

                    if (!nodeDict.ContainsKey(s))
                    {
                        nodeDict[s] = new NodeTotalInfo(prop.bonusType, prop.modifyType, prop.restriction);
                    }
                    NodeTotalInfo totalInfo = nodeDict[s];
                    float finalStat;

                    if (node.MaxLevel == 1)
                        finalStat = prop.growthValue;
                    else
                        finalStat = prop.growthValue * (node.MaxLevel - 1) + prop.finalLevelValue;

                    if (prop.modifyType != ModifyType.MULTIPLY)
                    {
                        if (finalStat < 0)
                        {
                            totalInfo.negTotal += finalStat;
                            totalInfo.negTotalLevel += node.MaxLevel;
                        }
                        else
                        {
                            totalInfo.posTotal += finalStat;
                            totalInfo.posTotalLevel += node.MaxLevel;
                        }
                    }
                    else
                    {
                        if (finalStat < 0)
                        {
                            totalInfo.negTotal *= (1f + finalStat / 100f);
                            totalInfo.negTotalLevel += node.MaxLevel;
                        }
                        else
                        {
                            totalInfo.posTotal *= (1f + finalStat / 100f);
                            totalInfo.posTotalLevel += node.MaxLevel;
                        }
                    }
                }
            }

            string display = "";
            List<NodeTotalInfo> list = nodeDict.Values.OrderBy(x => x.bonus).ToList();

            foreach (NodeTotalInfo nodeInfo in list)
            {
                if (nodeInfo.mod == ModifyType.FIXED_TO)
                    continue;
                if (nodeInfo.mod != ModifyType.MULTIPLY)
                {
                    if (nodeInfo.posTotal > 0)
                    {
                        float posAvg = nodeInfo.posTotal / nodeInfo.posTotalLevel;

                        display += Localization.GetBonusTypeString(nodeInfo.bonus, nodeInfo.mod, nodeInfo.posTotal, nodeInfo.posTotal, nodeInfo.restrict) + "Levels: " + nodeInfo.posTotalLevel + ", Avg: " + posAvg.ToString("n2") + "\n\n";
                    }
                    if (nodeInfo.negTotal < 0)
                    {
                        float negAvg = nodeInfo.negTotal / nodeInfo.negTotalLevel;
                        display += Localization.GetBonusTypeString(nodeInfo.bonus, nodeInfo.mod, nodeInfo.negTotal, nodeInfo.negTotal, nodeInfo.restrict) + "Levels: " + nodeInfo.negTotalLevel + ", Avg: " + negAvg.ToString("n2") + "\n\n";
                    }
                } else
                {
                    if (nodeInfo.posTotal > 100f)
                    {
                        float posAvg = nodeInfo.posTotal / nodeInfo.posTotalLevel;
                        nodeInfo.posTotal -= 100f;
                        display += Localization.GetBonusTypeString(nodeInfo.bonus, nodeInfo.mod, nodeInfo.posTotal, nodeInfo.posTotal, nodeInfo.restrict) + "Levels: " + nodeInfo.posTotalLevel + ", Avg: " + posAvg.ToString("n2") + "\n\n";
                    }
                    if (nodeInfo.negTotal < 100f)
                    {
                        float negAvg = nodeInfo.negTotal / nodeInfo.negTotalLevel;
                        nodeInfo.negTotal -= 100f;
                        display += Localization.GetBonusTypeString(nodeInfo.bonus, nodeInfo.mod, nodeInfo.negTotal, nodeInfo.negTotal, nodeInfo.restrict) + "Levels: " + nodeInfo.negTotalLevel + ", Avg: " + negAvg.ToString("n2") + "\n\n";
                    }
                }
            }

            //MessageBoxResult result = System.Windows.MessageBox.Show(display, "Totals", MessageBoxButton.OK);
            TotalsDisplay totalsDisplay = new TotalsDisplay(display);
            totalsDisplay.ShowDialog();
        }

        private class NodeTotalInfo
        {
            public BonusType bonus;
            public ModifyType mod;
            public GroupType restrict;
            public float posTotal;
            public int posTotalLevel;
            public float negTotal;
            public int negTotalLevel;

            public NodeTotalInfo(BonusType bonus, ModifyType mod, GroupType r)
            {
                this.bonus = bonus;
                this.mod = mod;
                this.restrict = r;
                if (mod == ModifyType.MULTIPLY)
                {
                    negTotal = 100f;
                    posTotal = 100f;
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ArchetypesList.SelectedItem == null)
                return;

            TextBox textBox = sender as TextBox;

            if (textBox.Text == null)
                return;

            DrawCanvas();
        }
    }

    public class EmptyBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return "TRUE";
                else
                    return "";
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}