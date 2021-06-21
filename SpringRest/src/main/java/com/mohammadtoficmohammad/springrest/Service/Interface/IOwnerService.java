package com.mohammadtoficmohammad.springrest.Service.Interface;

import java.util.Optional;

import com.mohammadtoficmohammad.springrest.Models.Dto.OwnerDto;
import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;

public interface IOwnerService {

	public OwnerDto saveOwner(OwnerDto ownerDto) ;
	public OwnerDto getOwnerById(long ownerId) ;
	public OwnerDto getOwnerIncludeCarsById(long ownerId) ;
}
