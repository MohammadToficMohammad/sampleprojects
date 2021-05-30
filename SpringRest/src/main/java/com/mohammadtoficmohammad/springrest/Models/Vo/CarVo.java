package com.mohammadtoficmohammad.springrest.Models.Vo;

import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CarVo {
	
	public CarModel model;
	
	public String color;

}
