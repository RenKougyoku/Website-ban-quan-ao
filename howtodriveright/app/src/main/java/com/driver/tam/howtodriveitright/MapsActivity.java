package com.driver.tam.howtodriveitright;


import android.Manifest;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.location.Criteria;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.FragmentActivity;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnLongClickListener;
import android.view.ViewGroup;
import android.view.Window;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.AnimationUtils;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout.LayoutParams;
import android.widget.TextView;
import android.widget.Toast;
import com.driver.tam.howtodriveitright.ConstLinkApi;
import com.driver.tam.howtodriveitright.MainActivity;
import com.driver.tam.howtodriveitright.ObjectLatLng;
import com.driver.tam.howtodriveitright.R;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.android.gms.maps.CameraUpdate;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.GoogleMap.InfoWindowAdapter;
import com.google.android.gms.maps.GoogleMap.OnMapClickListener;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.BitmapDescriptorFactory;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.koushikdutta.urlimageviewhelper.UrlImageViewHelper;
import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.AsyncHttpResponseHandler;

import org.json.JSONException;

import java.util.ArrayList;

public class MapsActivity extends FragmentActivity implements
        LocationListener {

    private ArrayList<ObjectLocation> arrLocation = new ArrayList<ObjectLocation>();
    private JSonParserFoursquare JSonFoursquare;
    private ConstLinkApi linkFoursquareApi;
    private GoogleMap mMap;
    private ListView listLocation;
    private LocationAdapter adapter;
    private ArrayAdapter<String> adapterComplete;
    private ArrayList<ObjectLocation> arrayLoc = new ArrayList<ObjectLocation>();
    private AutoCompleteTextView autoComplete;
    ObjectLocation[] objectsLocation = new ObjectLocation[]{};
    private String[] nameVenues;
    private LocationManager locationManager;
    private String provider;
    private boolean enabledGPS, enabledWIFI;
    private Animation out_left, out_right, in_left, in_right;
    private View infoWindow;
    private myLocationListener mLocationListener;
    private LocationManager service;
    private ConnectivityManager conMan;
    // private State enableWifi;
    private ImageView bgBack;
    Button btnHuy;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.activity_maps);

        checkWifiAndGPS();
        checkServiceGoogle();

        btnHuy = (Button)findViewById(R.id.btnHuy);
        this.btnHuy.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });

        infoWindow = getLayoutInflater().inflate(R.layout.info_window_layout,
                null);
        linkFoursquareApi = new ConstLinkApi();
        mMap = ((SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map)).getMapAsync();
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return;
        }
        mMap.setMyLocationEnabled(false);
        mMap.setInfoWindowAdapter(new CustomInfoAdapter());
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);
        Criteria criteria = new Criteria();
        provider = locationManager.getBestProvider(criteria, false);
        Location location = locationManager.getLastKnownLocation(provider);
        mLocationListener = new myLocationListener();

        if (location == null && isOnline()) {
            Location location_wifi = locationManager
                    .getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
            mLocationListener.onLocationChanged(location_wifi);
        } else if (location == null && enabledGPS) {
            locationManager.requestLocationUpdates(
                    LocationManager.GPS_PROVIDER, 60000, 10, this);
            Location location_gps = locationManager
                    .getLastKnownLocation(LocationManager.GPS_PROVIDER);
            if (location_gps != null) {
                focusMyLocation(location_gps);
            }

        } else {

            Toast.makeText(getBaseContext(),
                    "Đợi chút", Toast.LENGTH_SHORT)
                    .show();
            // mLocationListener.onLocationChanged(location);
        }

        bgBack = (ImageView) findViewById(R.id.imgBack);
        bgBack.setVisibility(View.VISIBLE);
        bgBack.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                animationInvisible(1);
                fullScreenMap(1);
            }
        });

        mMap.setOnMapClickListener(new OnMapClickListener() {

            @Override
            public void onMapClick(LatLng arg0) {
                // TODO Auto-generated method stub
                animationInvisible(0);
                bgBack.setVisibility(View.VISIBLE);
            }
        });
    }

    public void checkServiceGoogle() {
        int result = GooglePlayServicesUtil
                .isGooglePlayServicesAvailable(getApplicationContext());
        Log.i("xxx", "Result int value::" + result);
        switch (result) {
            case ConnectionResult.SUCCESS:
                Log.i("xxx", "RESULT:: SUCCESS");
                break;

            case ConnectionResult.DEVELOPER_ERROR:
                Log.i("xxx", "RESULT:: DEVELOPER_ERROR");
                break;

            case ConnectionResult.INTERNAL_ERROR:
                Log.i("xxx", "RESULT:: INTERNAL_ERROR");
                break;

            case ConnectionResult.INVALID_ACCOUNT:
                Log.i("xxx", "RESULT:: INVALID_ACCOUNT");
                break;

            case ConnectionResult.NETWORK_ERROR:
                Log.i("xxx", "RESULT:: NETWORK_ERROR");
                break;

            case ConnectionResult.RESOLUTION_REQUIRED:
                Log.i("xxx", "RESULT:: RESOLUTION_REQUIRED");
                break;

            case ConnectionResult.SERVICE_DISABLED:
                Log.i("xxx", "RESULT:: SERVICE_DISABLED");
                break;

            case ConnectionResult.SERVICE_INVALID:
                Log.i("xxx", "RESULT:: SERVICE_INVALID");
                break;

            case ConnectionResult.SERVICE_MISSING:
                Log.i("xxx", "RESULT:: SERVICE_MISSING");
                break;

            case ConnectionResult.SERVICE_VERSION_UPDATE_REQUIRED:
                Log.i("xxx", "RESULT:: SERVICE_VERSION_UPDATE_REQUIRED");
                break;

            case ConnectionResult.SIGN_IN_REQUIRED:
                Log.i("xxx", "RESULT:: SIGN_IN_REQUIRED");
                break;

            default:
                break;
        }
    }

    public void checkWifiAndGPS() {

        conMan = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        // enableWifi =
        // conMan.getNetworkInfo(ConnectivityManager.TYPE_WIFI).getState();

        if (isOnline()) {
            Toast.makeText(getApplicationContext(), "Đã có mạng ", Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(getApplicationContext(), "Chưa có mạng ",
                    Toast.LENGTH_SHORT).show();
        }

        service = (LocationManager) getSystemService(LOCATION_SERVICE);
        enabledGPS = service.isProviderEnabled(LocationManager.GPS_PROVIDER);
        if (!enabledGPS) {
            Intent intent = new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS);
            startActivityForResult(intent, RESULT_CANCELED);
            Toast.makeText(getApplicationContext(), "Không có GPS",
                    Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(getApplicationContext(), "Có GPS",
                    Toast.LENGTH_SHORT).show();
        }
    }

    public boolean isOnline() {
        ConnectivityManager cm = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo netInfo = cm.getActiveNetworkInfo();
        if (netInfo != null && netInfo.isConnectedOrConnecting()) {
            return true;
        }
        return false;
    }

    @Override
    protected void onActivityResult(int arg0, int result, Intent arg2) {
        // TODO Auto-generated method stub
        super.onActivityResult(arg0, result, arg2);
        if (result == RESULT_CANCELED) {
            if (!enabledGPS) {
                Toast.makeText(getApplicationContext(),
                        getResources().getString(R.string.gps),
                        Toast.LENGTH_SHORT).show();
            }
        }
    }

    public void focusMyLocation(Location location) {

       LatLng userLocation = new LatLng(location.getLatitude(),location.getLongitude());
        //LatLng userLocation = new LatLng(10.7826951, 106.6954774);
        CameraUpdate center = CameraUpdateFactory.newLatLng(new LatLng(
                userLocation.latitude, userLocation.longitude));
        CameraUpdate zoom = CameraUpdateFactory.zoomTo(15);

        mMap.moveCamera(center);
        mMap.animateCamera(zoom);
        mMap.addMarker(new MarkerOptions()
                .position(userLocation)
                .icon(BitmapDescriptorFactory
                        .fromResource(R.drawable.mylocation))
                .snippet("Chỗ tui đứng").title("Tui nè"));

        getDataFromMyLocation(userLocation);

    }

    public void getDataFromMyLocation(LatLng userLocation) {
        JSonFoursquare = new JSonParserFoursquare();
        AsyncHttpClient client = new AsyncHttpClient();
        client.get(linkFoursquareApi.getApiLocation(new ObjectLatLng(
                        userLocation.latitude, userLocation.longitude)),
                new AsyncHttpResponseHandler() {

                    @Override
                    public void onSuccess(String response) {
                        try {
                            arrLocation = JSonFoursquare
                                    .getDataFourSquare(response);
                            nameVenues = new String[50];
                            for (int i = 0; i < arrLocation.size(); i++) {
                                nameVenues[i] = arrLocation.get(i)
                                        .getNameLocation();
                            }

                            fillDataToListView();
                            fillAutoCompleteText();

                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }

                    @Override
                    protected void sendFailureMessage(Throwable arg0,
                                                      String arg1) {
                        super.sendFailureMessage(arg0, arg1);
                        Log.e("VCC", "sendFailureMessage(arg0, arg1)" + arg1);
                    }
                });
    }

    public int getIdDKM(String currentName) {
        int id = 0;
        for (int i = 0; i < nameVenues.length; i++) {
            if (nameVenues[i].equalsIgnoreCase(currentName)) {
                id = i;
            }
        }

        return id;
    }

    public void fillDataToListView() {
        listLocation = (ListView) findViewById(R.id.listViewLocation);
        if (arrLocation.size() != 0) {
            adapter = new LocationAdapter(getApplicationContext(),
                    R.layout.items_location, arrLocation);
            listLocation.setAdapter(adapter);
        }
    }

    public void fillAutoCompleteText() {
        autoComplete = (AutoCompleteTextView) findViewById(R.id.autoCompleteTextView);
        adapterComplete = new ArrayAdapter<String>(getApplicationContext(),
                R.layout.auto_itemtextview, nameVenues);
        autoComplete.setAdapter(adapterComplete);
        autoComplete.setThreshold(1);
    }

    public class LocationAdapter extends ArrayAdapter<ObjectLocation> {

        Context context;
        int layoutResourceId;
        ArrayList<ObjectLocation> objects = null;

        public LocationAdapter(Context context, int layoutResourceId,
                               ArrayList<ObjectLocation> objects) {
            super(context, layoutResourceId, objects);
            this.context = context;
            this.layoutResourceId = layoutResourceId;
            this.objects = objects;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {

            View row = convertView;
            LocationHolder objLoc = null;

            if (row == null) {
                LayoutInflater inflater = getLayoutInflater();
                row = inflater.inflate(layoutResourceId, parent, false);

                objLoc = new LocationHolder();
                objLoc.txtName = (TextView) row.findViewById(R.id.txtName);
                objLoc.txtAddress = (TextView) row
                        .findViewById(R.id.txtAddress);

                row.setTag(objLoc);
            } else {
                objLoc = (LocationHolder) row.getTag();
            }

            ObjectLocation obj = objects.get(position);

            objLoc.txtName.setText(obj.getNameLocation());
            objLoc.txtAddress.setText(obj.getAddVenues());

            row.setId(position);

            row.setOnLongClickListener(new OnLongClickListener() {

                @Override
                public boolean onLongClick(View v) {
                    // TODO Auto-generated method stub
                    animationInvisible(0);
                    bgBack.setVisibility(View.VISIBLE);
                    return true;
                }
            });

            row.setOnClickListener(new OnClickListener() {

                @Override
                public void onClick(View v) {
                    // TODO Auto-generated method stub
                    autoComplete.setText(arrLocation.get(v.getId())
                            .getNameLocation());
                }
            });

            return row;
        }

        class LocationHolder {
            TextView txtName;
            TextView txtAddress;
        }
    }

    public void animationInvisible(int type) {

        out_left = AnimationUtils.loadAnimation(this, R.anim.out_to_left);
        out_right = AnimationUtils.loadAnimation(this, R.anim.out_to_right);
        in_left = AnimationUtils.loadAnimation(this, R.anim.in_from_left);
        in_right = AnimationUtils.loadAnimation(this, R.anim.in_from_right);
        out_right.setAnimationListener(listener_anim);

        if (arrLocation.size() != 0) {
            if (type == 0) {
                listLocation.startAnimation(out_left);
                listLocation.setVisibility(View.INVISIBLE);
                autoComplete.startAnimation(out_right);
                autoComplete.setVisibility(View.INVISIBLE);
            } else {
                listLocation.startAnimation(in_left);
                listLocation.setVisibility(View.VISIBLE);
                autoComplete.startAnimation(in_right);
                autoComplete.setVisibility(View.VISIBLE);

                bgBack.setVisibility(View.INVISIBLE);
            }
        }
    }

    AnimationListener listener_anim = new AnimationListener() {

        @Override
        public void onAnimationStart(Animation animation) {
            // TODO Auto-generated method stub
        }

        @Override
        public void onAnimationRepeat(Animation animation) {
            // TODO Auto-generated method stub
        }

        @Override
        public void onAnimationEnd(Animation animation) {
            // TODO Auto-generated method stub
            fullScreenMap(0);
        }
    };

    public void fullScreenMap(int i) {
        LinearLayout mapLinear = (LinearLayout) findViewById(R.id.linearMap);
        if (i == 0) {
            LinearLayout.LayoutParams mapFrame = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.MATCH_PARENT);
            mapLinear.setLayoutParams(mapFrame);
            if (arrLocation.size() != 0) {
                addMarkertoMap(arrLocation, mMap, 5);
            }
        } else {
            LinearLayout.LayoutParams mapFrame = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT, 180);
            mapLinear.setLayoutParams(mapFrame);
        }
    }

    private void addMarkertoMap(ArrayList<ObjectLocation> arrayLoc,
                                GoogleMap mapGoogle, int limit) {
        ;

        for (int i = 0; i < limit; i++) {
            LatLng MarkerPos = new LatLng(arrayLoc.get(i).getmObjLatLng()
                    .getmLat(), arrayLoc.get(i).getmObjLatLng().getmLng());
            Marker markerCustom = mapGoogle.addMarker(new MarkerOptions()
                    .icon(BitmapDescriptorFactory
                            .fromResource(R.drawable.pin_map_1))
                    .position(MarkerPos)
                    .snippet(arrayLoc.get(i).getAddVenues())
                    .title(arrayLoc.get(i).getNameLocation()));
        }
    }

    @Override
    protected void onPause() {
        // TODO Auto-generated method stub
        super.onPause();
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return;
        }
        locationManager.removeUpdates(mLocationListener);
    }

    @Override
    protected void onResume() {
        // TODO Auto-generated method stub
        super.onResume();
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return;
        }
        locationManager.requestLocationUpdates(provider, 500, 50,
                mLocationListener);
    }


    public class myLocationListener implements LocationListener {

        @Override
        public void onLocationChanged(Location location) {
            // TODO Auto-generated method stub
            // mMap.clear();
            if (location != null)
                focusMyLocation(location);
            else
                Toast.makeText(getBaseContext(), "Đang lấy vị trí",
                        Toast.LENGTH_SHORT).show();

        }

        @Override
        public void onProviderDisabled(String provider) {
            // TODO Auto-generated method stub

        }

        @Override
        public void onProviderEnabled(String provider) {
            // TODO Auto-generated method stub

        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {
            // TODO Auto-generated method stub

        }

    }

    /**
     * Custom InfoWindow
     */
    public void displayView(Marker mark) {

        ((TextView) infoWindow.findViewById(R.id.nameVenues)).setText(mark
                .getTitle());
        UrlImageViewHelper.setUrlDrawable(((ImageView) infoWindow
                        .findViewById(R.id.avatar)),
                arrLocation.get(getIdDKM(mark.getTitle())).getLinkIcon(),
                R.drawable.loading);
        ((TextView) infoWindow.findViewById(R.id.addressVenues)).setText(mark
                .getSnippet());

    }

    class CustomInfoAdapter implements InfoWindowAdapter {

        @Override
        public View getInfoContents(Marker mark) {
            // TODO Auto-generated method stub
            return null;
        }

        @Override
        public View getInfoWindow(Marker mark) {
            // TODO Auto-generated method stub
            Toast.makeText(getApplicationContext(), mark.getId(),
                    Toast.LENGTH_SHORT).show();
            displayView(mark);
            return infoWindow;
        }
    }

    /**
     * Convert a view to bitmap
     */
    public static Bitmap createDrawableFromView(Context context, View view) {
        DisplayMetrics displayMetrics = new DisplayMetrics();
        ((Activity) context).getWindowManager().getDefaultDisplay()
                .getMetrics(displayMetrics);
        view.setLayoutParams(new LayoutParams(LayoutParams.WRAP_CONTENT,
                LayoutParams.WRAP_CONTENT));
        view.measure(displayMetrics.widthPixels, displayMetrics.heightPixels);
        view.layout(0, 0, displayMetrics.widthPixels,
                displayMetrics.heightPixels);
        view.buildDrawingCache();
        Bitmap bitmap = Bitmap.createBitmap(view.getMeasuredWidth(),
                view.getMeasuredHeight(), Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        view.draw(canvas);

        return bitmap;
    }

    @Override
    public void onLocationChanged(Location location) {
        // TODO Auto-generated method stub

    }

    @Override
    public void onProviderDisabled(String provider) {
        // TODO Auto-generated method stub

    }

    @Override
    public void onProviderEnabled(String provider) {
        // TODO Auto-generated method stub

    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {
        // TODO Auto-generated method stub

    }
}
