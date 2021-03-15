using MapsterTest.Domains;
using MapsterTest.Models;

namespace MapsterTest.Models
{
    public partial class EnrollmentDto
    {
        public int EnrollmentId { get; }
        public int CourseId { get; }
        public int StudentId { get; }
        public Grade? Grade { get; }
        public CourseDto Course { get; }
        public StudentDto Student { get; }
        
        public EnrollmentDto(int enrollmentId, int courseId, int studentId, Grade? grade, CourseDto course, StudentDto student)
        {
            this.EnrollmentId = enrollmentId;
            this.CourseId = courseId;
            this.StudentId = studentId;
            this.Grade = grade;
            this.Course = course;
            this.Student = student;
        }
    }
}