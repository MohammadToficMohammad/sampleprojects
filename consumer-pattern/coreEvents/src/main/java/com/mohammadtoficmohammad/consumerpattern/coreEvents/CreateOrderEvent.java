package com.mohammadtoficmohammad.consumerpattern.coreEvents;

import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.List;

public class CreateOrderEvent<T> extends EventBase<T> {
	
public static final String name="createOrder";
public String orderName;
public int orderPrice;

	    public CreateOrderEvent(Class<T> typeParameterClass) {
	    	super(typeParameterClass);
	    	namep=name;
	    }

	

	public T build(String name,int price) {
		params=Arrays.asList(name,price);
		return super.build();
	}


	public void Deserialize(T eventIn) 
	{
		var flist=super.Deserializep(eventIn);
		orderName=(String)flist.get(0);
		orderPrice=(int)flist.get(1);
		
	}

}
