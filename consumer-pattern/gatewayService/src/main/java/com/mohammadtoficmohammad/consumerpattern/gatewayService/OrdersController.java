package com.mohammadtoficmohammad.consumerpattern.gatewayService;

import java.util.Arrays;
import java.util.UUID;
import java.util.concurrent.CompletableFuture;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.sleuth.Span;
import org.springframework.cloud.sleuth.Tracer;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstractsCp.MqEventSender;
import com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstractsCp.MqEventSender.EventSenderDto;
import com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient.BusinessManagerRpcClient;
import com.mohammadtoficmohammad.consumerpattern.coreEvents.CreateOrderEvent;


@RestController
@RequestMapping(value = "/api/orders")
public class OrdersController {
	
	@Autowired
	MqEventSender mqEventsSender;
	
	@Autowired
	public Tracer tracer;
	
	@Autowired
	BusinessManagerRpcClient  businessManagerClient;
	
	@PostMapping
    public String createOrder(@RequestBody OrderCreateDTO orderCreateDTO){
	 	System.out.println(businessManagerClient.checkLock());
	 	if(businessManagerClient.checkAndSetLock()) {
		var event=new CreateOrderEvent<EventSenderDto>(EventSenderDto.class).build(orderCreateDTO.name, orderCreateDTO.price);
		System.out.println(event.eventName);
		Span span = this.tracer.currentSpan();
		span.tag("orderName", orderCreateDTO.name);
		span.tag("orderPrice", Integer.toString(orderCreateDTO.price));
   // span.event("CreateOrderEvent: "+"orderName:"+orderCreateDTO.name+" orderPrice:"+orderCreateDTO.price);
		mqEventsSender.send(event);
		return "Order in processing...";
	 	}else {
	 		
	 		return "there is another Order in processing...";
	 	}
    }
	

}
