using TodoApi.Models;

namespace TodoApi.Services.Interfaces;

public interface IKafkaService
{
    public void Subscribe(Action<string> Process);
    public Task Produce(string message);
}
