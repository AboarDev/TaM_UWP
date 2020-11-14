using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TamGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        ObservableCollection<LevelData> Levels;
        public BlankPage1()
        {
            this.InitializeComponent();
            Levels = new ObservableCollection<LevelData>();
            Levels.Add(new LevelData("Start", 7, 7,
                             "0303 0000 0001"
                         + " 1001 1000 1000 1000 1000 1000 1100"
                         + " 0001 0000 0000 0000 0000 0000 0100"
                         + " 0001 0010 0010 0010 0010 0010 0100"
                         + " 0001 1010 1010 1010 1010 1010 0100"
                         + " 0001 1000 1000 1000 1000 1000 0100"
                         + " 0001 0000 0000 0000 0000 0000 0100"
                         + " 0011 0010 0010 0010 0010 0010 0110"));
            Levels.Add(new LevelData("Test",7,7,
                             "0303 0606 0001"
                          + " 1001 1111 1000 1000 1000 1000 1100"
                          + " 0001 1111 0000 0000 0000 0000 0100"
                          + " 0001 0000 0000 0000 0000 0000 0100"
                          + " 0001 0000 0000 0000 0000 0000 0100"
                          + " 0001 0000 0000 0000 0000 0000 0100"
                          + " 0001 0000 0000 0000 0000 0000 0100"
                          + " 0011 0010 0010 0010 0010 0010 0110"));
            Levels.Add(new LevelData("Mike's Starting Level", 4, 3, "0001 0201 0103"
                + " 1001 1010 1100 0001"
                + " 0001 1110 0001 1010"
                + " 0011 1010 0110 1011"));
            LevelsList.ItemsSource = Levels;
            GetFile();
        }

        public List<LevelData> LevelDatas;

        public async void GetFile ()
        {
            FileHelper<LevelData> levelDataHelper = new FileHelper<LevelData>("MyGame");
            try
            {
                string iii = await levelDataHelper.LoadFile();

                LevelData levelData = await levelDataHelper.ParseFile(iii);

                levelData.Name = "Saved Level";

                Levels.Add(levelData);

                LevelsList.ItemsSource = Levels;
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //textBlock.Text = $"Hi, {e.Parameter.ToString()}";
            }
            else
            {
                //textBlock.Text = "Hi!";
            }
            base.OnNavigatedTo(e);
        }

        private void LevelsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            LevelData levelData = (LevelData) e.ClickedItem;
            this.Frame.Navigate(typeof(MainPage),levelData);
        }
    }
}
