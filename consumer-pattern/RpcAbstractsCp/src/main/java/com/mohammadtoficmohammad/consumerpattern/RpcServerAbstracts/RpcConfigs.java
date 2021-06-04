package com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts;

import java.util.Arrays;
import java.util.List;

import org.springframework.amqp.core.Binding;
import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.DirectExchange;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

@Component
public class RpcConfigs {

	@Autowired
	@Order(1)
	ServiceNameServerBean serviceName;

	/*
	 * Or like this
	 * 
	 * @Value("${spring.application.name}") private String serviceName;
	 */

	@Autowired
	@Order(2)
	QueueName queueName;

	public String RoutingKey = "RpcRoutingKey";

	@Order(3)
	@Autowired
	IRpcHandler rpcHandler;

	@Order(3)
	@Bean
	public Queue queue() {
		return new Queue(serviceName.name + ".rpc.requests");
	}

	@Order(3)
	@Bean
	public DirectExchange exchange() {
		return new DirectExchange(serviceName.name + ".rpc.directexchange");
	}

	@Order(3)
	@Bean
	public Binding binding(DirectExchange exchange, Queue queue) {
		return BindingBuilder.bind(queue).to(exchange).with(RoutingKey);
	}

	@Order(3)
	@RabbitListener(queues = "#{queueName.buildFor(\"\")}")
	// @RabbitListener(queues = "businessManager.rpc.requests")//just testing
	public Object RpcRouter(String rpcJson) {
		// System.out.println( "rpcjson recev is "+rpcJson);
		RpcDto rpcDto = new RpcDto(rpcJson);
		Object response = null;
		Method method = null;
		try {
			Method[] methods = rpcHandler.getClass().getDeclaredMethods();
			// method = rpcHandler.getClass().getMethod(rpcDto.methodName, List.class );

			for (var m : methods) {
				if (m.getName().equals(rpcDto.methodName)) {
					method = m;
					break;
				}
				;

			}

			response = method.invoke(rpcHandler, rpcDto.params.toArray()); // pass args
		} catch (SecurityException | IllegalAccessException | IllegalArgumentException | InvocationTargetException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return response;
	}

	public static class RpcDto {

		public String methodName;
		public List<Object> params;
		public Class<?>[] paramsTypes;

		public RpcDto(String _methodName, List<Object> _params, Class<?>[] _paramsTypes) {
			methodName = _methodName;
			params = _params;
			paramsTypes = _paramsTypes;
		}

		public RpcDto() {
		}

		public RpcDto(String json) {

			ObjectMapper objectMapper = new ObjectMapper();
			try {
				RpcDto rpcDto = objectMapper.readValue(json, RpcDto.class);
				methodName = rpcDto.methodName;
				params = rpcDto.params;
				paramsTypes = rpcDto.paramsTypes;

			} catch (JsonProcessingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

		public String getMethodName() {
			return methodName;
		}

		public void setMethodName(String methodName) {
			this.methodName = methodName;
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
