package com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstractsCp;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;



@SpringBootApplication
public class MqClientAbstractsApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(MqClientAbstractsApplication.class)
		  .web(WebApplicationType.NONE)
		  .run(args);
	}

}
