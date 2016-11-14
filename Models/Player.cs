using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Models
{
    public class Player : INotifyPropertyChanged
    {
        private string name;
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                onPropertyChanged(this, nameof(Name));
            }
        }

        private Character character;
        public Character Character {
            get
            {
                return character;
            }
            set
            {
                character = value;
                character.PropertyChanged += (object sender, PropertyChangedEventArgs args) => {
                    onPropertyChanged(this, nameof(Character));
                };
                onPropertyChanged(this, nameof(Character));
            }
        }

        public byte Number { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
