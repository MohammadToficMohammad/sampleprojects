package com.mohammadtoficmohammad.springrest.Controllers;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Vo.CarVo;
import com.mohammadtoficmohammad.springrest.Service.Implementation.CarService;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;

import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("/cars")
@Slf4j
public class CarController {

	@Autowired
	ICarService carService;
	
	@PostMapping("")
	public ResponseEntity<Car> saveCar(@RequestBody Car car) 
	{
		log.info("new car has been added...");
		return ResponseEntity.ok(carService.saveCar(car));
	}
	
	
	@GetMapping("") //
	public ResponseEntity<List<Car>> getAllCars() 
	{
		return ResponseEntity.ok(carService.getAllCars());
	}
	
	
	@GetMapping("/{id}") 
	public ResponseEntity<Optional<Car>> getCar(@PathVariable("id") long id) 
	{
		return ResponseEntity.ok(carService.getCarById(id));
	}
}
