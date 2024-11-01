using Mediator.Net.Contracts;
using WebApplication.Dto;

namespace WebApplication.Message.Response;

public class GetStudentListResponse: IResponse
{
    public List<StudentDto> StudentDtoList { get; set; }
}