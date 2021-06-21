package com.mohammadtoficmohammad.springrest.Controllers;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import com.mohammadtoficmohammad.springrest.Models.Dto.CarDto;
import com.mohammadtoficmohammad.springrest.Models.Dto.CarListDto;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;

import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("/cars")
@Slf4j
public class CarsController {

	@Autowired
	ICarService carService;
	
	@PostMapping("")
	public ResponseEntity<CarDto> saveCar(@RequestBody CarDto carDto) 
	{
		log.info("new car request...");
		var result=carService.saveCar(carDto);
		return result.success? ResponseEntity.ok(result): new ResponseEntity<>(result, HttpStatus.BAD_REQUEST);
	}
	
	
	@GetMapping("") 
	public ResponseEntity<CarListDto> getAllCars() 
	{
		var result=carService.getAllCars();
		return result.success? ResponseEntity.ok(result): new ResponseEntity<>(result, HttpStatus.NOT_FOUND);
	}
	
	
	@GetMapping("/{id}") 
	public ResponseEntity<CarDto> getCar(@PathVariable("id") long id) 
	{
		var result=carService.getCarById(id);
		return result.success? ResponseEntity.ok(result): new ResponseEntity<>(result, HttpStatus.NOT_FOUND);
	
	}
}
