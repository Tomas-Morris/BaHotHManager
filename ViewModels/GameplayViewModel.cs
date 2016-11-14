using BaHotHManager.Helpers;
using BaHotHManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BaHotHManager.ViewModels
{
    public class GameplayViewModel : INotifyPropertyChanged
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Game Game { get; set; }

        public DateTime LastSavedDate { get; set; }

        [JsonIgnore]
        public ObservableCollection<GameplayViewModel> SaveGames { get; set; }

        public GameplayViewModel()
        {
            Game = new Game();
            Game.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                onPropertyChanged(this, nameof(Game));
            };
            SaveGames = new ObservableCollection<GameplayViewModel>();
            SaveGames.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs args) =>
            {
                onPropertyChanged(this, nameof(SaveGames));
            };
        }

        public void StartGame(CharacterSelectViewModel characterSelectViewModel)
        {
            Game.Start(characterSelectViewModel.Players);
        }

        public void NextTurn()
        {
            Game.NextTurn();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ApplyEffect(IEnumerable<Player> players, double speedEffect, double mightEffect, double knowledgeEffect, double sanityEffect)
        {
            foreach(var player in players)
            {
                player.Character.SpeedPosition += (byte)speedEffect;
                player.Character.MightPosition += (byte)mightEffect;
                player.Character.KnowledgePosition += (byte)knowledgeEffect;
                player.Character.SanityPosition += (byte)sanityEffect;
            }
        }

        public override string ToString()
        {
            return string.Join(",", Game.PlayOrder.Select(p => p.Name));
        }

        internal async Task SaveAsync()
        {
            await StateManager.SaveAsync(this);
            LastSavedDate = DateTime.Now;
        }

        internal async Task LoadSaveGamesAsync()
        {
            SaveGames.Clear();
            foreach (var saveGame in await StateManager.LoadSavesAsync()) SaveGames.Add(saveGame);
        }
    }
}
