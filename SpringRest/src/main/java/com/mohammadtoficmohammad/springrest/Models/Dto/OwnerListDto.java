package com.mohammadtoficmohammad.springrest.Models.Dto;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class OwnerListDto {
	
	public List<OwnerDto> ownerDtos=new ArrayList<>();
	
	public static OwnerListDto build(List<Owner> ownerList) 
	{
		var ownerListDto=new OwnerListDto();
		if(ownerList!=null)
		  ownerListDto.ownerDtos=(ownerList.stream().map(c->OwnerDto.build(c)).collect(Collectors.toList()));
		return ownerListDto;
	}
	
	
	public boolean success;
	public String  message;

}
