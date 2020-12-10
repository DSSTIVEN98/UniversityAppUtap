using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UniversityApp.Helpers;
using Xamarin.Forms;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;

namespace UniversityApp.ViewModels
{
    public class OfficeAssignmentsViewModel : BaseViewModel
    {
        private BL.Services.IOfficeAssignmentService officeAssignmentService;
        private ObservableCollection<OfficeAssignmentDTO> officeAssignments;
        private bool isRefreshing;

        public ObservableCollection<OfficeAssignmentDTO> OfficeAssignments
        {
            get { return this.officeAssignments; }
            set { this.SetValue(ref this.officeAssignments, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public OfficeAssignmentsViewModel()
        {
            this.OfficeAssignmentService = new OfficeAssignmentService();
            this.RefreshCommand = new Command(async () => await GetOfficeAssignments());
            this.RefreshCommand.Execute(null);
        }

        public Command RefreshCommand { get; set; }
        public BL.Services.IOfficeAssignmentService OfficeAssignmentService { get => officeAssignmentService; set => officeAssignmentService = value; }

        async Task GetOfficeAssignments()
        {
            try
            {
                this.IsRefreshing = true;

                var connection = await OfficeAssignmentService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var listOfficeAssignment = await OfficeAssignmentService.GetAll(Endpoints.GET_OFFICEASSIGNMENTS);
                this.OfficeAssignments = new ObservableCollection<OfficeAssignmentDTO>(listOfficeAssignment);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
}
