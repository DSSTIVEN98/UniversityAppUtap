using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using UniversityApp.Views;
using Xamarin.Forms;
using static BL.Services;

namespace UniversityApp.ViewModels
{
    public class CoursesItemViewModel : CourseDTO
    {
        private BL.Services.ICourseService courseService;
        public CoursesItemViewModel()
        {
            EditCourseCommand = new Command(async () => await EditCourse());
            DeleteCourseCommand = new Command(async () => await DeleteCourse());
        }
        public Command EditCourseCommand { get; set; }
        public Command DeleteCourseCommand { get; set; }
        async Task EditCourse()
        {
            MainViewModel.GetInstance().EditCourse = new EditCourseViewModel(this);
            //await Application.Current.MainPage.Navigation.PushAsync(new EditCoursePage());
        }
        async Task DeleteCourse()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Confirm", "Delete Confirm", "Yes", "No");
            if (!answer)
                return;

            var connection = await courseService.CheckConnection();
            if (!connection)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                return;
            }

            await courseService.Delete(Endpoints.DELETE_COURSES, this.CourseID);

            await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");
            var courseViewModel = CoursesViewModel.GetInstance();
            var courseDeleted = courseViewModel.AllCourses.FirstOrDefault(x => x.CourseID == this.CourseID);
            courseViewModel.AllCourses.Remove(courseDeleted);
            courseViewModel.GetCoursesByTitle();
        }
    }
}
