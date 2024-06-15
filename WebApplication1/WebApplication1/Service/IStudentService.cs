using WebApplication1.Dto;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;

namespace WebApplication1.Service;

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