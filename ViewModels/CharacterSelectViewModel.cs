using BaHotHManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.ViewModels
{
    public class CharacterSelectViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CharacterType> CharacterTypes { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        private Dictionary<CharacterColor, Player> ColorToPlayerMap { get; set; }
        private Stack<byte> PlayerNumbers { get; set; }

        public CharacterSelectViewModel()
        {
            ColorToPlayerMap = new Dictionary<CharacterColor, Player>();

            Players = new ObservableCollection<Player>();
            Players.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                onPropertyChanged(this, nameof(Players));
            };

            CharacterTypes = new ObservableCollection<CharacterType>();
            CharacterTypes.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                onPropertyChanged(this, nameof(CharacterTypes));
            };
        }

        public async Task LoadAsync()
        {
            foreach(var characterType in (await CharacterChoices.GetAsync()).CharacterTypes)
            {
                CharacterTypes.Add(characterType);
            }
            PlayerNumbers = GetPlayerNumbers();
        }

        private Stack<byte> GetPlayerNumbers()
        {
            var stack = new Stack<byte>();
            foreach(byte num in Enumerable.Range(1, CharacterTypes.SelectMany(ct => ct.Characters).Count()).Reverse())
            {
                stack.Push(num);
            }
            return stack;
        }

        public void DeselectCharacter(Character character)
        {
            if (ColorToPlayerMap.ContainsKey(character.Color))
            {
                var player = ColorToPlayerMap[character.Color];
                ColorToPlayerMap.Remove(character.Color);
                Players.Remove(player);
                PlayerNumbers.Push(player.Number);
            }
        }

        public void SelectCharacter(Character character)
        {
            if (!ColorToPlayerMap.ContainsKey(character.Color))
            {
                var playerNumber = PlayerNumbers.Pop();
                var newPlayer = new Player()
                {
                    Name = $"Player {playerNumber}",
                    Number = playerNumber
                };
                newPlayer.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    onPropertyChanged(this, nameof(Player));
                };
                ColorToPlayerMap.Add(character.Color, newPlayer);
                Players.Add(newPlayer);
            }

            ColorToPlayerMap[character.Color].Character = character.Clone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
