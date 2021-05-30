package com.mohammadtoficmohammad.consumerpattern.businessManager;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;
import org.springframework.context.annotation.ComponentScan;


@SpringBootApplication
@ComponentScan({"com.mohammadtoficmohammad.consumerpattern.**"})
public class BusinessManagerApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(BusinessManagerApplication.class)
		  .web(WebApplicationType.NONE)
		  .run(args);
	}

}
