package com.mohammadtoficmohammad.consumerpattern.RpcClientAbstracts;

import java.lang.reflect.Method;
import java.util.List;

import org.springframework.amqp.core.DirectExchange;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

public abstract class RpcClientAbstract {

	public String ServiceName = "NotSet";

	public String RoutingKey = "RpcRoutingKey";

	@Autowired
	private RabbitTemplate template;

	// @Autowired
	// private DirectExchange exchange;

	protected Object Rpc(List<Object> params) {

		StackTraceElement[] list = Thread.currentThread().getStackTrace();
		/*
		 * for(StackTraceElement element : list) { System.out.println( element);
		 * 
		 * }
		 */
		var classname = Thread.currentThread().getStackTrace()[2].getClassName();
		Class<?> classIns = null;
		try {
			classIns = Class.forName(classname);
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		var methods = classIns.getMethods();
		String methodName = Thread.currentThread().getStackTrace()[2].getMethodName();
		Method method = null;
		;
		for (var m : methods) {
			// System.out.println(Thread.currentThread().getStackTrace()[2].getClassLoaderName());
			// System.out.println(m.getName());
			if (m.getName().equals(methodName)) {
				method = m;
				break;
			}
			;

		}
		Class<?>[] paramsTypes = method.getParameterTypes();
		var rpcDtoJson = (new RpcDto(methodName, params, paramsTypes)).GetJson();
		if (rpcDtoJson == null)
			return null;
		// System.out.println("MQQT "+ ServiceName);
		Object response = template.convertSendAndReceive(ServiceName + ".rpc.directexchange"
		// exchange.getName()
				, RoutingKey, rpcDtoJson);
		return response;
	}

	private class RpcDto {

		public String methodName;
		public List<Object> params;
		public Class<?>[] paramsTypes;

		public RpcDto(String _methodName, List<Object> _params, Class<?>[] _paramsTypes) {
			methodName = _methodName;
			params = _params;
			paramsTypes = _paramsTypes;
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
