namespace AuroraIgloosAPI.DTOs;

public class ContentBlockDTO
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public DateOnly LastModifiedAt { get; set; }
}

public class ContentBlockCreateDTO
{
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public DateOnly LastModifiedAt { get; set; }
    
}

public class ContentBlockUpdateDTO
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    
    public DateOnly LastModifiedAt { get; set; }
}