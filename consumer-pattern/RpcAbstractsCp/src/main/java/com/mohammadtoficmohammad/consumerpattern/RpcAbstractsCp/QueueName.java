package com.mohammadtoficmohammad.consumerpattern.RpcAbstractsCp;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

@Component
public class QueueName {
	

	   @Autowired
	   @Order(1)
	   ServiceNameServerBean serviceName;
	
    public String buildFor(String name) {
        return serviceName.name+".rpc.requests";
    }
}