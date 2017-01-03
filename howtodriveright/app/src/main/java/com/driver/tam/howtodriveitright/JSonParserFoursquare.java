package com.driver.tam.howtodriveitright;

import com.driver.tam.howtodriveitright.ObjectLatLng;import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class JSonParserFoursquare {
	
	public JSonParserFoursquare() {
		super();
	}

	public ArrayList<ObjectLocation> getDataFourSquare(String resStr) throws JSONException {
		ArrayList<ObjectLocation> arrayLocation = new ArrayList<ObjectLocation>();
		
		JSONObject jSonObject  =  new JSONObject(resStr);
		JSONArray  jsonArray   =  jSonObject.getJSONObject("response").getJSONArray("venues");
		
		if (jsonArray != null) {
			int size = jsonArray.length();

			JSONObject obj;
			for (int i = 0; i < size; i++) {

				ObjectLocation tmpLocation = new ObjectLocation();
				obj = (JSONObject) jsonArray.get(i);
				if (obj != null) {
					//---1
					tmpLocation.nameLocation = (obj.getString("name"));
					//---2
					JSONObject loc = obj.getJSONObject("location");
					Double lat = loc.getDouble("lat"); 
					Double lng = loc.getDouble("lng"); 
					tmpLocation.mObjLatLng = new ObjectLatLng(lat, lng);
					
					//---3
					if (obj.getJSONObject("location").has("address")) {
						tmpLocation.addVenues  = loc.getString("address");
					} else {
						tmpLocation.addVenues  = "chua xac dinh";
					}
					
					//---4
					JSONArray  category    =  obj.getJSONArray("categories");
					if (category.length() != 0) {
						JSONObject objCategory  =  category.getJSONObject(0);
						JSONObject icon  =  objCategory.getJSONObject("icon");
						String link      =  icon.getString("prefix");
						String linknew   =  ((String) link.subSequence(0, link.length() - 1)) + ".png";
						tmpLocation.linkIcon = linknew;
//						Log.i("xxx", "name = "+obj.getString("name"));
//						Log.i("xxx", "icon = "+linknew);
					} else {
						tmpLocation.linkIcon = "https://foursquare.com/img/categories_v2/building/default.png";
					}
		
					
					/*Log.i("xxx", "name = "+String.valueOf(obj.getString("name")));
					Log.i("xxx", "lat  = "+String.valueOf(lat)+ "  lng = "+String.valueOf(lng));
					Log.i("xxx", "add  = "+String.valueOf(tmpLocation.getAddVenues()));*/
	
				}

				arrayLocation.add(tmpLocation);
			}
			return arrayLocation;
		} else {
			return null;
		}	
	}


}
