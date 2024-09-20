name=kafka-broker

kafka:
	@echo "once is running, enter kafka with:"
	@echo "    docker exec -ti $(name) bash"
	@echo "then create the topic:"
	@echo "    /opt/bin/kafka/kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic"
	sudo docker run --rm -ti --network host --name $(name) apache/kafka:latest
	# now exec -ti and run `./kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic`
	# then use the app
