package com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstractsCp;

import java.util.List;

import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

@Service
public class MqEventSender {
	
	@Autowired
	@Order(1)
	MqServiceNameServerBean serviceName;
	
	@Autowired
    private RabbitTemplate template;
	
	public void send(EventSenderDto eventDto) {
		String event=eventDto.GetJson();
        template.convertAndSend(serviceName.name + ".MqFanoutExchange", "", event);
        System.out.println(" [x] Sent '" + event + "'");
    }

	
	public static class EventSenderDto {

		public String eventName;
		public List<Object> params;
		public Class<?>[] paramsTypes;

		public EventSenderDto(String _eventName, List<Object> _params, Class<?>[] _paramsTypes) {
			eventName = _eventName;
			params = _params;
			paramsTypes = _paramsTypes;
		}

		public EventSenderDto() {
		}

		public EventSenderDto(String json) {

			ObjectMapper objectMapper = new ObjectMapper();
			try {
				EventSenderDto rpcDto = objectMapper.readValue(json, EventSenderDto.class);
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
