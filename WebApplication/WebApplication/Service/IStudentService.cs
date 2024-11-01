using WebApplication.Dto;
using WebApplication.Message.Command;
using WebApplication.Message.Request;

namespace WebApplication.Service;

public interface IStudentService: IService
{
    //post
    Task<StudentDto> AddStudent(AddStudentCommand command);

    //post
    Task UpdateStudent(UpdateStudentCommand command);

    //delete
    Task DeleteStudent(DeleteStudentCommand command);

    //get
    Task<List<StudentDto>> StudentList(GetStudentListRequest request);
    
}