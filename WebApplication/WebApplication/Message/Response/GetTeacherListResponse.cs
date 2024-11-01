using Mediator.Net.Contracts;
using WebApplication.Dto;

namespace WebApplication.Message.Response;

public class GetTeacherListResponse: IResponse
{
    public List<TeacherDto> TeacherDtoList { get; set; }
}