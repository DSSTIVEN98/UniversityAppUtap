using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UniversityApp.Helpers;
using Xamarin.Forms;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using System.Collections.Generic;
using System.Linq;

namespace UniversityApp.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        private BL.Services.ICourseService courseService;
        private ObservableCollection<CoursesItemViewModel> courses;
        private bool isRefreshing;
        private string filter;
        public List<CoursesItemViewModel> AllCourses { get; set; }

        public string Filter
        {
            get { return this.filter; }
            set { this.SetValue(ref this.filter, value); }
        }
        public ObservableCollection<CoursesItemViewModel> Courses
        {
            get { return this.courses; }
            set { this.SetValue(ref this.courses, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public CoursesViewModel()
        {
            instance = this;
            this.CourseService = new CourseService();
            this.RefreshCommand = new Command(async () => await GetCourses());
            this.RefreshCommand.Execute(null);
        }
        private static CoursesViewModel instance;
        public static CoursesViewModel GetInstance()
        {
            if (instance == null)
                return new CoursesViewModel();

            return instance;
        }

        public Command RefreshCommand { get; set; }
        public BL.Services.ICourseService CourseService { get => courseService; set => courseService = value; }

        async Task GetCourses()
        {
            try
            {
                this.IsRefreshing = true;

                var connection = await CourseService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var listCourses = ((await courseService.GetAll(Endpoints.GET_COURSES)).Select(x => ToCourseItemViewModel(x))).OrderByDescending(x => x.Title);
                this.AllCourses = listCourses.ToList();
                this.Courses = new ObservableCollection<CoursesItemViewModel>(listCourses);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }

        private CoursesItemViewModel ToCourseItemViewModel(CourseDTO courseDTO) => new CoursesItemViewModel
        {

            CourseID = courseDTO.CourseID,
            Title = courseDTO.Title,
            Credits = courseDTO.Credits
        };
        public void GetCoursesByTitle()
        {
            var listCourses = this.AllCourses;
            if (!string.IsNullOrEmpty(this.Filter))
                listCourses = listCourses.Where(x => x.Title.ToLower().Contains(this.Filter.ToLower())).ToList();

            this.Courses = new ObservableCollection<CoursesItemViewModel>(listCourses);

        }
    }
}