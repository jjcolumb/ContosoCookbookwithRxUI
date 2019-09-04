using System;
using ContosoCookbook.Business;
using Prism.Navigation;
using ReactiveUI;

namespace ContosoCookbook.ViewModels
{
    public class RecipePageViewModel : ViewModelBase
    {
        private Recipe _recipe;
        public Recipe Recipe
        {
            get => _recipe;
            set => this.RaiseAndSetIfChanged(ref _recipe, value);
        }

        public RecipePageViewModel()
        {

        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("recipe"))
                Recipe = parameters.GetValue<Recipe>("recipe");
        }
    }
}