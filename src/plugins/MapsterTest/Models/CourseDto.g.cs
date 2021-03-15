using System.Collections.Generic;
using MapsterTest.Models;

namespace MapsterTest.Models
{
    public partial class CourseDto
    {
        public int CourseId { get; }
        public string Title { get; }
        public int Credits { get; }
        public ICollection<EnrollmentDto> Enrollments { get; }
        
        public CourseDto(int courseId, string title, int credits, ICollection<EnrollmentDto> enrollments)
        {
            this.CourseId = courseId;
            this.Title = title;
            this.Credits = credits;
            this.Enrollments = enrollments;
        }
    }
}