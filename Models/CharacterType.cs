using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Models
{
    public class CharacterType
    {
        public CharacterColor Color { get; set; }

        public IEnumerable<Character> Characters { get; set; }
    }

    public enum CharacterColor
    {
        White,
        Purple,
        Red,
        Yellow,
        Green,
        Blue
    }
}
