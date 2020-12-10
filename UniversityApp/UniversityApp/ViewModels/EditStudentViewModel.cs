using System;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class EditStudentViewModel : BaseViewModel
    {
        private BL.Services.IStudentService studentService;
        private StudentDTO student;

        private bool isEnabled;
        private bool isRunning;

        public StudentDTO Student
        {
            get { return this.student; }
            set { this.SetValue(ref this.student, value); }
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

        public EditStudentViewModel(StudentDTO student)
        {
            this.student = student;

            this.studentService = new StudentService();
            this.SaveCommand = new Command(async () => await EditStudent());

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        public Command SaveCommand { get; set; }

        async Task EditStudent()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Student.LastName))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You must enter the field title", "Cancel");
                    return;
                }

                this.IsRunning = true;
                this.IsEnabled = false;

                var connection = await studentService.CheckConnection();
                if (!connection)
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;

                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var studentDTO = new StudentDTO { ID = this.Student.ID, LastName = this.Student.LastName, FirstMidName = this.Student.FirstMidName, EnrollmentDate = this.Student.EnrollmentDate };
                await studentService.Update(Endpoints.PUT_STUDENTS, this.student.ID, studentDTO);

                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");

                this.Student.ID = 0;
                this.Student.LastName = string.Empty;
                this.Student.FirstMidName = string.Empty;
                var EnrollmentDate = DateTime.UtcNow;


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