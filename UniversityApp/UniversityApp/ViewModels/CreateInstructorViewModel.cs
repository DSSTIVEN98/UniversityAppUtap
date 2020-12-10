using System;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using UniversityApp.Views;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class CreateInstructorViewModel : BaseViewModel
    {
        private BL.Services.IInstructorService instructorService;
        private int iD;
        private string lastName;
        private string firstMidName;
        private DateTime hireDate;
        //private string fullName;

        private bool isEnabled;
        private bool isRunning;

        public int ID
        {
            get { return this.iD; }
            set { this.SetValue(ref this.iD, value); }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.SetValue(ref this.lastName, value); }
        }

        public String FirstMidName
        {
            get { return this.firstMidName; }
            set { this.SetValue(ref this.firstMidName, value); }
        }
        public DateTime HireDate
        {
            get { return this.hireDate; }
            set { this.SetValue(ref this.hireDate, value); }
        }
        //public String FullName
        //{
            //get { return this.fullName; }
            //set { this.SetValue(ref this.fullName, value); }
        //}
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

        public CreateInstructorViewModel()
        {
            this.instructorService = new InstructorService();
            this.SaveCommand = new Command(async () => await CreateInstructor());

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        public Command SaveCommand { get; set; }

        async Task CreateInstructor()
        {
            try
            {
                if (string.IsNullOrEmpty(this.LastName))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You must enter the field FullName", "Cancel");
                    return;
                }

                this.IsRunning = true;
                this.IsEnabled = false;

                var connection = await instructorService.CheckConnection();
                if (!connection)
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var instructorDTO = new InstructorDTO {LastName = this.LastName, FirstMidName = this.FirstMidName, HireDate = this.HireDate};
                await instructorService.Create(Endpoints.POST_INSTRUCTORS, instructorDTO);

                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");
                Application.Current.MainPage = new NavigationPage(new InstructorsPage());

                this.LastName = string.Empty;
                this.FirstMidName = string.Empty;
                var HireDate = DateTime.UtcNow;

                //Application.Current.MainPage = new NavigationPage(new InstructorPage());
                //var enrollmentDate = DateTime.UtcNow;
                //this.EnrollmentDate = DateTime.UtcNow;
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