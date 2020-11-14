using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TamLib;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Media.Playback;
using Windows.Media.Core;

namespace TamGui
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        int moveCount;

        MediaPlayer mediaPlayer;
        readonly Action Back;

        static GameViewModel ()
        {
            timeSpan = new TimeSpan(0, 0, 1);
        }

        public GameViewModel (Action back)
        {
            Back = back;
            Game = new Game();
            Paused = false;
            mediaPlayer = new MediaPlayer();
        }

        public string Moves { get => $"Moves: {MoveCount}"; }
        private static TimeSpan timeSpan;
        public TimeSpan totalTime;
        public string Times { get => String.Format("Time: {0:mm\\:ss}", totalTime); }
        public Square[,] Squares { get => Game.Squares; }

        public bool Paused { get; set; }

        public void Move (Moves move)
        {
            bool moved = Game.MoveTheseus(move);
            if (!moved)
            {
                return;
            }
            Position theseusOld;
            Position theseusNew;
            Position minotaurOld;
            Position minotaurNew;
            if (Game.MoveCount != this.MoveCount)
                this.MoveCount++;
            theseusOld = new Position { X = TheseusX, Y = TheseusY };
            theseusNew = new Position { X = Game.TheseusPosition.X, Y = Game.TheseusPosition.Y };
            if (TheseusX != Game.TheseusPosition.X)
            {
                TheseusX = Game.TheseusPosition.X;
                OnPropertyChanged(nameof(TheseusX));
            }
            if (TheseusY != Game.TheseusPosition.Y)
            {
                TheseusY = Game.TheseusPosition.Y;
                OnPropertyChanged(nameof(TheseusY));
            }
            this.Game.MoveMinotaur();
            this.Game.MoveMinotaur();
            minotaurOld = new Position { X = MinotaurX, Y = MinotaurY };
            minotaurNew = new Position { X = Game.MinotaurPosition.X, Y = Game.MinotaurPosition.Y };
            if (MinotaurX != Game.MinotaurPosition.X)
            {
                MinotaurX = Game.MinotaurPosition.X;
                OnPropertyChanged(nameof(MinotaurX));
            }
            if (MinotaurY != Game.MinotaurPosition.Y)
            {
                MinotaurY = Game.MinotaurPosition.Y;
                OnPropertyChanged(nameof(MinotaurY));
            }
            Game.AddMove(theseusOld,theseusNew,minotaurOld,minotaurNew,totalTime);
            int dialog = 0;
            if (Game.HasTheseusWon)
            {
                dialog = 1;
            } else if(Game.HasMinotaurWon){
                dialog = 2;
            }
            if (dialog != 0)
            {
                EndDialog(dialog == 1 ? true : false);
            }
        }

        public async void PlaySound (bool won)
        {
            Uri uri;
            if (won)
            {
                uri = new Uri("ms-appx:///Assets/move.mp3");
            } else
            {
                uri = new Uri("ms-appx:///Assets/completed.mp3");
            }
            mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            mediaPlayer.Play();
        }

        public async void EndDialog (bool won)
        {
            PlaySound(won);
            string message = (won) ? "won!" : "lost! Wow, you might be bad at this";
            string title = (won) ? "won" : "over";
            ContentDialog gameOverDialog = new ContentDialog
            {
                Title = $"Game {title}",
                Content = $"You have {message}",
                SecondaryButtonText = "Undo",
                PrimaryButtonText = "Back to Selection",
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await gameOverDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Back();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                Undo();
            }
        }
        // was static
        public static void StartTimer (TimeSpan intervalInMinutes, Action action)
        {
            //was local var
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = intervalInMinutes
            };
            timer.Tick += (s, e) => action();
            timer.Start();
        }

        public void Timer ()
        {
            totalTime = new TimeSpan(0, 0, 0);
            //timeSpan.ToString("g");
            StartTimer(timeSpan, async () =>
            {
                if (!Paused)
                {
                    totalTime += timeSpan;
                    OnPropertyChanged(nameof(Times));


                }
            });
        }

        public void AddLevel (LevelData levelData)
        {
            Game.AddLevel(levelData.Name, levelData.Width, levelData.Height, levelData.Schema);

            if (levelData.Moves != null)
            {
                //Game.SetMoves(levelData.Moves);
                //Game.Current.MoveCount = levelData.Moves.Count;

                //MoveCount = Game.Current.MoveCount;
                //OnPropertyChanged(nameof(Moves));
            }

            Width = Game.LevelWidth;
            Height = Game.LevelHeight;

            LevelName = Game.CurrentLevelName;
            OnPropertyChanged(nameof(LevelName));

            TheseusX = Game.TheseusPosition.X;
            OnPropertyChanged(nameof(TheseusX));

            TheseusY = Game.TheseusPosition.Y;
            OnPropertyChanged(nameof(TheseusY));

            MinotaurX = Game.MinotaurPosition.X;
            OnPropertyChanged(nameof(MinotaurX));

            MinotaurY = Game.MinotaurPosition.Y;
            OnPropertyChanged(nameof(MinotaurY));

            ExitX = Game.ExitPosition.X;
            OnPropertyChanged(nameof(ExitX));

            ExitY = Game.ExitPosition.Y;
            OnPropertyChanged(nameof(ExitY));
        }

        public void Restart ()
        {
            totalTime = totalTime - totalTime;
        }

        public void ExportLevel ()
        {
            if (Game.Current != null)
            {
                LevelData levelData = new LevelData(Game.Current,Game.TheseusPosition,Game.MinotaurPosition,Game.ExitPosition);
                FileHelper < LevelData > levelDataHelper = new FileHelper<LevelData>("MyGame");
                levelDataHelper.MakeFile(levelData);
                //LevelData parsed = await levelDataHelper.ReadFile();
                //await Task.Delay(0);
                //int height = parsed.Height;
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string LevelName { get; set; }

        public void Increment ()
        {
            this.MoveCount++;
        }

        public Game Game { get; set; }
        public int MoveCount
        {
            get { return this.moveCount; }
            set
            {
                this.moveCount = value;
                this.OnPropertyChanged();
                OnPropertyChanged(nameof(Moves));
            }
        }
        public int TheseusX { get; set; }
        public int TheseusY { get; set; }
        public int MinotaurX { get; set; }
        public int MinotaurY { get; set; }
        public int ExitX { get; set; }
        public int ExitY { get; set; }
        public Position ExitPos { get; set; }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Pause ()
        {
            Paused = !Paused;
        }
        
        public void Undo ()
        {
            Move move = Game.Undo();
            MoveCount = Game.MoveCount;
            OnPropertyChanged(nameof(Moves));
            if (move.TheseusMoved)
            {
                TheseusX = Game.TheseusPosition.X;
                OnPropertyChanged(nameof(TheseusX));
                TheseusY = Game.TheseusPosition.Y;
                OnPropertyChanged(nameof(TheseusY));
            }
            if (move.MinotaurMoved)
            {
                MinotaurX = Game.MinotaurPosition.X;
                OnPropertyChanged(nameof(MinotaurX));
                MinotaurY = Game.MinotaurPosition.Y;
                OnPropertyChanged(nameof(MinotaurY));

            }
        }
    }
}
