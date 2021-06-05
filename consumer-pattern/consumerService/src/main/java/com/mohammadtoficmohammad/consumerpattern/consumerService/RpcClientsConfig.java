package com.mohammadtoficmohammad.consumerpattern.consumerService;

import org.springframework.context.annotation.Bean;
import org.springframework.stereotype.Component;

import com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient.BusinessManagerRpcClient;

@Component
public class RpcClientsConfig {

	@Bean
	public BusinessManagerRpcClient getBusinessManagerRpcClient() {
		return new BusinessManagerRpcClient();
	}

}
