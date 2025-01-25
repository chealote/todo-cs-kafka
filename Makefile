name=kafka-broker
tag=latest

run:
	@echo "once is running, enter kafka with:"
	@echo "    docker exec -ti $(name) bash"
	@echo "then create the topic:"
	@echo "    /opt/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic"
	sudo docker run --rm -ti \
		-p 9092:9092 --name $(name) $(name):$(tag)
