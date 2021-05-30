package com.mohammadtoficmohammad.springrest.Service.Interface;

import java.util.List;
import java.util.Optional;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Vo.CarVo;

public interface ICarService {
	
	public Car saveCar(Car car) ;
	public Car saveCar(CarVo carVo) ;
	public Optional<Car> getCarById(long id) ;
	public List<Car> getAllCars() ;
	

}
