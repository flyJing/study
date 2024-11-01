using System.Collections;
using Mediator.Net;
using WebApplication1.Common;
using WebApplication1.Dto;
using WebApplication1.Message;
using WebApplication1.Message.Request;
using WebApplication1.Message.Response;
using Xunit;

namespace XUnitTest;
using Shouldly;
public class XUnitTest
{
    private readonly IMediator _mediator;
    
    public XUnitTest(IMediator mediator)
    {
        
        _mediator = mediator;
    }
    
    [Fact]
    // [ClassData(typeof(ShouldValidatorStudentTestData))]
    public async void ShouldValidatorStudent()
    {
        var request = new GetStudentListRequest();
        var result = await _mediator.RequestAsync<GetStudentListRequest,
            GetStudentListResponse>(request);

        Console.WriteLine(result);
        // var isValid = studentValidator.Validate(dto).IsValid;
       
    }
    public class ShouldValidatorStudentTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                true, new StudentDto()
                {
                    Id = Guid.NewGuid(),
                    Age = 10,
                    Name = "snake"
                }
            };
            yield return new object[]
            {
                false, new StudentDto()
                {
                    Id = Guid.NewGuid(),
                    Age = 0,
                    Name = "snake"
                }
            };
            yield return new object[]
            {
                false, new StudentDto()
                {
                    Id = Guid.NewGuid(),
                    Age = 50,
                    Name = null
                }
            };
            yield return new object[]
            {
                false, new StudentDto()
                {
                    Id = Guid.NewGuid(),
                    Age = 300,
                    Name = "snake"
                }
            };
            yield return new object[]
            {
                true, new StudentDto()
                {
                    Id = Guid.NewGuid(),
                    Age = 10,
                    Name = "snake"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}