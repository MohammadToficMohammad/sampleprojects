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

import com.mohammadtoficmohammad.springrest.Controllers.CarController;
import com.mohammadtoficmohammad.springrest.Models.Entity.CarModel;
import com.mohammadtoficmohammad.springrest.Service.Implementation.CarService;
import com.mohammadtoficmohammad.springrest.Service.Interface.ICarService;
import com.mohammadtoficmohammad.springrest.Models.Entity.Car;

@SpringBootTest
public class CarControllerTests {

	@InjectMocks
    CarController carController;
     
    @Mock
    ICarService carService;
    
    
    @Test
    public void testSaveCar() 
    {
        MockHttpServletRequest request = new MockHttpServletRequest();
        RequestContextHolder.setRequestAttributes(new ServletRequestAttributes(request));
         
        when(carService.saveCar(any(Car.class))).//thenReturn(new Car());
        thenAnswer(new Answer<Car>() {
            @Override
            public Car answer(InvocationOnMock invocation) throws Throwable {
              Object[] args = invocation.getArguments();
              return (Car) args[0];
            }
          });
        
       
         
        Car car = new Car(1, CarModel.BMW ,"BLACK",null);
        ResponseEntity<Car> responseEntity = carController.saveCar(car);
         
        assertThat(responseEntity.getStatusCodeValue()).isEqualTo(200);
        assertThat(responseEntity.getBody().getModel()).isEqualTo(CarModel.BMW);
    }
	
}
