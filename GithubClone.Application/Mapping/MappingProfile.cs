using AutoMapper;
using GithubClone.Application.DTOs;
using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();

            CreateMap<CreateRepositoryDto, Repositories>().ReverseMap();
            CreateMap<Repositories,RepositoryDto>().ReverseMap();

            CreateMap<Commit, CommitDto>().ReverseMap();

            CreateMap<Branch, BranchDto>().ReverseMap();

        }
    }
}
