using System;
using System.Threading.Tasks;
using UniversityApp.Views;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string email;
        private string password;
        private bool isEnabled;
        private bool isRunning;

        public string Email
        {
            get { return this.email; }
            set { this.SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { this.SetValue(ref this.password, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        #endregion

        public LoginViewModel()
        {
            this.IsRunning = false;
            this.IsEnabled = true;

            LoginCommand = new Command(async () => await GoToHome());
        }

        public Command LoginCommand { get; set; }
        async Task GoToHome()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You must enter the fields", "Cancel");
                    return;
                }

                this.IsRunning = true;
                this.IsEnabled = false;

                if (this.Email.Equals("daniel.camilo.0ds@utap.edu.co") && this.Password.Equals("123456"))
                    Application.Current.MainPage = new NavigationPage(new HomePage());
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Email or Password incorrect", "Cancel");

                this.IsRunning = false;
                this.IsEnabled = true;
            }
            catch (Exception ex)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
}
