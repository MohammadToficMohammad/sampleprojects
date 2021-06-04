package com.mohammadtoficmohammad.consumerpattern.businessManagerRpcClient;

public interface IbusinessManager {

	public boolean checkLock();

	public void setLock();

	public void freeLock();

	public boolean checkAndSetLock();

}
