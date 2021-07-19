package com.mohammadtoficmohammad.springrest.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mohammadtoficmohammad.springrest.Models.Entity.Owner;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;


@Repository
public interface OwnerRepository extends JpaRepository<Owner, Long> {

	@Transactional(propagation = Propagation.SUPPORTS)
	@Query("SELECT o FROM Owner o LEFT JOIN FETCH o.cars WHERE o.id = (:id)")
	public Owner findByIdAndFetchCarsEagerly(@Param("id") Long id);
}
