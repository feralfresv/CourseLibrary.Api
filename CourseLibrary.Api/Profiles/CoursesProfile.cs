﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.Api.Profiles
{
    public class CoursesProfile : Profile
    {

        public CoursesProfile()
        {
            CreateMap<API.Entities.Course, Models.CourseDto>();

        }
    }
}