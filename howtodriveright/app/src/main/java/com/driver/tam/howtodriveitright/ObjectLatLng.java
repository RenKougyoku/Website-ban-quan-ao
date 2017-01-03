package com.driver.tam.howtodriveitright;

public class ObjectLatLng {

    private double mLat;
    private double mLng;

    public ObjectLatLng(double mLat, double mLng) {
        super();
        this.setmLat(mLat);
        this.setmLng(mLng);
    }

    //@@@
    public double getmLat() {
        return mLat;
    }

    public void setmLat(double mLat) {
        this.mLat = mLat;
    }

    //@@@
    public double getmLng() {
        return mLng;
    }

    public void setmLng(double mLng) {
        this.mLng = mLng;
    }
}
