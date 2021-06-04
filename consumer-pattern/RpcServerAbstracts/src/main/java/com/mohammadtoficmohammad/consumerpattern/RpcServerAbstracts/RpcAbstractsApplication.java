package com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;

@SpringBootApplication
public class RpcAbstractsApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(RpcAbstractsApplication.class).web(WebApplicationType.NONE).run(args);
	}

}
