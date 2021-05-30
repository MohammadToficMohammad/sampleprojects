package com.mohammadtoficmohammad.springrest.Service.Implementation;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.transaction.Transactional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;
import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;
import com.mohammadtoficmohammad.springrest.Repository.CarRepository;
import com.mohammadtoficmohammad.springrest.Repository.OwnerRepository;
import com.mohammadtoficmohammad.springrest.Service.Interface.IOwnerService;

@Service
public class OwnerService implements IOwnerService{

	@Autowired
	OwnerRepository ownerRepository;

	@PersistenceContext
	private EntityManager entityManager;

	public Owner saveOwner(Owner owner) {
		ownerRepository.save(owner);
		return owner;
	}

	public Optional<Owner> getOwnerById(long id) {
		var owner = ownerRepository.findById(id);
		return owner;
	}

	public Optional<Owner> getOwnerIncludeCarsById(long id) {
		var owner = ownerRepository.findByIdAndFetchCarsEagerly(id);
		return Optional.of(owner);
	}
	
	
}


