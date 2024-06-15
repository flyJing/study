using WebApplication1.Dto;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;

namespace WebApplication1.Service;

public interface ITeacherService: IService
{
    //post
    Task<TeacherDto> AddTeacher(AddTeacherCommand command, CancellationToken cancellationToken);
    //
    //post
    Task UpdateTeacher(UpdateTeacherCommand command, CancellationToken cancellationToken);
    //
    //delete
    Task DeleteTeacher(DeleteTeacherCommand command, CancellationToken cancellationToken);

    //get
    Task<List<TeacherDto>> TeacherList(GetTeacherListRequest request, CancellationToken cancellationToken);
}