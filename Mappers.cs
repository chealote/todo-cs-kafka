using TodoApi.Models;

public static class Mappers
{
    public static Todo TodoMapper(TodoRequest request)
    {
        return new Todo { Name = request.Name };
    }
}
