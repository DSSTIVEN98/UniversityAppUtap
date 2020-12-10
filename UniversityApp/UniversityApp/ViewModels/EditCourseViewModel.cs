using System;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class EditCourseViewModel : BaseViewModel
    {
        private BL.Services.ICourseService courseService;
        private CourseDTO course;

        private bool isEnabled;
        private bool isRunning;

        public CourseDTO Course
        {
            get { return this.course; }
            set { this.SetValue(ref this.course, value); }
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

        public EditCourseViewModel(CourseDTO course)
        {
            this.course = course;

            this.courseService = new CourseService();
            this.SaveCommand = new Command(async () => await EditCourse());

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        public Command SaveCommand { get; set; }

        async Task EditCourse()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Course.Title))
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

                var courseDTO = new CourseDTO { CourseID = this.Course.CourseID, Title = this.Course.Title, Credits = this.Course.Credits };
                await courseService.Update(Endpoints.PUT_COURSES, this.Course.CourseID, courseDTO);

                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");

                this.Course.CourseID = this.Course.Credits = 0;
                this.Course.Title = string.Empty;

                await Application.Current.MainPage.Navigation.PopAsync();
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