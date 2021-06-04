package com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstracts;

import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

@Component
public class MqClientQueueName {

	/*
	 * @Autowired MqClientEventsToSubscribe mqClientEventsToSubscribe;
	 * 
	 * 
	 * public String[] buildFor(String name) {
	 * 
	 * var list=mqClientEventsToSubscribe.EventsToSubscribe.stream().map(x->x+
	 * ".mqevents").collect(Collectors.toList()); String[] listArr=list.toArray(new
	 * String[0]); return listArr; }
	 */

	@Order(1)
	@Autowired
	public MqServiceNameClientBean serviceName;

	public String buildFor(String name) {

		return serviceName.name + ".mqevents";
	}

}
