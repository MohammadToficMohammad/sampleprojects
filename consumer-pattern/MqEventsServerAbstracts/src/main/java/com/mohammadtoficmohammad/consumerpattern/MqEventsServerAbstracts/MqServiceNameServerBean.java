package com.mohammadtoficmohammad.consumerpattern.MqEventsServerAbstracts;

public class MqServiceNameServerBean {
	public static String name = "NotSet";

	public MqServiceNameServerBean(String _name, String localKey) {
		name = _name + localKey;
	}

	public MqServiceNameServerBean(String _name) {
		name = _name;
	}
}
