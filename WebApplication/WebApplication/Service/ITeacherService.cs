using WebApplication.Dto;
using WebApplication.Message.Command;
using WebApplication.Message.Request;

namespace WebApplication.Service;

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