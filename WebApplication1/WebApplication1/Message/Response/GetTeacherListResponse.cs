using Mediator.Net.Contracts;
using WebApplication1.Dto;

namespace WebApplication1.Message.Response;

public class GetTeacherListResponse: IResponse
{
    public List<TeacherDto> TeacherDtoList { get; set; }
}