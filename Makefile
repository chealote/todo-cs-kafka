kafka:
	sudo docker run --rm -ti --network host --name kafka-broker apache/kafka:latest
	# now exec -ti and run `./kafka-topics.sh --bootstrap-server localhost:9092 --create --topic kafka-topic`
	# then use the app
