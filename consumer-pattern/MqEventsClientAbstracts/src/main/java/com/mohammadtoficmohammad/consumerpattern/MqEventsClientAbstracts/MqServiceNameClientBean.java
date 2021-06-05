package com.mohammadtoficmohammad.consumerpattern.MqEventsClientAbstracts;

public class MqServiceNameClientBean {
	public static String name = "NotSet";

	public MqServiceNameClientBean(String _name, String localKey) {
		name = _name + localKey;
	}

	public MqServiceNameClientBean(String _name) {
		name = _name;
	}
}
