# C# dotnet and Kafka practice

Small Todo API to practice some dotnet and Kafka

Run kafka with `make`, once running create the topic by doing this:

```
sudo docker exec -ti kafka-broker bash
/opt/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic
```

Then run the app with `dotnet run` and visit http://localhost:5074/swagger/index.html or whatever the port is for you.
