namespace WebApplication1.Dto;

public class TestDto
{
    public int Id { get; set; }
    public int RPid { get; set; }
    public string Name { get; set; }
    public UserInfo User { get; set; }
    public int Like { get; set; }
    public int View { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
}

public class UserInfo
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}