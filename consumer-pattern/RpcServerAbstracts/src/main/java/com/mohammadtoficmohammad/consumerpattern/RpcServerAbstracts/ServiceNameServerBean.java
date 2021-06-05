package com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts;

public class ServiceNameServerBean {
	public static String name = "NotSet";

	public ServiceNameServerBean(String _name, String localKey) {
		name = _name + localKey;
	}

	public ServiceNameServerBean(String _name) {
		name = _name;
	}
}