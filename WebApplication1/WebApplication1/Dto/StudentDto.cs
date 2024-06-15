using AutoMapper;
using Mediator.Net.Contracts;
using WebApplication1.Common;

namespace WebApplication1.Dto;

public class StudentDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
    
    
}