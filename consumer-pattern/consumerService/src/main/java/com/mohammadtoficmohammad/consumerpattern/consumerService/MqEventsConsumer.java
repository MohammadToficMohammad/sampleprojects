package com.mohammadtoficmohammad.consumerpattern.consumerService;

import java.util.Arrays;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.sleuth.ScopedSpan;
import org.springframework.cloud.sleuth.Span;
import org.springframework.cloud.sleuth.Tracer;
import org.springframework.cloud.sleuth.annotation.NewSpan;
import org.springframework.context.annotation.Bean;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp.MqClientAbstract;
import com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp.MqClientEventsToSubscribe;
import com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp.MqServiceNameClientBean;
import com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient.BusinessManagerRpcClient;
import com.mohammadtoficmohammad.consumerpattern.coreEvents.CreateOrderEvent;

@Component
public class MqEventsConsumer extends MqClientAbstract{

	@Autowired
	BusinessManagerRpcClient  businessManagerClient;
	
	@Autowired
	public Tracer tracer;
	
	@Order(0)
	@Bean 
	public MqServiceNameClientBean getMqServiceNameClientBean() 
	{
	return new	MqServiceNameClientBean("consumerService");
	}
	
    @Order(1)
	@Bean 
	public MqClientEventsToSubscribe getMqClientEventsToSubscribe() 
	{
	return new	MqClientEventsToSubscribe(Arrays.asList("gatewayService"));
	}
	
	@Override
    public  Object handleEvent(EventClientDto event) {

		switch (event.eventName) {
		case CreateOrderEvent.name:
			System.out.println("inside event handler :"+ event.eventName);
			var eventObj= new CreateOrderEvent<EventClientDto>(EventClientDto.class);
			eventObj.Deserialize(event);
			System.out.println(eventObj.orderName);
			System.out.println(eventObj.orderPrice);
			
			
			try {
				Thread.sleep(10000);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			

			businessManagerClient.freeLock();
			break;

		default:
			break;
		}
		
		
		
		return event;
		
	}
	
}

