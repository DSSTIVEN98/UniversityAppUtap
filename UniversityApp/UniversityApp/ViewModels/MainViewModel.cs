using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Views;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class MainViewModel
    {
        public CoursesViewModel Courses { get; set; }
        public CourseInstructorsViewModel CourseInstructors { get; set; }
        public DepartmentsViewModel Departments { get; set; }
        public EnrollmentsViewModel Enrollments { get; set; }
        public InstructorsViewModel Instructors { get; set; }
        public StudentsViewModel Students { get; set; }
        public OfficeAssignmentsViewModel OfficeAssignments { get; set; }
        public CreateCourseViewModel CreateCourse { get; set; }
        public CreateStudentViewModel CreateStudent { get; set; }
        public CreateInstructorViewModel CreateInstructor { get; set; }
        public LoginViewModel Login { get; set; }
        public HomeViewModel Home { get; set; }
        public EditCourseViewModel EditCourse { get; set; }
        public EditInstructorViewModel EditInstructor { get; set; }
        public EditStudentViewModel EditStudent { get; set; }


        public MainViewModel()
        {

            instance = this;
            Courses = new CoursesViewModel();
            CourseInstructors = new CourseInstructorsViewModel();
            Departments = new DepartmentsViewModel();
            Enrollments = new EnrollmentsViewModel();
            Instructors = new InstructorsViewModel();
            Students = new StudentsViewModel();
            OfficeAssignments = new OfficeAssignmentsViewModel();
            CreateCourse = new CreateCourseViewModel();
            CreateStudent = new CreateStudentViewModel();
            CreateInstructor = new CreateInstructorViewModel();
            Login = new LoginViewModel();
            Home = new HomeViewModel();


            CreateCourseCommand = new Command(async () => await GoToCreateCourse());
            CreateStudentCommand = new Command(async () => await GoToCreateStudent());
            CreateInstructorCommand = new Command(async () => await GoToCreateInstructor());
        }

        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
                return new MainViewModel();

            return instance;
        }

        public Command CreateCourseCommand { get; set; }
        public async Task GoToCreateCourse()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CreateCoursePage());
        }

        public Command CreateStudentCommand { get; set; }
        public async Task GoToCreateStudent()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CreateStudentPage());
        }

        public Command CreateInstructorCommand { get; set; }
        public async Task GoToCreateInstructor()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CreateInstructorPage());
        }
    }
}
