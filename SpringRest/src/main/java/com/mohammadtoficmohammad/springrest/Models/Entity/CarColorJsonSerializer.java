package com.mohammadtoficmohammad.springrest.Models.Entity;

import java.io.IOException;

import com.fasterxml.jackson.core.JsonGenerator;
import com.fasterxml.jackson.databind.JsonSerializer;
import com.fasterxml.jackson.databind.SerializerProvider;

public class CarColorJsonSerializer extends JsonSerializer<String> {


	@Override
	public void serialize(String value, JsonGenerator gen, SerializerProvider serializers) throws IOException
	{
		
		gen.writeString("Color is: "+value);
	}
	

}
