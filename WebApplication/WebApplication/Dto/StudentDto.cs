using AutoMapper;
using Mediator.Net.Contracts;
using WebApplication.Common;

namespace WebApplication.Dto;

public class StudentDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
    
    
}