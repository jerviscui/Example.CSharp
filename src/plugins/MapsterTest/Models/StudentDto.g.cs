using System;
using System.Collections.Generic;
using MapsterTest.Models;

namespace MapsterTest.Models
{
    public partial class StudentDto
    {
        public int? Id { get; }
        public string LastName { get; }
        public string FirstMidName { get; }
        public DateTime? EnrollmentDate { get; }
        public ICollection<EnrollmentDto> Enrollments { get; }
        
        public StudentDto(int? id, string lastName, string firstMidName, DateTime? enrollmentDate, ICollection<EnrollmentDto> enrollments)
        {
            this.Id = id;
            this.LastName = lastName;
            this.FirstMidName = firstMidName;
            this.EnrollmentDate = enrollmentDate;
            this.Enrollments = enrollments;
        }
    }
}