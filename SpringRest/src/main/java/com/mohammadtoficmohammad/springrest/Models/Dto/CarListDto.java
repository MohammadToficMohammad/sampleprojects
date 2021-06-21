package com.mohammadtoficmohammad.springrest.Models.Dto;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CarListDto {
	
	public List<CarDto> carDtos=new ArrayList<>();
	
	public static CarListDto build(List<Car> carList) 
	{
		var carListDto=new CarListDto();
		if(carList!=null)
		  carListDto.carDtos=(carList.stream().map(c->CarDto.build(c)).collect(Collectors.toList()));
		return carListDto;
	}
	
	
	
	public boolean success;
	public String  message;

}
