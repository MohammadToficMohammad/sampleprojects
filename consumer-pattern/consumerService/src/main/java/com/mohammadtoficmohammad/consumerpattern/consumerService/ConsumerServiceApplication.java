package com.mohammadtoficmohammad.consumerpattern.consumerService;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;
import org.springframework.context.annotation.ComponentScan;

@SpringBootApplication
@ComponentScan({"com.mohammadtoficmohammad.consumerpattern.**"})
public class ConsumerServiceApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(ConsumerServiceApplication.class)
		  .web(WebApplicationType.NONE)
		  .run(args);
	}

}
