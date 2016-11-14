using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Models
{
    public class Character : INotifyPropertyChanged
    {

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public CharacterColor Color { get; set; }

        private byte speedPosition;
        public byte SpeedPosition { get { return speedPosition; }
            set
            {
                if(value >= (SpeedValues?.Count() ?? 9))
                {
                    speedPosition = (byte)(SpeedValues.Count() - 1);
                }
                else if(value <= 0)
                {
                    speedPosition = 0;
                }
                else
                {
                    speedPosition = value;
                }
                onPropertyChanged(this, nameof(Speed));
            }
        }
        public IEnumerable<byte> SpeedValues { get; set; }
        [JsonIgnore]
        public byte Speed
        {
            get
            {
                return SpeedValues.ElementAt(SpeedPosition);
            }
        }

        private byte mightPosition;
        public byte MightPosition
        {
            get { return mightPosition; }
            set
            {
                if (value >= (MightValues?.Count() ?? 9))
                {
                    mightPosition = (byte)(MightValues.Count() - 1);
                }
                else if (value <= 0)
                {
                    mightPosition = 0;
                }
                else
                {
                    mightPosition = value;
                }
                onPropertyChanged(this, nameof(Might));
            }
        }
        public IEnumerable<byte> MightValues { get; set; }
        [JsonIgnore]
        public byte Might {
            get
            {
                return MightValues.ElementAt(MightPosition);
            }
        }

        private byte sanityPosition;
        public byte SanityPosition
        {
            get { return sanityPosition; }
            set
            {
                if (value >= (SanityValues?.Count() ?? 9))
                {
                    sanityPosition = (byte)(SanityValues.Count() - 1);
                }
                else if (value <= 0)
                {
                    sanityPosition = 0;
                }
                else
                {
                    sanityPosition = value;
                }
                onPropertyChanged(this, nameof(Sanity));
            }
        }
        public IEnumerable<byte> SanityValues { get; set; }
        [JsonIgnore]
        public byte Sanity
        {
            get
            {
                return SanityValues.ElementAt(SanityPosition);
            }
        }

        private byte knowledgePosition;
        public byte KnowledgePosition
        {
            get { return knowledgePosition; }
            set
            {
                if (value >= (KnowledgeValues?.Count() ?? 9))
                {
                    knowledgePosition = (byte)(KnowledgeValues.Count() - 1);
                }
                else if (value <= 0)
                {
                    knowledgePosition = 0;
                }
                else
                {
                    knowledgePosition = value;
                }
                onPropertyChanged(this, nameof(Knowledge));
            }
        }
        public IEnumerable<byte> KnowledgeValues { get; set; }
        [JsonIgnore]
        public byte Knowledge
        {
            get
            {
                return KnowledgeValues.ElementAt(KnowledgePosition);
            }
        }

        public Character Clone()
        {
            return (Character)MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
