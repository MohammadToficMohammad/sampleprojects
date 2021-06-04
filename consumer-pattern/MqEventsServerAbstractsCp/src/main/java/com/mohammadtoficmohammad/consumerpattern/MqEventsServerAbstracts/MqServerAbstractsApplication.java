package com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstracts;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;

@SpringBootApplication
public class MqServerAbstractsApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(MqServerAbstractsApplication.class).web(WebApplicationType.NONE).run(args);
	}

}
