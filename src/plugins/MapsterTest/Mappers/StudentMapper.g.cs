using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapsterTest;
using MapsterTest.Domains;
using MapsterTest.DomainsDto;

namespace MapsterTest.Mappers
{
    public partial class StudentMapper : IStudentMapper
    {
        public Expression<Func<Student, StudentDto2>> ProjectTo => p1 => new StudentDto2(p1.Id, p1.LastName, p1.FirstMidName, p1.EnrollmentDate, p1.Enrollments.Select<Enrollment, EnrollmentDto2>(p2 => new EnrollmentDto2(p2.EnrollmentId, p2.CourseId, p2.StudentId, p2.Grade, new CourseDto2(p2.Course.CourseId, p2.Course.Title, p2.Course.Credits, null), new StudentDto2(p2.Student.Id, p2.Student.LastName, p2.Student.FirstMidName, p2.Student.EnrollmentDate, null))).ToList<EnrollmentDto2>());
        public StudentDto2 Map(Student p3)
        {
            return p3 == null ? null : new StudentDto2(p3.Id, p3.LastName, p3.FirstMidName, p3.EnrollmentDate, funcMain1(p3.Enrollments));
        }
        public StudentDto2 MapTo(Student p5, StudentDto2 p6)
        {
            return p5 == null ? null : new StudentDto2(p5.Id, p5.LastName, p5.FirstMidName, p5.EnrollmentDate, funcMain2(p5.Enrollments));
        }
        
        private ICollection<EnrollmentDto2> funcMain1(ICollection<Enrollment> p4)
        {
            if (p4 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto2> result = new List<EnrollmentDto2>(p4.Count);
            
            IEnumerator<Enrollment> enumerator = p4.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(item == null ? null : new EnrollmentDto2(item.EnrollmentId, item.CourseId, item.StudentId, item.Grade, item.Course == null ? null : new CourseDto2(item.Course.CourseId, item.Course.Title, item.Course.Credits, null), item.Student == null ? null : new StudentDto2(item.Student.Id, item.Student.LastName, item.Student.FirstMidName, item.Student.EnrollmentDate, null)));
            }
            return result;
            
        }
        
        private ICollection<EnrollmentDto2> funcMain2(ICollection<Enrollment> p7)
        {
            if (p7 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto2> result = new List<EnrollmentDto2>(p7.Count);
            
            IEnumerator<Enrollment> enumerator = p7.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(item == null ? null : new EnrollmentDto2(item.EnrollmentId, item.CourseId, item.StudentId, item.Grade, item.Course == null ? null : new CourseDto2(item.Course.CourseId, item.Course.Title, item.Course.Credits, null), item.Student == null ? null : new StudentDto2(item.Student.Id, item.Student.LastName, item.Student.FirstMidName, item.Student.EnrollmentDate, null)));
            }
            return result;
            
        }
    }
}