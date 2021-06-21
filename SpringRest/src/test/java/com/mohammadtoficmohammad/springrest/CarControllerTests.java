package com.mohammadtoficmohammad.springrest;

import static org.assertj.core.api.Assertions.assertThat;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;

import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.invocation.InvocationOnMock;
import org.mockito.stubbing.Answer;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.ResponseEntity;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import com.mohammadtoficmohammad.springrest.Controllers.CarsController;
import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Service.Implementation.CarService;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;
import com.mohammadtoficmohammad.springrest.Models.Dto.CarDto;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;

@SpringBootTest
public class CarControllerTests {

	@InjectMocks
    CarsController carsController;
     
    @Mock
    ICarService carService;
    
    
    @Test
    public void testSaveCar() 
    {
        MockHttpServletRequest request = new MockHttpServletRequest();
        RequestContextHolder.setRequestAttributes(new ServletRequestAttributes(request));
         
        when(carService.saveCar(any(Car.class))).//thenReturn(new Car());
        thenAnswer(new Answer<CarDto>() {
            @Override
            public CarDto answer(InvocationOnMock invocation) throws Throwable {
              Object[] args = invocation.getArguments();
              var car=(Car) args[0];
              var carDto=CarDto.build(car);
              carDto.success=true;
              carDto.message="good";
              return carDto;
              
            }
          });
        
       
         
        var carDto = new CarDto(1, CarModel.BMW ,"BLACK");
        ResponseEntity<CarDto> responseEntity = carsController.saveCar(carDto);
         
        assertThat(responseEntity.getStatusCodeValue()).isEqualTo(200);
        assertThat(responseEntity.getBody().getModel()).isEqualTo(CarModel.BMW);
    }
	
}
