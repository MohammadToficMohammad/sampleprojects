package com.mohammadtoficmohammad.consumerpattern.businessManager;

import org.springframework.context.annotation.Bean;
import org.springframework.stereotype.Service;

import com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts.IRpcHandler;
import com.mohammadtoficmohammad.consumerpattern.RpcServerAbstracts.ServiceNameServerBean;
import com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient.IbusinessManager;

import com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient.IbusinessManager;

@Service
public class RpcHandler implements IRpcHandler, IbusinessManager {

	public static volatile boolean lock = false;
	public static Object lockLock = new Object();

	@Bean
	public ServiceNameServerBean getServiceName() {
		return new ServiceNameServerBean("businessManager");
	}

	@Override
	public boolean checkLock() {
		// TODO Auto-generated method stub
		return lock;
	}

	@Override
	public void setLock() {
		// TODO Auto-generated method stub
		synchronized (lockLock) {
			lock = true;
		}

	}

	@Override
	public void freeLock() {
		// TODO Auto-generated method stub
		synchronized (lockLock) {
			lock = false;
		}

	}

	@Override
	public boolean checkAndSetLock() {
		synchronized (lockLock) {
			if (lock)
				return false;
			else {
				lock = true;
				return true;
			}
		}

	}

}
