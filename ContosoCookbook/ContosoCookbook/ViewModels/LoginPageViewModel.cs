using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ContosoCookbook.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public INavigationService NavigationService { get; set; }
        public IPageDialogService DialogService { get; set; }
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)          
        {
            DialogService = pageDialogService;
            NavigationService = navigationService;

            Title = "Login";

            LoginCommand = ReactiveCommand.CreateFromTask
                (OnLoginCommandExecuted,
                 this.WhenAnyValue(
                x => x.UserName,
                x => x.Password,               
                (username, password) =>
                {
                    if (String.IsNullOrEmpty(username))
                        return false;
                    if (String.IsNullOrEmpty(password))
                        return false;
                   
                    return true;

                }));
            this.BindBusy(LoginCommand);
            IObservable<Exception> ex = this.LoginCommand.ThrownExceptions;
            ex.Subscribe(x => Debug.WriteLine("Prism And RXUI Exception: " + x.Message));

        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { this.RaiseAndSetIfChanged(ref _userName, value); }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

  

        private async Task OnLoginCommandExecuted()
        {
            
            if (Password == "1234")
            {
               await NavigationService.NavigateAsync("/NavigationPage/MainPage");
              

            }
            else 
            {
                await DialogService.DisplayAlertAsync("Login", "Authentication Failed", "OK");
                return;
            }

        
        }

    }
}

