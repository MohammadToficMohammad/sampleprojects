package com.mohammadtoficmohammad.springrest;

import java.util.ArrayList;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.ApplicationArguments;
import org.springframework.boot.ApplicationRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.stereotype.Component;

import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Models.Dto.OwnerDto;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;
import com.mohammadtoficmohammad.springrest.Service.Implementation.CarService;
import com.mohammadtoficmohammad.springrest.Service.Implementation.OwnerService;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;
import com.mohammadtoficmohammad.springrest.Service.Interface.IOwnerService;



@Configuration
public class Initializer implements ApplicationRunner {

    @Autowired
    ICarService carService;
    
    @Autowired
    IOwnerService ownerService;


	@Override
	public void run(ApplicationArguments args) throws Exception {
		
	    // we can initialize db here or in data.sql in data.sql ,in data.sql we have to update last index also
		var owner=new Owner();
		owner.setFirstName("Mohammad");
		owner.setLastName("Tofic");
		
		
		var car1=new Car();
		car1.setColor("BLACK");
		car1.setModel(CarModel.BMW);
		car1.setOwner(owner);
		//carService.saveCar(car1);
		
		var car2=new Car();
		car2.setColor("BLUE");
		car2.setModel(CarModel.VOLVO);
		car2.setOwner(owner);
		//car2=carService.saveCar(car2);
		
	
		owner.cars.add(car1);
		owner.cars.add(car2);
		//cascade all will persist the cars
		var result=ownerService.saveOwner(OwnerDto.build(owner));

		
		
		
		var ownerDto=ownerService.getOwnerIncludeCarsById(result.getOwnerId());
		System.out.println(ownerDto.carListDto.carDtos.size());
		for(var o :ownerDto.carListDto.carDtos) 
		{
			System.out.println(o.getColor());
		}
		
		
		
	
	}
}