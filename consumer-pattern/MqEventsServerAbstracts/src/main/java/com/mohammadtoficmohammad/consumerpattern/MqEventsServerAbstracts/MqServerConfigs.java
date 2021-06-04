package com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstracts;

import java.util.Arrays;
import java.util.List;

import org.springframework.amqp.core.Binding;
import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.DirectExchange;
import org.springframework.amqp.core.FanoutExchange;
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
public class MqServerConfigs {

	@Autowired
	@Order(1)
	MqServiceNameServerBean serviceName;

	@Order(2)
	@Bean
	public FanoutExchange fanoutExchange() {
		return new FanoutExchange(serviceName.name + ".MqFanoutExchange");
	}

}
