package com.driver.tam.howtodriveitright;

import com.driver.tam.howtodriveitright.ObjectLatLng;public class ObjectLocation {

    public String nameLocation;
    public ObjectLatLng mObjLatLng;
    public String addVenues;
    public String linkIcon;

    public ObjectLocation() {
        super();
    }

    public ObjectLocation(String nameLocation, ObjectLatLng mObjLatLng, String addVenues, String linkIcon) {
        super();
        this.setNameLocation(nameLocation);
        this.setmObjLatLng(mObjLatLng);
        this.setAddVenues(addVenues);
        this.setLinkIcon(linkIcon);
    }

    //@@@
    public String getNameLocation() {
        return nameLocation;
    }

    public void setNameLocation(String nameLocation) {
        this.nameLocation = nameLocation;
    }

    //@@@
    public ObjectLatLng getmObjLatLng() {
        return mObjLatLng;
    }

    public void setmObjLatLng(ObjectLatLng mObjLatLng) {
        this.mObjLatLng = mObjLatLng;
    }

    //@@@
    public String getAddVenues() {
        return addVenues;
    }

    public void setAddVenues(String addVenues) {
        this.addVenues = addVenues;
    }

    //@@@
    public String getLinkIcon() {
        return linkIcon;
    }

    public void setLinkIcon(String linkIcon) {
        this.linkIcon = linkIcon;
    }

}
