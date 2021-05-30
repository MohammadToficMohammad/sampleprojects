package com.mohammadtoficmohammad.springrest;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ComponentScan;

@SpringBootApplication
@ComponentScan(basePackages = {"com.mohammadtoficmohammad.springrest.**"})
public class SpringRestBootApplication {

	public static void main(String[] args) {
		SpringApplication.run(SpringRestBootApplication.class, args);
	}

}
