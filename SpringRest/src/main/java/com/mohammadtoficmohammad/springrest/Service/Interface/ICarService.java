package com.mohammadtoficmohammad.springrest.Service.Interface;

import java.util.List;
import java.util.Optional;

import com.mohammadtoficmohammad.springrest.Models.Dto.CarDto;
import com.mohammadtoficmohammad.springrest.Models.Dto.CarListDto;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;

public interface ICarService {
	
	public CarDto saveCar(CarDto carDto) ;
	public CarDto getCarById(long carId) ;
	public CarListDto getAllCars() ;
	

}
