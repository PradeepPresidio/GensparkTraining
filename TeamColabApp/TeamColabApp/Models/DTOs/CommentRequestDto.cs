using TeamColabApp.Validations;
public class CommentRequestDto
{
    public string Content { get; set; } = string.Empty;
    [ValidName]
    public string UserName { get; set; } = string.Empty;
    [ValidName]
    public string ProjectTitle { get; set; } = string.Empty;
    public long ProjectTaskId { get; set; }
}
