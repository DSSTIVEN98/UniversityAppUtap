using System;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class CreateCourseViewModel : BaseViewModel
    {
        private BL.Services.ICourseService courseService;
        private int courseID;
        private string title;
        private int credits;
        private bool isEnabled;
        private bool isRunning;

        public int CourseID
        {
            get { return this.courseID; }
            set { this.SetValue(ref this.courseID, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetValue(ref this.title, value); }
        }

        public int Credits
        {
            get { return this.credits; }
            set { this.SetValue(ref this.credits, value); }
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

        public CreateCourseViewModel()
        {
            this.courseService = new CourseService();
            this.SaveCommand = new Command(async () => await CreateCourse());

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        public Command SaveCommand { get; set; }

        async Task CreateCourse()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Title))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You must enter the field title", "Cancel");
                    return;
                }

                this.IsRunning = true;
                this.IsEnabled = false;

                var connection = await courseService.CheckConnection();
                if (!connection)
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var courseDTO = new CourseDTO { CourseID = this.CourseID, Title = this.Title, Credits = this.Credits };
                await courseService.Create(Endpoints.POST_COURSES, courseDTO);

                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");

                this.CourseID = this.Credits = 0;
                this.Title = string.Empty;

                //Application.Current.MainPage = new NavigationPage(new CoursesPage());
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