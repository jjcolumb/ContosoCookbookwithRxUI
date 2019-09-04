using ContosoCookbook.Business;
using ContosoCookbook.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace ContosoCookbook.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Dictionary<int, List<Recipe>> _recipeList = new Dictionary<int, List<Recipe>>();
        List<Recipe> filteredData = null;
        private IRecipeService _recipeService { get; }
        private INavigationService NavigationService { get; }
        public DelegateCommand LogOutCommand { get; }
        public DelegateCommand<Recipe> RecipeSelectedCommand { get; }
        public MainPageViewModel(INavigationService navigationService, IRecipeService recipeService) 
        {
            
            NavigationService = navigationService;
             _recipeService = recipeService;
            RecipeSelectedCommand = new DelegateCommand<Recipe>(RecipeSelected);
            LogOutCommand = new DelegateCommand(LogOut);

            (this).WhenAnyValue(x => x.SearchTerm)
            .Do(x =>
             {
               if (string.IsNullOrEmpty(x))
               {
                     SearchTerm = " ";
                     //HACK Error if updating RecipeGroups from here: Unable to activate instance of type Xamarin.Forms.Platform.Android.SearchBarRenderer from native
                     //https://github.com/xamarin/Xamarin.Forms/issues/6550
                 }
             })
            .Where(x => !string.IsNullOrEmpty(x))
            .Throttle(TimeSpan.FromSeconds(1.5), RxApp.MainThreadScheduler)
            .Subscribe(searchTerm =>
            {
               
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    Debug.WriteLine($"Searching for: {searchTerm}");
                    try
                    {
                        var index = RecipeGroups.IndexOf(SelectedTabItem);
                        if (searchTerm.Length<3)
                        {
                            if(RecipeGroups!= null)
                                filteredData = _recipeList[index];
                        }
                        else
                        {
                            filteredData = _recipeList[index].Where(d => d.ToString().IndexOf(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        }
                    
                        if (filteredData != null)
                        {
                          
                            RecipeGroups.Select(d => d.Recipes).ToList()[index].Clear();
                            RecipeGroups.Select(d => d.Recipes).ToList()[index].AddRange(filteredData);
                            RecipeGroup recipeGroup = RecipeGroups[index];
                            RecipeGroups.Insert(index, recipeGroup);
                            RecipeGroups.RemoveAt(index+1);
                        }

                    }
                    catch (Exception exception)
                    {

                        Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                        if (exception.InnerException != null)
                        {
                            Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));

                        }
                        Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
                    }
                }
            });
        }

        private RecipeGroup _SelectedTabItem;

        public RecipeGroup SelectedTabItem
        {
            get { return _SelectedTabItem; }
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedTabItem, value);                
            }
        }



        private string _SearchTerm;

        public string SearchTerm
        {
            get { return _SearchTerm; }
            set { this.RaiseAndSetIfChanged(ref _SearchTerm, value); }
        }

        private ObservableCollection<RecipeGroup> _recipeGroups;
        public ObservableCollection<RecipeGroup> RecipeGroups
        {
            get => _recipeGroups;
            set => this.RaiseAndSetIfChanged(ref _recipeGroups, value);
        }

        private ObservableCollection<RecipeGroup> _recipeGroupsOriginal;
        public ObservableCollection<RecipeGroup> RecipeGroupsOriginal
        {
            get => _recipeGroupsOriginal;
            set => this.RaiseAndSetIfChanged(ref _recipeGroupsOriginal, value);
        }
        private async void LogOut()
        {
           await NavigationService.NavigateAsync("/LoginPage");
        }


        private async void RecipeSelected(Recipe recipe)
        {
            var p = new NavigationParameters
            {
                { "recipe", recipe }
            };

            await NavigationService.NavigateAsync("RecipePage", p);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            if (RecipeGroups == null)
            {
                RecipeGroups = new ObservableCollection<RecipeGroup>(await _recipeService.GetRecipeGroups());
                RecipeGroupsOriginal = new ObservableCollection<RecipeGroup>(await _recipeService.GetRecipeGroups());

                int i = 0;
                foreach(var detail in RecipeGroupsOriginal)
                {
                    _recipeList.Add(i, detail.Recipes);
                    i++;
                }
                
            }

        }
    }
}