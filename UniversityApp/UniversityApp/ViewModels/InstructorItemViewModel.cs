using System.Linq;
using System.Threading.Tasks;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services;
using UniversityApp.BL.Services.Implements;
using UniversityApp.Helpers;
using UniversityApp.Views;
using Xamarin.Forms;

namespace UniversityApp.ViewModels
{
    public class InstructorItemViewModel : InstructorDTO
    {
        private BL.Services.IInstructorService instructorService;
        public InstructorItemViewModel()
        {
            EditInstructorCommand = new Command(async () => await EditInstructor());
        }
        public Command EditInstructorCommand { get; set; }
        async Task EditInstructor()
        {
            MainViewModel.GetInstance().EditInstructor = new EditInstructorViewModel(this);
            //await Application.Current.MainPage.Navigation.PushAsync(new EditInstructorPage());
        }
        async Task DeleteInstructor()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Confirm", "Delete Confirm", "Yes", "No");
            if (!answer)
                return;

            var connection = await instructorService.CheckConnection();
            if (!connection)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                return;
            }

            await instructorService.Delete(Endpoints.DELETE_INSTRUCTORS, this.ID);

            await Application.Current.MainPage.DisplayAlert("Message", "The process is successful", "Cancel");
            var instructorViewModel = InstructorsViewModel.GetInstance();
            var instructorDeleted = instructorViewModel.Allinstructors.FirstOrDefault(x => x.ID == this.ID);
            instructorViewModel.Allinstructors.Remove(instructorDeleted);
            instructorViewModel.GetInstructorsByFirstMidName();
        }

    }
}
