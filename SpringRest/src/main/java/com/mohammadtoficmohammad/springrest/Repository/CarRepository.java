package com.mohammadtoficmohammad.springrest.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mohammadtoficmohammad.springrest.Models.Entity.Car;

@Repository
public interface CarRepository extends JpaRepository<Car, Long>{

}
