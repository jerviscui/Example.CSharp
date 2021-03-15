using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MapsterTest.Domains;
using MapsterTest.Mappers;
using MapsterTest.Models;

namespace MapsterTest
{
    class Program
    {
        private static Student? _student;

        static void Main(string[] args)
        {
            static void Setup()
            {
                _student = new Student()
                {
                    Id = 1,
                    FirstMidName = "A",
                    LastName = null,
                    EnrollmentDate = DateTime.Now
                };

                var course1 = new Course()
                {
                    CourseId = 2,
                    Credits = 10,
                    Title = "Title1"
                };

                var course2 = new Course()
                {
                    CourseId = 3,
                    Credits = 11,
                    Title = "Title2"
                };

                var enrollments = new List<Enrollment>()
                {
                    new Enrollment()
                    {
                        EnrollmentId = 4,
                        Course = course1,
                        CourseId = course1.CourseId,
                        Student = _student,
                        StudentId = _student.Id.Value,
                        Grade = Grade.A
                    },
                    new Enrollment()
                    {
                        EnrollmentId = 5,
                        Course = course2,
                        CourseId = course2.CourseId,
                        Student = _student,
                        StudentId = _student.Id.Value,
                        Grade = Grade.A
                    }
                };

                _student.Enrollments.Add(enrollments[0]);
                _student.Enrollments.Add(enrollments[1]);

                course1.Enrollments.Add(enrollments[0]);
                course2.Enrollments.Add(enrollments[1]);
            };

            Setup();

            Console.WriteLine(JsonSerializer.Serialize(_student,
                new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve }));

            Console.WriteLine();

            FluentApiTest();

            Console.WriteLine();

            InterfaceTest();
        }

        static void FluentApiTest()
        {
            var dto = _student.AdaptToDto();

            Console.WriteLine(JsonSerializer.Serialize(dto));
        }

        static void InterfaceTest()
        {
            var mapper = new Mappers.StudentMapper();

            var dto2 = mapper.Map(_student);

            Console.WriteLine(JsonSerializer.Serialize(dto2));
        }
    }
}
