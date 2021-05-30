package com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.function.BiFunction;
import java.util.function.Function;

import javax.naming.Binding;

import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.FanoutExchange;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitAdmin;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.core.annotation.Order;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

public abstract class MqClientAbstract {

	
	//public Function<EventClientDto, Object> eventHandler;
	
	@Order(1)
	@Bean
    public RabbitAdmin rabbitAdmin(ConnectionFactory connectionFactory) {
        return new RabbitAdmin(connectionFactory);
    }

	@Order(1)
	@Autowired
	public RabbitAdmin _RabbitAdmin;
	
	@Order(1)
	@Autowired
	public MqServiceNameClientBean serviceName;
	
	@Order(1)
	@Autowired
	public MqClientEventsToSubscribe mqClientEventsToSubscribe;

	@Order(1)
	@Autowired
	public MqClientQueueName mqClientQueueName;

	@Order(1)
	@Autowired
	private RabbitTemplate template;

	
	/*
	@Order(1)
	@Bean
	public Queue getEventQueue() {
		// return new AnonymousQueue(); this on scale will make all instances get the
		// event
		return new Queue(serviceName.name + ".mqevents");// this on scale will make only one of instances to get the
															// event
	}
	*/
	
	
	
	
	@Order(2)
	@Bean
	public void bindQueues() {
		var queue=new Queue(serviceName.name + ".mqevents");
		_RabbitAdmin.declareQueue(queue);
		
		for(var qu : mqClientEventsToSubscribe.EventsToSubscribe) 
		{
			FanoutExchange fanout=new FanoutExchange(qu+".MqFanoutExchange");
			_RabbitAdmin.declareExchange(fanout);
			_RabbitAdmin.declareBinding(BindingBuilder.bind(queue).to(fanout));
		}
	}
	
	
	
	/*
	@Order(2)
	@Bean
	public void declareQueues() {
		var list=Arrays.asList(mqClientQueueName.buildFor(""));
		for(var qu : list) 
		{
			_RabbitAdmin.declareQueue( new Queue(qu));
		}
	}
	*/
	@Order(1)
	@Bean
	public void test() {
	//	System.out.println("Here");
	//	System.out.println(mqClientQueueName.buildFor(""));
	}
	@Order(5)
	@RabbitListener(queues = "#{mqClientQueueName.buildFor(\"\")}")
	public void mqReceive(String eventJson) throws InterruptedException {
System.out.println("Event Recieved: " +eventJson);
		var eventDto=new EventClientDto(eventJson);
		//eventHandler.apply(eventDto);
		handleEvent(eventDto);
		
	}
	
	public Object handleEvent(EventClientDto event) {
		
		
		return event;
		
	}
	

	public static class EventClientDto {

		public String eventName;
		public List<Object> params;
		public Class<?>[] paramsTypes;

		public EventClientDto(String _eventName, List<Object> _params, Class<?>[] _paramsTypes) {
			eventName = _eventName;
			params = _params;
			paramsTypes = _paramsTypes;
		}

		public EventClientDto() {
		}

		public EventClientDto(String json) {

			ObjectMapper objectMapper = new ObjectMapper();
			try {
				EventClientDto rpcDto = objectMapper.readValue(json, EventClientDto.class);
				eventName = rpcDto.eventName;
				params = rpcDto.params;
				paramsTypes = rpcDto.paramsTypes;

			} catch (JsonProcessingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

		public String getEventName() {
			return eventName;
		}

		public void setEventName(String eventName) {
			this.eventName = eventName;
		}

		public Class<?>[] getParamsTypes() {
			return paramsTypes;
		}

		public void setParamsTypes(Class<?>[] paramstypes) {
			this.paramsTypes = paramstypes;
		}

		public List<Object> getParams() {
			return params;
		}

		public void setParams(List<Object> params) {
			this.params = params;
		}

		public String GetJson() {

			ObjectMapper objectMapper = new ObjectMapper();
			try {
				return objectMapper.writeValueAsString(this);
			} catch (JsonProcessingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			return null;
		}

	}
}
