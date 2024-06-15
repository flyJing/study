using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ContextConfiguration;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;

namespace WebApplication1.Service.Impl;

public class TeacherService : ITeacherService
{
    private readonly SchoolContext _schoolContext;
    private readonly IMapper _mapper;

    public TeacherService(SchoolContext schoolContext, IMapper mapper)
    {
        _schoolContext = schoolContext;
        _mapper = mapper;
    }
    
    public async Task<TeacherDto> AddTeacher(AddTeacherCommand command, CancellationToken cancellationToken)
    {
        var teacher = _mapper.Map<Teacher>(command);
        await _schoolContext.AddAsync(teacher, cancellationToken).ConfigureAwait(false);
        await _schoolContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return _mapper.Map<TeacherDto>(command);
    }

    public async Task UpdateTeacher(UpdateTeacherCommand command, CancellationToken cancellationToken)
    {
        var teacher = await _schoolContext.Set<Teacher>().FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken).ConfigureAwait(false);
        if (teacher == null)
        {
            throw new ArgumentException("teacher为空");
        }
        _mapper.Map(command,teacher);
        await _schoolContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteTeacher(DeleteTeacherCommand command, CancellationToken cancellationToken)
    {
        var dbTeacher = await _schoolContext.Set<Teacher>().SingleOrDefaultAsync(t => t.Id == command.Id, cancellationToken)
            .ConfigureAwait(false);
        if (dbTeacher == null)
        {
            throw new ArgumentException("teacher为空");
        }
        _schoolContext.Remove(dbTeacher);
        await _schoolContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<TeacherDto>> TeacherList(GetTeacherListRequest request, CancellationToken cancellationToken)
    {
        if (request.Name == null)
        {
            return await _schoolContext.Set<Teacher>().Select(teacher => _mapper.Map<TeacherDto>(teacher))
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        return await _schoolContext.Set<Teacher>().Where(t => t.Name.Contains(request.Name))
            .Select(teacher => _mapper.Map<TeacherDto>(teacher)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}