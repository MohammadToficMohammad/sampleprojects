package com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

@Component
public class QueueName {

	@Autowired
	@Order(1)
	ServiceNameServerBean serviceName;

	/*
	 * Or like this
	 * 
	 * @Value("${spring.application.name}") private String serviceName;
	 */

	public String buildFor(String name) {
		return serviceName.name + ".rpc.requests";
	}
}