using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Pages;
using System.Collections.Generic;
using System.Linq;

namespace MauiApp1.Viewmodels
{
    public partial class StudentListVM : ObservableObject, IQueryAttributable
    {
        private readonly StudentService _studentService;
        private List<StudentDTO> _allStudents = new();  // Lista de todos los alumnos 

        [ObservableProperty]
        public ObservableCollection<StudentDTO> students;   // Lista de alumnos VISIBLES en el page

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        string searchText;

        public StudentListVM(StudentService studentService)
        {
            _studentService = studentService;
            Students = new ObservableCollection<StudentDTO>();
        }

        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Students.Clear();
                foreach (var student in _allStudents)
                {
                    Students.Add(student);
                }
            }
            else
            {
                var filteredStudents = _allStudents
                    .Where(s => (s.Name ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                (s.PaternalSurname ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                (s.MaternalSurname ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Students.Clear();
                foreach (var student in filteredStudents)
                {
                    Students.Add(student);
                }
            }
        }

        [RelayCommand]
        public async Task LoadStudentsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                _allStudents = await _studentService.GetAllStudents();
                PerformSearch(); // Apply current search text or load all if search is empty
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadStudentsAsync();
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
                
                // Remove from both lists to keep UI and master list in sync
                var studentToRemoveFromAll = _allStudents.FirstOrDefault(s => s.IdStudent == student.IdStudent);
                if(studentToRemoveFromAll != null)
                {
                    _allStudents.Remove(studentToRemoveFromAll);
                }
                Students.Remove(student);
            }
        }
    }
}


