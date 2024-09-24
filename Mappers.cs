using TodoApi.Models;

public static class Mappers
{
    public static Todo TodoMapper(TodoRequest request)
    {
        // cast (int) of null maybe already maps to 0
        if (request.Id == null)
            request.Id = 0;
        return new Todo {
            Id = (int) request.Id,
            Name = request.Name,
            IsComplete = request.IsComplete,
        };
    }
}
