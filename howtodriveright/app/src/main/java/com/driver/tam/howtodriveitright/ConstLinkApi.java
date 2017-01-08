package com.driver.tam.howtodriveitright;

import android.util.Log;

import com.driver.tam.howtodriveitright.ObjectLatLng;

import java.util.Calendar;


public class ConstLinkApi {
	
	public ConstLinkApi() {
		super();
	}
	
	/**
	 * 
	 * @param ObjectLatLng
	 * get lat(kinhdo), lng(vido) from ObjLatLng
	 * 
	 */
	public String getApiLocation(ObjectLatLng obj) {
		
		String API_LOCATION = "";
		
		API_LOCATION    =   "https://api.foursquare.com/v2/venues/search?client_id=C3ML2TDLI315P4WG2C0GSIHTLX3NTEG005P5UUAAKVODKB0R" +
							"&client_secret=LKACZPLHNPQ5TGN2HL1SIVAMU0Z2JGG0RMMDQV3NW3ARMTBE" +
							"&limit=50"+
							"&intent=browse"+
							"&radius=800"+
							"&ll="+String.valueOf(obj.getmLat())+","+String.valueOf(obj.getmLng()) +
							"&v="+currentTime();
		
		Log.i("xxx", "API = "+API_LOCATION);
		return API_LOCATION;
	}

	
	//TODO: convert yyyy/m/d >>> yyyy/0m/0d
	public String addZero(int num) {
		String newNumber = "";
		if (String.valueOf(num).length() == 1) {
			newNumber = "0"+num;
		} else {
			newNumber = String.valueOf(num);
		}
		
		return newNumber;
	}
	
	
	public String currentTime() {
		String currentDateTime = "";
		final Calendar c =  Calendar.getInstance();
		int   month      =  c.get(Calendar.MONTH);
		int   day        =  c.get(Calendar.DAY_OF_MONTH);
		int   year       =  c.get(Calendar.YEAR);
		
		currentDateTime  = currentDateTime + year + addZero(month+1) + addZero(day);
		
		Log.i("xxx", currentDateTime);
		
		return currentDateTime;
	}
}
