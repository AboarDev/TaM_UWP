using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using  Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using TamLib;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TamGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string theTitle = "AAA";
        public int row = 0;
        public int col = 0;
        public int moves = 0;

        public int numberOfColumns = 3;

        public int numberOfRows = 3;

        public GameViewModel ViewModel { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new GameViewModel(() => GoBack());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is LevelData)
            {
                LevelData levelData = (LevelData) e.Parameter;
                ViewModel.AddLevel(levelData);
                ViewModel.Timer();
                DrawLevel();
            }
        }

        private void DrawLevel ()
        {
            while (aGrid.ColumnDefinitions.Count < this.ViewModel.Width)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                aGrid.ColumnDefinitions.Add(columnDefinition);
            }

            while (aGrid.RowDefinitions.Count < this.ViewModel.Height)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                aGrid.RowDefinitions.Add(rowDefinition);
            }

            Brush brush = new SolidColorBrush(Colors.DarkBlue);
            for (int y = 0; y < ViewModel.Height; y++)
            {
                for (int x = 0; x < ViewModel.Width; x++)
                {
                    Square square = ViewModel.Squares[y, x];
                    if (square.Top)
                        makeRectangle(brush, x, y, Directions.UP);
                    if (square.Bottom)
                        makeRectangle(brush, x, y, Directions.DOWN);
                    if (square.Left)
                        makeRectangle(brush, x, y, Directions.LEFT);
                    if (square.Right)
                        makeRectangle(brush, x, y, Directions.RIGHT);
                }
            }
        }

        public void makeRectangle (Brush brush,int x, int y, Directions direction)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = brush;
            aGrid.Children.Add(rectangle);
            Grid.SetColumn(rectangle, x);
            Grid.SetRow(rectangle, y);
            
            switch (direction)
            {
                case Directions.UP:
                    rectangle.Height = 8;
                    rectangle.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case Directions.DOWN:
                    rectangle.Height = 8;
                    rectangle.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case Directions.LEFT:
                    rectangle.Width = 8;
                    rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case Directions.RIGHT:
                    rectangle.Width = 8;
                    rectangle.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
            }
            
        }

        private void GoBack()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Move(Moves.DOWN);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.Move(Moves.UP);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Move(Moves.RIGHT);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Move(Moves.LEFT);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Move_Pause_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Move(Moves.PAUSE);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            aGrid.Visibility = aGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            ViewModel.Pause();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Undo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExportLevel();
        }
    }
}
