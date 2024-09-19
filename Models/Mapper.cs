public static class Mapper
{
    public static TModel Map<TModel, TRequest>(TRequest request, Func<TRequest, TModel> mapFunction)
    where TModel : new()
    where TRequest : IModelRequest
    {
        return mapFunction(request);
    }
}
