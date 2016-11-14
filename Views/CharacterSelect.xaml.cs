using BaHotHManager.Models;
using BaHotHManager.ViewModels;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BaHotHManager.Views
{
    public sealed partial class CharacterSelect : Page
    {
        CharacterSelectViewModel ViewModel { get; set; }

        public CharacterSelect()
        {
            InitializeComponent();
            ViewModel = new CharacterSelectViewModel();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Gameplay), ViewModel);
        }

        private void ColorCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var characters = sender as ListView;
            if(characters != null && characters.SelectedItems.Count > 1)
            {
                var removeCharacter = characters.SelectedItems.FirstOrDefault() as Character;
                characters.SelectedItems.RemoveAt(0);
                if(removeCharacter != null) ViewModel.DeselectCharacter(removeCharacter);
            }

            var removedCharacter = e.RemovedItems.FirstOrDefault() as Character;
            if (removedCharacter != null)
            {
                ViewModel.DeselectCharacter(removedCharacter);
            }

            var addedCharacter = e.AddedItems.FirstOrDefault() as Character;
            if (addedCharacter != null) {
                ViewModel.SelectCharacter(addedCharacter);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var destination = e.Content;
            if(destination is Gameplay)
            {
                (destination as Gameplay).ViewModel.StartGame(ViewModel);
            }
            base.OnNavigatedFrom(e);
        }
    }
}
