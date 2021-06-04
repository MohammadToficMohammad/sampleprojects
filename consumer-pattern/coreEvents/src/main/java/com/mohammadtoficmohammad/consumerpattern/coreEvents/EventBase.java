package com.mohammadtoficmohammad.consumerpattern.coreEvents;

import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.List;

public class EventBase<T> {

	final Class<T> typeParameterClass;
	public String namep = "notset";

	public EventBase(Class<T> typeParameterClass) {
		this.typeParameterClass = typeParameterClass;
	}

	public Object params;

	public T build() {
		Field f;
		try {
			T eventIn = typeParameterClass.getDeclaredConstructor().newInstance();
			f = eventIn.getClass().getDeclaredField("eventName");
			f.setAccessible(true);
			f.set(eventIn, namep);

			f = eventIn.getClass().getDeclaredField("params");
			f.setAccessible(true);
			f.set(eventIn, params);

			return eventIn;
		} catch (NoSuchFieldException | SecurityException | IllegalArgumentException | IllegalAccessException
				| InstantiationException | InvocationTargetException | NoSuchMethodException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return null;
	}

	public List<Object> Deserializep(T eventIn) {
		Field f;
		try {
			f = eventIn.getClass().getDeclaredField("params");
			List<Object> paramsList = (List<Object>) f.get(eventIn);

			return paramsList;
		} catch (NoSuchFieldException | SecurityException | IllegalArgumentException | IllegalAccessException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return null;
	}

}
