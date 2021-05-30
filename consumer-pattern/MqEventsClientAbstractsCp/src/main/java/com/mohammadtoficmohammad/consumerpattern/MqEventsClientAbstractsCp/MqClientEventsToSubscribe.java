package com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;


public class MqClientEventsToSubscribe {
	
	public static List<String> EventsToSubscribe = new ArrayList<String>();
	
	public MqClientEventsToSubscribe(List<String> _EventsToSubscribe) 
	{
		EventsToSubscribe=_EventsToSubscribe;
	}
	
	
    
}