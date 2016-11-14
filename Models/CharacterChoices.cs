using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Models
{
    class CharacterChoices
    {
        public List<CharacterType> CharacterTypes { get; set; }

        public static async Task<CharacterChoices> GetAsync()
        {
            using (var reader = new StreamReader(typeof(CharacterChoices).GetTypeInfo().Assembly.GetManifestResourceStream("BaHotHManager.Models.characters.json")))
            {
                var characterChoicesString = await reader.ReadToEndAsync();
                return await Task.Factory.StartNew(() =>
                {
                    return JsonConvert.DeserializeObject<CharacterChoices>(characterChoicesString);
                });
            }
        }
    }
}
