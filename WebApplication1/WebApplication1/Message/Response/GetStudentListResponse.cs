using Mediator.Net.Contracts;
using WebApplication1.Dto;

namespace WebApplication1.Message.Response;

public class GetStudentListResponse: IResponse
{
    public List<StudentDto> StudentDtoList { get; set; }
}