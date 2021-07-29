﻿using Mapster;
using MapsterTest.Domains;
using MapsterTest.DomainsDto;
using System;
using System.Linq.Expressions;

namespace MapsterTest
{
    [Mapper]
    public interface IStudentMapper
    {
        StudentDto2 Map(Student entity);

        StudentDto2 MapTo(Student entity, StudentDto2 dto2);

        Expression<Func<Student, StudentDto2>> ProjectTo { get; }
    }
}