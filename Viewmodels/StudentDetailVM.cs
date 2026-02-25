using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Models; // For StudentOccupation, StudentMaritalStatus, StudentBelt
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq; // For .FirstOrDefault()

namespace MauiApp1.Viewmodels
{
    [QueryProperty(nameof(StudentId), "IdStudent")]
    public partial class StudentDetailVM : ObservableObject, IQueryAttributable
    {
        private readonly StudentService _studentService;

        [ObservableProperty]
        public StudentDTO student;

        [ObservableProperty]
        public int studentId;

        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        public List<StudentOccupation> occupations;

        [ObservableProperty]
        public List<StudentMaritalStatus> maritalStatuses;

        [ObservableProperty]
        public List<StudentBelt> belts;

        [ObservableProperty]
        public StudentOccupation selectedOccupation;

        [ObservableProperty]
        public StudentMaritalStatus selectedMaritalStatus;

        [ObservableProperty]
        public StudentBelt selectedBelt;

        public StudentDetailVM(StudentService studentService)
        {
            _studentService = studentService;
            Student = new StudentDTO { BirthDate = DateTime.Now, EnrollmentDate = DateTime.Now, IsActive = true };
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Reset StudentId before processing new query
            StudentId = 0; 
            
            if (query.TryGetValue("IdStudent", out object studentIdParam))
            {
                if (studentIdParam is string studentIdString && int.TryParse(studentIdString, out int id))
                {
                    StudentId = id;
                }
                else if (studentIdParam is int studentIdInt)
                {
                    StudentId = studentIdInt;
                }
            }

            // Let the command handle the actual loading logic
            await LoadStudentDetails();
        }

        private async Task LoadPickerData()
        {
            Occupations = await _studentService.GetAllOccupations();
            MaritalStatuses = await _studentService.GetAllMaritalStatuses();
            Belts = await _studentService.GetAllBelts();
        }

        private async Task LoadStudentDetails()
        {
            IsLoading = true;
            try
            {
                await LoadPickerData(); // Load reference data first

                if (StudentId > 0) // Existing student
                {
                    Title = "Editar Estudiante";
                    Debug.WriteLine($"Attempting to load student with ID from LoadStudentDetailsCommand: {StudentId}");
                    var loadedStudent = await _studentService.GetById(StudentId);

                    if (loadedStudent == null)
                    {
                        Debug.WriteLine($"Student with ID {StudentId} not found. Initializing new student as fallback.");
                        Student = new StudentDTO { BirthDate = DateTime.Now, EnrollmentDate = DateTime.Now, IsActive = true };
                        Title = "Nuevo Estudiante"; // Revert to new if not found
                    }
                    else
                    {
                        Student = loadedStudent;
                        // Set selected picker items based on loaded student's FK IDs
                        SelectedOccupation = Occupations?.FirstOrDefault(o => o.IdStudentOccupation == Student.OccupationId);
                        SelectedMaritalStatus = MaritalStatuses?.FirstOrDefault(m => m.IdStudentMaritalStatus == Student.MaritalStatusId);
                        SelectedBelt = Belts?.FirstOrDefault(b => b.IdStudentBelt == Student.BeltId);
                    }
                }
                else // New student
                {
                    Title = "Nuevo Estudiante";
                    Debug.WriteLine("No valid StudentId, initializing new student in LoadStudentDetailsCommand.");
                    Student = new StudentDTO { BirthDate = DateTime.Now, EnrollmentDate = DateTime.Now, IsActive = true };
                    // Ensure pickers are cleared for a new student entry
                    SelectedOccupation = null;
                    SelectedMaritalStatus = null;
                    SelectedBelt = null;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSelectedOccupationChanged(StudentOccupation value)
        {
            if (Student != null && value != null)
            {
                Student.OccupationId = value.IdStudentOccupation;
            }
        }

        partial void OnSelectedMaritalStatusChanged(StudentMaritalStatus value)
        {
            if (Student != null && value != null)
            {
                Student.MaritalStatusId = value.IdStudentMaritalStatus;
            }
        }

        partial void OnSelectedBeltChanged(StudentBelt value)
        {
            if (Student != null && value != null)
            {
                Student.BeltId = value.IdStudentBelt;
            }
        }

        [RelayCommand]
        private async Task SaveStudent()
        {
            if (Student == null)
                return;

            // FKs are already updated by OnSelected...Changed methods
            // Now ensure they are set if not selected by the user (e.g., default value)
            if (SelectedOccupation != null) Student.OccupationId = SelectedOccupation.IdStudentOccupation;
            if (SelectedMaritalStatus != null) Student.MaritalStatusId = SelectedMaritalStatus.IdStudentMaritalStatus;
            if (SelectedBelt != null) Student.BeltId = SelectedBelt.IdStudentBelt;


            StudentDTO savedStudent;
            if (Student.IdStudent == 0)
            {
                // New student
                savedStudent = await _studentService.Add(Student); // _studentService.Add already returns updated DTO with Id
            }
            else
            {
                // Existing student
                await _studentService.Update(Student);
                savedStudent = Student; // For updates, the passed DTO is already updated
            }

            // Pass the saved student back to the previous page
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                { "SavedStudent", savedStudent }
            });
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
