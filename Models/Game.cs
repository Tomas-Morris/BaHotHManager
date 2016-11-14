using BaHotHManager.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Models
{
    public class Game : INotifyPropertyChanged
    {
        public ObservableCollection<Player> PlayOrder { get; set; }

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                onPropertyChanged(this, nameof(CurrentPlayer));
            }
        }

        public Game()
        {
            PlayOrder = new ObservableCollection<Player>();
            PlayOrder.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs args) =>
            {
                onPropertyChanged(this, nameof(PlayOrder));
            };
        }

        public Player NextTurn()
        {
            var currentPlayer = PlayOrder.First();
            PlayOrder.RemoveAt(0);
            PlayOrder.Add(currentPlayer);
            CurrentPlayer = PlayOrder.First();
            return CurrentPlayer;
        }

        public void Start(ObservableCollection<Player> players)
        {
            var firstPlayerIndex = players.IndexOf(players.OrderBy(p => p.Character.Birthday).FirstOrDefault());
            foreach (var player in players.Skip(firstPlayerIndex)) PlayOrder.Add(player);
            foreach (var player in players.Take(firstPlayerIndex)) PlayOrder.Add(player);
            CurrentPlayer = PlayOrder.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
