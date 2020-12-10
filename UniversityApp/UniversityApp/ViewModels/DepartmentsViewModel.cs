using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UniversityApp.Helpers;
using Xamarin.Forms;
using UniversityApp.BL.DTOs;
using UniversityApp.BL.Services.Implements;

namespace UniversityApp.ViewModels
{
    public class DepartmentsViewModel : BaseViewModel
    {
        private BL.Services.IDepartmentService departmentService;
        private ObservableCollection<DepartmentsDTO> departments;
        private bool isRefreshing;

        public ObservableCollection<DepartmentsDTO> Departments
        {
            get { return this.departments; }
            set { this.SetValue(ref this.departments, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public DepartmentsViewModel()
        {
            this.DepartmentService = new DepartmentService();
            this.RefreshCommand = new Command(async () => await GetDepartments());
            this.RefreshCommand.Execute(null);
        }

        public Command RefreshCommand { get; set; }
        public BL.Services.IDepartmentService DepartmentService { get => departmentService; set => departmentService = value; }

        async Task GetDepartments()
        {
            try
            {
                this.IsRefreshing = true;

                var connection = await DepartmentService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var listDepartment = await DepartmentService.GetAll(Endpoints.GET_DEPARTMENTS);
                this.Departments= new ObservableCollection<DepartmentsDTO>(listDepartment);
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
