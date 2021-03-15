using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mapster;
using Mapster.Utils;
using MapsterTest.Domains;
using MapsterTest.Models;

namespace MapsterTest.Models
{
    public static partial class StudentMapper
    {
        public static StudentDto AdaptToDto(this Student p1)
        {
            if (p1 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p1, typeof(StudentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (StudentDto)cache;
                }
                StudentDto result = new StudentDto(p1.Id, p1.LastName, p1.FirstMidName, p1.EnrollmentDate, funcMain1(p1.Enrollments));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        public static StudentDto AdaptTo(this Student p6, StudentDto p7)
        {
            if (p6 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p6, typeof(StudentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (StudentDto)cache;
                }
                StudentDto result = new StudentDto(p6.Id, p6.LastName, p6.FirstMidName, p6.EnrollmentDate, funcMain5(p6.Enrollments));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        public static Expression<Func<Student, StudentDto>> ProjectToDto => p12 => new StudentDto(p12.Id, p12.LastName, p12.FirstMidName, p12.EnrollmentDate, p12.Enrollments.Select<Enrollment, EnrollmentDto>(p13 => new EnrollmentDto(p13.EnrollmentId, p13.CourseId, p13.StudentId, p13.Grade, new CourseDto(p13.Course.CourseId, p13.Course.Title, p13.Course.Credits, null), new StudentDto(p13.Student.Id, p13.Student.LastName, p13.Student.FirstMidName, p13.Student.EnrollmentDate, null))).ToList<EnrollmentDto>());
        
        private static ICollection<EnrollmentDto> funcMain1(ICollection<Enrollment> p2)
        {
            if (p2 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p2.Count);
            
            IEnumerator<Enrollment> enumerator = p2.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain2(item));
            }
            return result;
            
        }
        
        private static ICollection<EnrollmentDto> funcMain5(ICollection<Enrollment> p8)
        {
            if (p8 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p8.Count);
            
            IEnumerator<Enrollment> enumerator = p8.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain6(item));
            }
            return result;
            
        }
        
        private static EnrollmentDto funcMain2(Enrollment p3)
        {
            if (p3 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p3, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p3.EnrollmentId, p3.CourseId, p3.StudentId, p3.Grade, funcMain3(p3.Course), funcMain4(p3.Student));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static EnrollmentDto funcMain6(Enrollment p9)
        {
            if (p9 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p9, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p9.EnrollmentId, p9.CourseId, p9.StudentId, p9.Grade, funcMain7(p9.Course), funcMain8(p9.Student));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static CourseDto funcMain3(Course p4)
        {
            if (p4 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p4, typeof(CourseDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (CourseDto)cache;
                }
                CourseDto result = new CourseDto(p4.CourseId, p4.Title, p4.Credits, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static StudentDto funcMain4(Student p5)
        {
            if (p5 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p5, typeof(StudentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (StudentDto)cache;
                }
                StudentDto result = new StudentDto(p5.Id, p5.LastName, p5.FirstMidName, p5.EnrollmentDate, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static CourseDto funcMain7(Course p10)
        {
            if (p10 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p10, typeof(CourseDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (CourseDto)cache;
                }
                CourseDto result = new CourseDto(p10.CourseId, p10.Title, p10.Credits, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static StudentDto funcMain8(Student p11)
        {
            if (p11 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p11, typeof(StudentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (StudentDto)cache;
                }
                StudentDto result = new StudentDto(p11.Id, p11.LastName, p11.FirstMidName, p11.EnrollmentDate, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
    }
}