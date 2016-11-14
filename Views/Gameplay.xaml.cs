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
using Windows.UI.Composition;
using System.Numerics;
using BaHotHManager.Models;
using BaHotHManager.Helpers;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace BaHotHManager.Views
{
    public sealed partial class Gameplay : Page
    {
        public GameplayViewModel ViewModel { get; set; }

        public Gameplay()
        {
            ViewModel = new GameplayViewModel();
            InitializeComponent();
            ApplyEffectFlyout.Closed += ApplyEffectReset;
            Application.Current.Suspending += SuspendAsync;
            SaveGames.SelectionChanged += LoadGameAsync;
        }

        private async void LoadGameAsync(object sender, SelectionChangedEventArgs e)
        {
            if (SaveGames.SelectedItems.Count != 1) return;
            await ViewModel.SaveAsync();
            splitter.IsPaneOpen = false;
            var newGame = SaveGames.SelectedItems.FirstOrDefault() as GameplayViewModel;
            await newGame?.LoadSaveGamesAsync();
            if (newGame != null)
            {
                ViewModel = newGame;
            }
            SaveGames.SelectedItems.Clear();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var content = e.Parameter;
            if(content is GameplayViewModel)
            {
                ViewModel = content as GameplayViewModel;
            }
            await ViewModel.LoadSaveGamesAsync();
        }

        private async void SuspendAsync(object sender, SuspendingEventArgs e)
        {
            await StateManager.SuspendAsync(typeof(Gameplay), ViewModel, e);
        }

        private void ApplyEffectReset(object sender, object e)
        {
            AffectedPlayers.SelectedItems.Clear();
            SpeedEffect.Value = 0;
            MightEffect.Value = 0;
            KnowledgeEffect.Value = 0;
            SanityEffect.Value = 0;
        }

        private void NextTurn(object sender, RoutedEventArgs e)
        {
            ViewModel.NextTurn();
        }

        private void AffectedPlayers_Loaded(object sender, RoutedEventArgs e)
        {
            var affectedPlayerListView = sender as ListView;
            if(affectedPlayerListView?.Items?.Count() > 0)
                affectedPlayerListView.SelectedIndex = 0;
        }

        private void ApplyEffect(object sender, RoutedEventArgs e)
        {
            ApplyEffectFlyout.Hide();
            ViewModel.ApplyEffect(AffectedPlayers.SelectedItems.Cast<Player>(), SpeedEffect.Value, MightEffect.Value, KnowledgeEffect.Value, SanityEffect.Value);
        }

        private async void SaveGameAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync();
        }

        private void TogglePane(object sender, RoutedEventArgs e)
        {
            splitter.IsPaneOpen = !splitter.IsPaneOpen;
        }
    }
}
