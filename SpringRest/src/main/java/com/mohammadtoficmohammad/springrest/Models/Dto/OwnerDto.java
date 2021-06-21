package com.mohammadtoficmohammad.springrest.Models.Dto;

import java.util.stream.Collectors;

import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class OwnerDto {
	

	public long ownerId;
	
	public String firstName;
	
	public String lastName;
	
	public CarListDto carListDto;

	
	public Owner buildNewOwner() 
	{
		var owner=new Owner();
		owner.setFirstName(firstName);
		owner.setLastName(lastName);
		owner.cars=carListDto.carDtos.stream().map(cd->cd.buildNewCar()).collect(Collectors.toList());
		System.out.println("asdasda "+ owner.cars.size());
		return owner;
	}
	
	public static OwnerDto build(Owner owner) 
	{
		var ownerDto=new OwnerDto();
		ownerDto.ownerId=owner.getOwnerId();
		ownerDto.firstName=owner.getFirstName();
		ownerDto.lastName=owner.getLastName();
		ownerDto.carListDto=CarListDto.build(owner.getCars());
		return ownerDto;
	}
	
	public OwnerDto(long ownerId,String firstName,String lastName) 
	{
		this.ownerId=ownerId;
		this.firstName=firstName;
		this.lastName=lastName;
	}
	
	public boolean success;
	public String  message;

}
