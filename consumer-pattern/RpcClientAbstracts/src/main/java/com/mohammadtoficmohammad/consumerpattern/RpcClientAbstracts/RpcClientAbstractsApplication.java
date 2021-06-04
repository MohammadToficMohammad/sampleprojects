package com.mohammadtoficmohammad.consumerpattern.RpcClientAbstracts;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.WebApplicationType;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;

@SpringBootApplication
public class RpcClientAbstractsApplication {

	public static void main(String[] args) {
		new SpringApplicationBuilder(RpcClientAbstractsApplication.class).web(WebApplicationType.NONE).run(args);
	}

}
