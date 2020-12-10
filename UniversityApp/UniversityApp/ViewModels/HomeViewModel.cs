using System.Threading.Tasks;
using UniversityApp.Views;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            GoToStudentsCommand = new Command(async () => await GoToStudents());
            GoToInstructorsCommand = new Command(async () => await GoToInstructors());
        }

        public Command GoToStudentsCommand { get; set; }
        public Command GoToInstructorsCommand { get; set; }

        async Task GoToStudents()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new StudentsPage());
        }

        async Task GoToInstructors()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new InstructorsPage());
        }
    }
}