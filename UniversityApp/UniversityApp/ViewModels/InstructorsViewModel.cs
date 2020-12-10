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
    public class InstructorsViewModel : BaseViewModel
    {
        private BL.Services.IInstructorService instructorService;
        private ObservableCollection<InstructorItemViewModel> instructors;
        private bool isRefreshing;
        private string filter;
        public List<InstructorItemViewModel> Allinstructors { get; set; }

        public string Filter
        {
            get { return this.filter; }
            set { this.SetValue(ref this.filter, value);
                this.GetInstructorsByFirstMidName();
            }
        }


        public ObservableCollection<InstructorItemViewModel> Instructors
        {
            get { return this.instructors; }
            set { this.SetValue(ref this.instructors, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public InstructorsViewModel()
        {
            instance = this;
            this.InstructorService = new InstructorService();
            this.RefreshCommand = new Command(async () => await GetInstructors());
            this.RefreshCommand.Execute(null);
        }
        private static InstructorsViewModel instance;
        public static InstructorsViewModel GetInstance()
        {
            if (instance == null)
                return new InstructorsViewModel();

            return instance;
        }

        public Command RefreshCommand { get; set; }

        public BL.Services.IInstructorService InstructorService { get => instructorService; set => instructorService = value; }

        async Task GetInstructors()
        {
            try
            {
                this.IsRefreshing = true;

                var connection = await InstructorService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "No internet connection", "Cancel");
                    return;
                }

                var listInstructors = ((await instructorService.GetAll(Endpoints.GET_INSTRUCTORS)).Select(x => ToInstructorItemViewModel(x))).OrderByDescending(x => x.FirstMidName);
                this.Allinstructors = listInstructors.ToList();
                this.Instructors = new ObservableCollection<InstructorItemViewModel>(listInstructors);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
            }
        
            
        }
        private InstructorItemViewModel ToInstructorItemViewModel(InstructorDTO instructorDTO) => new InstructorItemViewModel
        {

            ID = instructorDTO.ID,
            LastName = instructorDTO.LastName,
            FirstMidName = instructorDTO.FirstMidName,
            HireDate = instructorDTO.HireDate
        };
        public void GetInstructorsByFirstMidName()
        {
            var listInstructors = this.Allinstructors;
            if (!string.IsNullOrEmpty(this.Filter))
                listInstructors = listInstructors.Where(x => x.FirstMidName.ToLower().Contains(this.Filter.ToLower()) || x.LastName.ToLower().Contains(this.Filter.ToLower())).ToList();

            this.Instructors = new ObservableCollection<InstructorItemViewModel>(listInstructors);
        }
    }
}
