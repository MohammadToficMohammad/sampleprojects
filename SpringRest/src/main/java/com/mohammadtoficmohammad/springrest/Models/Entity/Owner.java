package com.mohammadtoficmohammad.springrest.Models.Entity;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Owner {

	@Id
	@GeneratedValue(strategy = GenerationType.AUTO)
	private long OwnerId;

	@Column
	private String FirstName;
	
	@Column
	private String LastName;

	@OneToMany(mappedBy = "owner",cascade =CascadeType.ALL,fetch = FetchType.LAZY)
	public List<Car> cars = new ArrayList<>(); //lombok has bug with hashset

}
