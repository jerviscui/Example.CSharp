using System;
using System.Collections.Generic;

namespace MapsterTest.Domains
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }
        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }

    public class Student
    {
        public int? Id { get; set; }
        public string? LastName { get; set; } = string.Empty;
        public string FirstMidName { get; set; } = string.Empty;
        public DateTime? EnrollmentDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
