# C# dotnet and Kafka practice

Small Todo API to practice some dotnet and Kafka

Run kafka with `make`, once running create the topic by doing this:

```
sudo docker exec -ti kafka-broker bash
/opt/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic
```

Then run the app with `dotnet run` and visit http://localhost:5074/swagger/index.html or whatever the port is for you.

## Usage

There are three endpoints, one to get todos, another to create, and another to
subscribe the API to kafka and create those todos in DB.

First, create a todo by making a `POST /` in the swagger, then check the `GET
/` to list todos, should be empty; after you run the request to `GET
/subscribe`, the app starts listening for kafka items and creates a new todo
for each new item that it receives.
