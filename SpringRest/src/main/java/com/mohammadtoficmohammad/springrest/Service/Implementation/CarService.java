package com.mohammadtoficmohammad.springrest.Service.Implementation;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Vo.CarVo;
import com.mohammadtoficmohammad.springrest.Repository.CarRepository;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;

@Service
public class CarService implements ICarService {
	
	@Autowired
	CarRepository carRepository;
	
	
	public Car saveCar(Car car) 
	{
		carRepository.save(car);
		return car;
	}
	
	public Car saveCar(CarVo carVo) 
	{
		var car=new Car();
		car.setColor(carVo.color);
		car.setModel(carVo.model);
		carRepository.save(car);
		return car;
	}

	public Optional<Car> getCarById(long id) 
	{
		return carRepository.findById(id);
	}
	
	public List<Car> getAllCars() 
	{
		return carRepository.findAll();
	}
}
