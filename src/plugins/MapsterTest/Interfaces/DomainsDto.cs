using System;
using System.Collections.Generic;
using MapsterTest.Domains;

namespace MapsterTest.DomainsDto
{
    public partial class StudentDto2
    {
        public int? Id { get; }
        public string LastName { get; }
        public string? FirstMidName { get; }
        public DateTime? EnrollmentDate { get; }
        public ICollection<EnrollmentDto2> Enrollments { get; }
        
        public StudentDto2(int? id, string lastName, string firstMidName, DateTime? enrollmentDate, ICollection<EnrollmentDto2> enrollments)
        {
            this.Id = id;
            this.LastName = lastName;
            this.FirstMidName = firstMidName;
            this.EnrollmentDate = enrollmentDate;
            this.Enrollments = enrollments;
        }
    }

    public partial class EnrollmentDto2
    {
        public int EnrollmentId { get; }
        public int CourseId { get; }
        public int StudentId { get; }
        public Grade? Grade { get; }
        public CourseDto2 Course { get; }
        public StudentDto2 Student { get; }
        
        public EnrollmentDto2(int enrollmentId, int courseId, int studentId, Grade? grade, CourseDto2 course, StudentDto2 student)
        {
            this.EnrollmentId = enrollmentId;
            this.CourseId = courseId;
            this.StudentId = studentId;
            this.Grade = grade;
            this.Course = course;
            this.Student = student;
        }
    }

    public partial class CourseDto2
    {
        public int CourseId { get; }
        public string Title { get; }
        public int Credits { get; }
        public ICollection<EnrollmentDto2> Enrollments { get; }
        
        public CourseDto2(int courseId, string title, int credits, ICollection<EnrollmentDto2> enrollments)
        {
            this.CourseId = courseId;
            this.Title = title;
            this.Credits = credits;
            this.Enrollments = enrollments;
        }
    }
}