package com.mohammadtoficmohammad.consumerpattern.gatewayService;

import org.springframework.context.annotation.Bean;
import org.springframework.stereotype.Component;

import com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstracts.MqServiceNameServerBean;

@Component
public class MqEventsServerConfig {
	@Bean
	public MqServiceNameServerBean getMqServiceNameServerBean() {
		return new MqServiceNameServerBean("gatewayService");
	}
}
