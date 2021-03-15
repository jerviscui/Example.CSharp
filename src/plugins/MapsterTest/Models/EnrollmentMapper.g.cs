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
    public static partial class EnrollmentMapper
    {
        public static EnrollmentDto AdaptToDto(this Enrollment p1)
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
                ReferenceTuple key = new ReferenceTuple(p1, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p1.EnrollmentId, p1.CourseId, p1.StudentId, p1.Grade, funcMain1(p1.Course), funcMain4(p1.Student));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        public static EnrollmentDto AdaptTo(this Enrollment p8, EnrollmentDto p9)
        {
            if (p8 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p8, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p8.EnrollmentId, p8.CourseId, p8.StudentId, p8.Grade, funcMain7(p8.Course), funcMain10(p8.Student));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        public static Expression<Func<Enrollment, EnrollmentDto>> ProjectToDto => p16 => new EnrollmentDto(p16.EnrollmentId, p16.CourseId, p16.StudentId, p16.Grade, new CourseDto(p16.Course.CourseId, p16.Course.Title, p16.Course.Credits, p16.Course.Enrollments.Select<Enrollment, EnrollmentDto>(p17 => new EnrollmentDto(p17.EnrollmentId, p17.CourseId, p17.StudentId, p17.Grade, null, null)).ToList<EnrollmentDto>()), new StudentDto(p16.Student.Id, p16.Student.LastName, p16.Student.FirstMidName, p16.Student.EnrollmentDate, p16.Student.Enrollments.Select<Enrollment, EnrollmentDto>(p18 => new EnrollmentDto(p18.EnrollmentId, p18.CourseId, p18.StudentId, p18.Grade, null, null)).ToList<EnrollmentDto>()));
        
        private static CourseDto funcMain1(Course p2)
        {
            if (p2 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p2, typeof(CourseDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (CourseDto)cache;
                }
                CourseDto result = new CourseDto(p2.CourseId, p2.Title, p2.Credits, funcMain2(p2.Enrollments));
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
                StudentDto result = new StudentDto(p5.Id, p5.LastName, p5.FirstMidName, p5.EnrollmentDate, funcMain5(p5.Enrollments));
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
                CourseDto result = new CourseDto(p10.CourseId, p10.Title, p10.Credits, funcMain8(p10.Enrollments));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static StudentDto funcMain10(Student p13)
        {
            if (p13 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p13, typeof(StudentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (StudentDto)cache;
                }
                StudentDto result = new StudentDto(p13.Id, p13.LastName, p13.FirstMidName, p13.EnrollmentDate, funcMain11(p13.Enrollments));
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static ICollection<EnrollmentDto> funcMain2(ICollection<Enrollment> p3)
        {
            if (p3 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p3.Count);
            
            IEnumerator<Enrollment> enumerator = p3.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain3(item));
            }
            return result;
            
        }
        
        private static ICollection<EnrollmentDto> funcMain5(ICollection<Enrollment> p6)
        {
            if (p6 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p6.Count);
            
            IEnumerator<Enrollment> enumerator = p6.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain6(item));
            }
            return result;
            
        }
        
        private static ICollection<EnrollmentDto> funcMain8(ICollection<Enrollment> p11)
        {
            if (p11 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p11.Count);
            
            IEnumerator<Enrollment> enumerator = p11.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain9(item));
            }
            return result;
            
        }
        
        private static ICollection<EnrollmentDto> funcMain11(ICollection<Enrollment> p14)
        {
            if (p14 == null)
            {
                return null;
            }
            ICollection<EnrollmentDto> result = new List<EnrollmentDto>(p14.Count);
            
            IEnumerator<Enrollment> enumerator = p14.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                Enrollment item = enumerator.Current;
                result.Add(funcMain12(item));
            }
            return result;
            
        }
        
        private static EnrollmentDto funcMain3(Enrollment p4)
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
                ReferenceTuple key = new ReferenceTuple(p4, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p4.EnrollmentId, p4.CourseId, p4.StudentId, p4.Grade, null, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static EnrollmentDto funcMain6(Enrollment p7)
        {
            if (p7 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p7, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p7.EnrollmentId, p7.CourseId, p7.StudentId, p7.Grade, null, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static EnrollmentDto funcMain9(Enrollment p12)
        {
            if (p12 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p12, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p12.EnrollmentId, p12.CourseId, p12.StudentId, p12.Grade, null, null);
                references[key] = (object)result;
                return result;
            }
            finally
            {
                scope.Dispose();
            }
            
        }
        
        private static EnrollmentDto funcMain12(Enrollment p15)
        {
            if (p15 == null)
            {
                return null;
            }
            MapContextScope scope = new MapContextScope();
            
            try
            {
                object cache;
                
                Dictionary<ReferenceTuple, object> references = scope.Context.References;
                ReferenceTuple key = new ReferenceTuple(p15, typeof(EnrollmentDto));
                
                if (references.TryGetValue(key, out cache))
                {
                    return (EnrollmentDto)cache;
                }
                EnrollmentDto result = new EnrollmentDto(p15.EnrollmentId, p15.CourseId, p15.StudentId, p15.Grade, null, null);
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