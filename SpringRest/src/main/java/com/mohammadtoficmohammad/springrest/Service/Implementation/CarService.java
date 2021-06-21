package com.mohammadtoficmohammad.springrest.Service.Implementation;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mohammadtoficmohammad.springrest.Models.Dto.CarDto;
import com.mohammadtoficmohammad.springrest.Models.Dto.CarListDto;
import com.mohammadtoficmohammad.springrest.Models.Dto.OwnerDto;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Repository.CarRepository;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;

@Service
public class CarService implements ICarService {

	@Autowired
	CarRepository carRepository;

	@Override
	public CarDto saveCar(Car car) {

		var result = new CarDto();
		if (car == null) {
			result.success = false;
			result.message = "Null car not accepted";
			return result;
		}
		
		try {

			var carResult=carRepository.save(car);
			result=CarDto.build(carResult);
			result.success = true;
			result.message = "car saved";
			return result;

		} catch (Exception e) {
			result.success = false;
			result.message = "error happened";
			return result;
		}

	}

	@Override
	public CarDto getCarById(long carId) {

		var result = new CarDto();

		try {
			var car = carRepository.findById(carId);

			if (car.isEmpty()) {
				result.success = false;
				result.message = "no such car id exits";
				return result;
			}

			result = CarDto.build(car.get());
			result.success = true;
			result.message = "car exists";
			return result;

		} catch (Exception e) {
			result.success = false;
			result.message = "error happened";
			return result;
		}
	}

	@Override
	public CarListDto getAllCars() {

		var result = CarListDto.build(carRepository.findAll());
		result.success = true;
		result.message = "all existing cars";
		return result;
	}

}
