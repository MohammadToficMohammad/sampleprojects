package com.mohammadtoficmohammad.springrest.Service.Implementation;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Optional;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.transaction.Transactional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mohammadtoficmohammad.springrest.Models.Dto.CarDto;
import com.mohammadtoficmohammad.springrest.Models.Dto.OwnerDto;
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

	@Override
	public OwnerDto saveOwner(Owner owner) {
		var result = new OwnerDto();
		if (owner == null) {
			result.success = false;
			result.message = "Null car not accepted";
			return result;
		}
		
		try {

			var ownerResult=ownerRepository.save(owner);
			result=OwnerDto.build(ownerResult);
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
	public OwnerDto getOwnerById(long ownerId) {
		var result = new OwnerDto();

		try {
			var owner = ownerRepository.findById(ownerId);

			if (owner.isEmpty()) {
				result.success = false;
				result.message = "no such owner id exits";
				return result;
			}

			result = OwnerDto.build(owner.get());
			result.success = true;
			result.message = "owner exists";
			return result;

		} catch (Exception e) {
			result.success = false;
			result.message = "error happened";
			return result;
		}
	}

	@Override
	public OwnerDto getOwnerIncludeCarsById(long ownerId) {
		var result = new OwnerDto();

		try {
			var owner = ownerRepository.findByIdAndFetchCarsEagerly(ownerId);

			if (owner==null) {
				result.success = false;
				result.message = "no such owner id exits";
				return result;
			}

			result = OwnerDto.build(owner);
			result.success = true;
			result.message = "owner exists";
			return result;

		} catch (Exception e) {
			result.success = false;
			result.message = "error happened";
			return result;
		}
	}

	
	
	
}


