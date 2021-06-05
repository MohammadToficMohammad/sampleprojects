package com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient;

import java.util.Arrays;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Service;
import com.mohammadtoficmohammad.consumerpattern.RpcClientAbstracts.RpcClientAbstract;

@Primary
//@Service
public class BusinessManagerRpcClient extends RpcClientAbstract implements IbusinessManager {

	public BusinessManagerRpcClient(String localKey) {
		ServiceName = "businessManager" + localKey;
	}

	public BusinessManagerRpcClient() {
		ServiceName = "businessManager";
	}

	@Override
	public boolean checkLock() {
		// var response=Rpc(Arrays.asList(UserId));
		var response = Rpc(Arrays.asList());
		return (boolean) response;
	}

	@Override
	public void setLock() {

		var response = Rpc(Arrays.asList());

	}

	@Override
	public void freeLock() {
		// TODO Auto-generated method stub
		var response = Rpc(Arrays.asList());
	}

	@Override
	public boolean checkAndSetLock() {
		var response = Rpc(Arrays.asList());
		return (boolean) response;
	}

}
