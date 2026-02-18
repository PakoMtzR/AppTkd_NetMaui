using MauiApp1.Data;
using MauiApp1.Models;
using MauiApp1.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class StudentService
    {
        private readonly DatabaseContext _context;

        public StudentService(DatabaseContext context)
        {
            _context = context;
        }

        // --- Mappers ---
        private StudentDTO MapToDTO(Student student)
        {
            if (student == null) return null;

            return new StudentDTO
            {
                IdStudent = student.IdStudent,
                Name = student.Name,
                PaternalSurname = student.PaternalSurname,
                MaternalSurname = student.MaternalSurname,
                Address = student.Address,
                BirthDate = student.BirthDate,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.IsActive,
                Code = student.Code,
                Phone = student.Phone,
                Email = student.Email,
                OccupationId = student.IdStudentOccupation,
                MaritalStatusId = student.IdStudentMaritalStatus,
                BeltId = student.IdStudentBelt,
                BeltColor = student.StudentBelt?.Color, // Null-conditional operator for navigation property
                BeltDescription = student.StudentBelt?.Description,
                MaritalStatusDescription = student.StudentMaritalStatus?.Description,
                OccupationDescription = student.StudentOccupation?.Description
            };
        }

        private Student MapToModel(StudentDTO dto)
        {
            if (dto == null) return null;

            return new Student
            {
                IdStudent = dto.IdStudent,
                Name = dto.Name,
                PaternalSurname = dto.PaternalSurname,
                MaternalSurname = dto.MaternalSurname,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                EnrollmentDate = dto.EnrollmentDate,
                IsActive = dto.IsActive,
                Code = dto.Code,
                Phone = dto.Phone,
                Email = dto.Email,
                IdStudentOccupation = dto.OccupationId,
                IdStudentMaritalStatus = dto.MaritalStatusId,
                IdStudentBelt = dto.BeltId
            };
        }
        // --- End Mappers ---


        public async Task<StudentDTO> GetById(int id)
        {
            var student = await _context.Students
                                        .Include(s => s.StudentBelt)
                                        .Include(s => s.StudentMaritalStatus)
                                        .Include(s => s.StudentOccupation)
                                        .FirstOrDefaultAsync(s => s.IdStudent == id);
            return MapToDTO(student);
        }

        public async Task<StudentDTO> Add(StudentDTO dto)
        {
            var student = MapToModel(dto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            dto.IdStudent = student.IdStudent; // Update DTO with generated ID
            return dto;
        }

        public async Task Update(StudentDTO dto)
        {
            var student = await _context.Students.FindAsync(dto.IdStudent);
            if (student != null)
            {
                // Update properties from DTO to model
                student.Name = dto.Name;
                student.PaternalSurname = dto.PaternalSurname;
                student.MaternalSurname = dto.MaternalSurname;
                student.Address = dto.Address;
                student.BirthDate = dto.BirthDate;
                student.EnrollmentDate = dto.EnrollmentDate;
                student.IsActive = dto.IsActive;
                student.Code = dto.Code;
                student.Phone = dto.Phone;
                student.Email = dto.Email;
                student.IdStudentOccupation = dto.OccupationId;
                student.IdStudentMaritalStatus = dto.MaritalStatusId;
                student.IdStudentBelt = dto.BeltId;

                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<StudentOccupation>> GetAllOccupations()
        {
            return await _context.StudentOccupations.ToListAsync();
        }

        public async Task<List<StudentMaritalStatus>> GetAllMaritalStatuses()
        {
            return await _context.StudentMaritalStatuses.ToListAsync();
        }

        public async Task<List<StudentBelt>> GetAllBelts()
        {
            return await _context.StudentBelts.ToListAsync();
        }

        public async Task<List<StudentDTO>> GetAllStudents()
        {
            var students = await _context.Students
                                         .Include(s => s.StudentBelt)
                                         .Include(s => s.StudentMaritalStatus)
                                         .Include(s => s.StudentOccupation)
                                         .ToListAsync();
            return students.Select(s => MapToDTO(s)).ToList();
        }
    }
}
