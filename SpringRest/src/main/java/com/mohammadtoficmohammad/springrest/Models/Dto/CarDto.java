package com.mohammadtoficmohammad.springrest.Models.Dto;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CarDto {
	
	public long carId;
	
	public CarModel model;
	
	public String color;
	
	public Car buildNewCar() 
	{
		var car=new Car();
		car.setCarId(carId);
		car.setModel(model);
		car.setColor(color);
		return car;
	}
	
	public static CarDto build(Car car) 
	{
		return new CarDto(car.getCarId(),car.getModel(),car.getColor());
	}
	
	public CarDto(long carId,CarModel model,String color) 
	{
		this.carId=carId;
		this.model=model;
		this.color=color;
	}
	
	public boolean success;
	public String  message;
}
