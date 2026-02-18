using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Pages; // Assuming StudentDetailPage is in Pages
using System.Collections.Generic; // Required for IDictionary
using System.Linq; // Required for FirstOrDefault

namespace MauiApp1.Viewmodels
{
    public partial class StudentListVM : ObservableObject, IQueryAttributable
    {
        private readonly StudentService _studentService;

        [ObservableProperty]
        public ObservableCollection<StudentDTO> students;

        [ObservableProperty]
        public bool isBusy;

        public StudentListVM(StudentService studentService)
        {
            _studentService = studentService;
            Students = new ObservableCollection<StudentDTO>();
            // Since "carga de alumnos" was removed, the collection starts empty.
            // Students will be added/updated manually after CRUD operations.
        }

        [RelayCommand]
        public async Task LoadStudentsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var studentList = await _studentService.GetAllStudents();
                Students.Clear();
                foreach (var student in studentList)
                {
                    Students.Add(student);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Always reload all students when navigating to this page to ensure data freshness.
            await LoadStudentsAsync();
            // Clear the query attributes after processing to prevent repeated actions.
            query.Clear();
        }

        [RelayCommand]
        private async Task AddNewStudent()
        {
            await Shell.Current.GoToAsync(nameof(StudentDetailPage));
        }

        [RelayCommand]
        private async Task EditStudent(StudentDTO student)
        {
            if (student == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(StudentDetailPage)}?{nameof(StudentDTO.IdStudent)}={student.IdStudent}");
        }

        [RelayCommand]
        private async Task DeleteStudent(StudentDTO student)
        {
            if (student == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert("Confirmar Eliminación", $"¿Estás seguro de que quieres eliminar a {student.Name} {student.PaternalSurname}?", "Sí", "No");
            if (confirm)
            {
                await _studentService.Delete(student.IdStudent);
                Students.Remove(student);
            }
        }
    }
}


