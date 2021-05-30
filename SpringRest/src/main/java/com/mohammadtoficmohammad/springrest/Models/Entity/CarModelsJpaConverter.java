package com.mohammadtoficmohammad.springrest.Models.Entity;

import javax.persistence.AttributeConverter;
import javax.persistence.Converter;

@Converter
public class CarModelsJpaConverter implements AttributeConverter<CarModel, String> {

 
@Override
public String convertToDatabaseColumn(CarModel attribute) {

	return attribute.name();
}

@Override
public CarModel convertToEntityAttribute(String dbData) {

	return CarModel.valueOf(dbData);
}

}