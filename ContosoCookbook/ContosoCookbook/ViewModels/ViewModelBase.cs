using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;

namespace ContosoCookbook.ViewModels
{
    public class ViewModelBase : ReactiveObject, INavigationAware, IConfirmNavigation, IDestructible, IInitialize
    {        
  
        public ViewModelBase()
        {
                       
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { this.RaiseAndSetIfChanged(ref _Title, value); }
        }

        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { this.RaiseAndSetIfChanged(ref _IsBusy, value); }
        }

        public bool IsNotBusy
        {
            get { return !IsBusy; }
        }

        protected void BindBusy(IReactiveCommand command)
        {
            command.IsExecuting.Subscribe(
                x => this.IsBusy = x,
                _ => this.IsBusy = false,
                () => this.IsBusy = false
                );
            ;
        }

        public bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        public void Destroy()
        {
            
        }

       
    }
}
