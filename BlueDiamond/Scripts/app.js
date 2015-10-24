function ConvertToUTM(lat, lon, success) {
    var xy = new Array(2);
    // Compute the UTM zone.		
    var zone = Math.floor((lon + 180.0) / 6) + 1;
    zone = LatLonToUTMXY(DegToRad(lat), DegToRad(lon), zone, xy);
    success(zone, xy[0].toFixed(0), xy[1].toFixed(0));
}

function GeoLocate(onSuccess, onError) {
    if (!navigator.geolocation) {
        onError("Geolocation not supported in this browser");
        return;
    }

    try {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                var coords = position.coords || position.coordinate || position;
                onSuccess(coords);
            },
            function (error) {
                var msg;
                console.log(error.code);
                switch (error.code) {
                    case error.UNKNOWN_ERROR: msg = "Unable to find your location"; break;
                    case error.PERMISSION_DENINED: msg = "Permission denied in finding your location"; break;
                    case error.POSITION_UNAVAILABLE: msg = "Your location is currently unknown"; break;
                    case error.TIMEOUT: msg = "Attempt to find location took too long"; break;
                    case error.BREAK: msg = "Attempt to find location took too long 2"; break;
                    default: msg = "Other error: " + error.code;
                }

                var errorObj = new Object();
                errorObj.error = error;
                errorObj.message = msg;
                onError(errorObj);
            },
            { timeout: 10000, enableHighAccuracy: true });
    }
    catch (ex) {
        console.log(error);
        onError("Exception: " + ex);
    }
}