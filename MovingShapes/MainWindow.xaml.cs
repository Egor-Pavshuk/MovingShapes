using MovingShapes.Events;
using MovingShapes.Exceptions;
using MovingShapes.Exceptions.CustomException;
using MovingShapes.Logging;
using MovingShapes.Models;
using MovingShapes.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfFunctionalLibrary;

namespace MovingShapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CustomShape> _shapes;
        private DispatcherTimer _dispatcherTimer;
        private int _countOfMovingShapes;
        private ResourceDictionary _dictionary;

        public MainWindow()
        {
            InitializeComponent();

            DataContractIsolatedStorage.WriteToFile(new RandomPoint());

            _shapes = new List<CustomShape>();
            LanguageList.Items.Add("English");
            LanguageList.Items.Add("Русский");

            _dictionary = new ResourceDictionary
            {
                Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative)
            };
            Resources.MergedDictionaries.Add(_dictionary);

            LanguageList.SelectedItem = LanguageList.Items[0];

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += TimerEvent;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            _dispatcherTimer.Start();
        }

        private void TimerEvent(object? sender, EventArgs e)
        {
            foreach (var shape in _shapes)
            {
                if (!shape.IsStoped)
                {
                    var point = new Point(CanvasWindow.ActualWidth, CanvasWindow.ActualHeight);
                    try
                    {
                        shape.Move(ref point);
                    }
                    catch (CustomException<ShapeIsOutOfWindowExceptionArgs>)
                    {
                        _ = Logger.LogExceptionAsync(new ShapeIsOutOfWindowExceptionArgs(shape).Message, DateTime.Now);
                        shape.ReturnShapeToWindow(ref point);
                    }
                    finally
                    {
                        shape.Draw();                      
                    }
                    shape.CheckForIntersection(_shapes);
                }
            }
        }

        #region Add shape
        private void Triangle_Click(object sender, RoutedEventArgs e)
        {
            CustomShape triangle = new CustomTriangle(CanvasWindow) { Name = _dictionary["triangle"], CurrentNumber = _shapes.Where(s => s.GetType().Name == typeof(CustomTriangle).Name).Count() + 1 };
            _shapes.Add(triangle);
            ShapesList.Items.Add($"{triangle.Name} {triangle.CurrentNumber} \n");
            _countOfMovingShapes++;
            ChangeButtonsContent();
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            CustomShape square = new CustomSquare(CanvasWindow) { Name = _dictionary["square"], CurrentNumber = _shapes.Where(s => s.GetType().Name == typeof(CustomSquare).Name).Count() + 1 };
            _shapes.Add(square);
            ShapesList.Items.Add($"{square.Name} {square.CurrentNumber} \n");
            _countOfMovingShapes++;
            ChangeButtonsContent();
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            CustomShape circle = new CustomCircle(CanvasWindow) { Name = _dictionary["circle"], CurrentNumber = _shapes.Where(s => s.GetType().Name == typeof(CustomCircle).Name).Count() + 1 };
            _shapes.Add(circle);
            ShapesList.Items.Add($"{circle.Name} {circle.CurrentNumber} \n");
            _countOfMovingShapes++;
            ChangeButtonsContent();
        }
        #endregion
        private void ShapesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ShapesList.SelectedItems.Count != 0)
            {
                if (_shapes[ShapesList.SelectedIndex].IsStoped)
                {
                    StopRunButton.Content = _dictionary["run"];
                }
                else
                {
                    StopRunButton.Content = _dictionary["stop"];
                }
            }
        }

        private void StopRunButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShapesList.SelectedItems.Count != 0)
            {
                if (_shapes[ShapesList.SelectedIndex].IsStoped)
                {
                    _shapes[ShapesList.SelectedIndex].IsStoped = false;
                    _countOfMovingShapes++;
                }
                else
                {
                    _shapes[ShapesList.SelectedIndex].IsStoped = true;
                    _countOfMovingShapes--;
                }
            }

            ChangeButtonsContent();

        }

        private void StopRunAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (_countOfMovingShapes != 0)
            {
                var movingShapes = _shapes.Where(s => s.IsStoped == false);
                foreach (var shape in movingShapes)
                {
                    shape.IsStoped = true;
                }
                StopRunAllButton.Content = _dictionary["runAll"];
                _countOfMovingShapes = 0;
                StopRunButton.Content = _dictionary["run"];
            }
            else
            {
                foreach (var shape in _shapes)
                {
                    shape.IsStoped = false;
                }
                StopRunAllButton.Content = _dictionary["stopAll"];
                _countOfMovingShapes = _shapes.Count;
                StopRunButton.Content = _dictionary["stop"]; ;
            }

        }

        private void LanguageList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (LanguageList.SelectedValue)
            {
                case "English":
                    if (!"English".Equals(_dictionary["language"]))
                    {
                        _dictionary.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                        ChangeButtonsContent();
                        ChangeShapesNames();
                    }
                    break;
                case "Русский":
                    if (!"Russian".Equals(_dictionary["language"]))
                    {
                        _dictionary.Source = new Uri("..\\Languages\\Russian.xaml", UriKind.Relative);
                        ChangeButtonsContent();
                        ChangeShapesNames();
                    }
                    break;
                default:
                    if (!"English".Equals(_dictionary["language"]))
                    {
                        _dictionary.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                        ChangeButtonsContent();
                        ChangeShapesNames();
                    }
                    break;
            }

        }

        private void ChangeButtonsContent()
        {
            if (ShapesList.SelectedItems.Count != 0)
            {
                if (_shapes[ShapesList.SelectedIndex].IsStoped)
                {
                    StopRunButton.Content = _dictionary["run"];
                }
                else
                    StopRunButton.Content = _dictionary["stop"];
            }
            else
            {
                StopRunButton.Content = _dictionary["stop"];
            }

            if (_countOfMovingShapes == 0 && _shapes.Count != 0)
            {
                StopRunAllButton.Content = _dictionary["runAll"];
            }
            else if (_countOfMovingShapes == _shapes.Count)
            {
                StopRunAllButton.Content = _dictionary["stopAll"];
            }
            else
                StopRunAllButton.Content = _dictionary["stopAll"];

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _shapes.Clear();
            ShapesList.Items.Clear();
            StopRunAllButton.Content = _dictionary["stopAll"];
            StopRunButton.Content = _dictionary["stop"];
            CanvasWindow.Children.Clear();
            _countOfMovingShapes = 0;
        }

        private void ChangeShapesNames()
        {
            ShapesList.Items.Clear();

            foreach (var shape in _shapes)
            {
                switch (shape.GetType().Name)
                {
                    case "CustomCircle":
                        shape.Name = _dictionary["circle"];
                        break;
                    case "CustomTriangle":
                        shape.Name = _dictionary["triangle"];
                        break;
                    case "CustomSquare":
                        shape.Name = _dictionary["square"];
                        break;
                    default:
                        break;
                }
                ShapesList.Items.Add($"{shape.Name} {shape.CurrentNumber} \n");
            }
        }
        #region Open/Save
        private void OpenBin_Click(object sender, RoutedEventArgs e)
        {
            ClearButton_Click(sender, e);

            _shapes = ShapesSerialization.DeserializeFromBin();
            foreach (var shape in _shapes)
            {
                ShapesList.Items.Add($"{shape.Name} {shape.CurrentNumber} \n");
                shape.AddToCanvas(CanvasWindow);
                if (!shape.IsStoped)
                {
                    _countOfMovingShapes++;
                }
            }
            ChangeButtonsContent();
            ChangeShapesNames();
        }

        private void OpenXml_Click(object sender, RoutedEventArgs e)
        {
            ClearButton_Click(sender, e);

            _shapes = ShapesSerialization.DeserializeFromXml();
            foreach (var shape in _shapes)
            {
                ShapesList.Items.Add($"{shape.Name} {shape.CurrentNumber} \n");
                shape.AddToCanvas(CanvasWindow);
                if (!shape.IsStoped)
                {
                    _countOfMovingShapes++;
                }
            }
            ChangeButtonsContent();
            ChangeShapesNames();
        }

        private void OpenJson_Click(object sender, RoutedEventArgs e)
        {
            ClearButton_Click(sender, e);

            _shapes = ShapesSerialization.DeserializeFromJson();
            foreach (var shape in _shapes)
            {
                ShapesList.Items.Add($"{shape.Name} {shape.CurrentNumber} \n");
                shape.AddToCanvas(CanvasWindow);
                if (!shape.IsStoped)
                {
                    _countOfMovingShapes++;
                }
            }
            ChangeButtonsContent();
            ChangeShapesNames();
        }

        private void SaveBin_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count != 0)
            {
                ShapesSerialization.SerializeToBin(_shapes);
                //ClearButton_Click(sender, e);
            }
        }

        private void SaveXml_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count != 0)
            {
                ShapesSerialization.SerializeToXml(_shapes);
                //ClearButton_Click(sender, e);
            }
        }

        private void SaveJson_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count != 0)
            {
                ShapesSerialization.SerializeToJson(_shapes);
                //ClearButton_Click(sender, e);
            }
        }
        #endregion
        private void PlusButon_Click(object sender, RoutedEventArgs e)
        {
            if (ShapesList.SelectedItems.Count != 0)
            {
                _shapes[ShapesList.SelectedIndex].ShapesIntersection += ShapesIntersected;

                var selectedItem = ShapesList.SelectedItem;
                selectedItem = new StringBuilder(selectedItem.ToString()).Append(" *");
                ShapesList.Items[ShapesList.SelectedIndex] = selectedItem;
            }
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShapesList.SelectedItems.Count != 0)
            {
                _shapes[ShapesList.SelectedIndex].ShapesIntersection -= ShapesIntersected;

                var selectedItem = ShapesList.SelectedItem.ToString();
                if (selectedItem != null && selectedItem.LastIndexOf('*') - 1 > 0)
                {
                    var newItem = new StringBuilder(selectedItem).Remove(selectedItem.LastIndexOf('*') - 1, 2);
                    ShapesList.Items[ShapesList.SelectedIndex] = newItem;
                }
            }
        }

        private void ShapesIntersected(object? sender, ShapesIntersectionEventArgs e)
        {
            MakeSound();
            WritePointOfIntersectionToConsole(e.PointOfIntersection);
        }
        private void MakeSound()
        {
            Console.Beep();
        }
        private void WritePointOfIntersectionToConsole(Point point)
        {
            Console.WriteLine(point);
        }
    }
}
