package com.mohammadtoficmohammad.springrest.Service.Interface;

import java.util.Optional;

import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;

public interface IOwnerService {

	public Owner saveOwner(Owner owner) ;
	public Optional<Owner> getOwnerById(long id) ;
	public Optional<Owner> getOwnerIncludeCarsById(long id) ;
}
