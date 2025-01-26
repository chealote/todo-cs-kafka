public enum ActionEnum
{
    Create,
    Update,
    Patch,
    Delete,
}

public class TodoRequest : IModelRequest
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public ActionEnum Action { get; set; }
}
