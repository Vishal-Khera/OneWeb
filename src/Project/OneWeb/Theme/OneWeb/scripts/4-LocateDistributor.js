$("#locatedistributorbutton").click(function() {
  Locatesearch();
});



$("#locatedistributortextbox").on('keypress',function(e) {
    if(e.which == 13) {
         Locatesearch();
    }
});
function PopulateDataLocate(Events) {
	var lat = [];
	var longi=[];
	var arr=[];
    for (var i = 0; i < Events.length; i++) {
        debugger;
        document.getElementById("displaynone").style.display = "block";
        document.getElementById("map").style.height = "500px";
        var obj = Events[i];
		$("#destination_lat").remove();
		$("#destination_long").remove();
        var xx = document.createElement("INPUT");
        xx.setAttribute("type", "hidden");
        xx.setAttribute("value",  obj.Latitude);
        xx.setAttribute("id", "destination_lat");
        document.body.appendChild(xx);
        var xy = document.createElement("INPUT");
        xy.setAttribute("type", "hidden");
        xy.setAttribute("value", obj.Longitude);
        xy.setAttribute("id", "destination_long");
        document.body.appendChild(xy);
		$('#locatedata').empty();
		$('#locatedata').append('<div class="component testimonial-card"><div class="component-content"><div class="cards"><div class="card"><div class="card-header"><div class="card-image"><img src="'+obj.Image.Url+'"></div> <div class="card-heading"><h2 class="name">'+obj.Title+'</h2><h3 class="designation">'+obj.Subtext+'</h3></div></div><div class="card-body"><div class="card-paragraph"> <ul> <li><a href="tel:'+obj.Mobile+'"><i class="fas fa-phone fa-fw"></i>Phone: '+obj.Mobile+' </a></li> <li><a href="mailto:'+obj.Email+'"><i class="fas fa-envelope fa-fw"></i>'+obj.Email+'</a></li></ul></div></div></div></div></div></div></div>');
		lat.push(obj.Latitude);
		longi.push(obj.Longitude);
		arr.push(lat,longi);
		
        initialize(obj.Latitude, obj.Longitude);
        
    }
}

function Locatesearch()
{
	  var geocoder = new google.maps.Geocoder();
    var keyword = document.getElementById('locatedistributortextbox').value;
    var keys = "";
	var ss= stringContainsNumber(keyword);
    if (stringContainsNumber(keyword)) {
        var parameters = {
            "Keyword": keyword
        };
		 fetch("https://maps.googleapis.com/maps/api/geocode/json?address="+keyword+'&key=AIzaSyAIHG5nPId68uwnbUIMxqZbsPMxWGtwkOI')
    .then(response => response.json())
    .then(data => {
      const latitude = data.results.geometry.location.lat;
      const longitude = data.results.geometry.location.lng;
	})
	
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: "/oneweb/LocateDistributor/GetLocateDistributor",
            data: JSON.stringify({
                'parameters': parameters
            }),
            cache: false,
            async: false,
            dataType: 'json',
            type: "POST",
            success: function(response) {
                if (response != undefined) {
                                                if (owCommon.isValidJson(response.Events)) {
													if(response.Events.length>0){
													document.getElementById("noresult").style.display = "none";
                                                    PopulateDataLocate(response.Events);
													}
													else{
														document.getElementById("noresult").style.display = "block";
															$('#locatedata').empty();            
	$('#locatedistance').empty();
													}
													
                                                }
												else{
													 document.getElementById("noresult").style.display = "block";
													 	$('#locatedata').empty();            
	$('#locatedistance').empty();
													
												}
                                            }
            },
            error: function(xhr) {
                console.log(xhr);
            }
        });
    } else {
		var parameters1  = keyword;        
		fetch("https://maps.googleapis.com/maps/api/geocode/json?address="+keyword+'&key=AIzaSyAIHG5nPId68uwnbUIMxqZbsPMxWGtwkOI')
    .then(response => response.json())
    .then(data => {
      const latitude = data.results[0].geometry.location.lat;
      const longitude = data.results[0].geometry.location.lng;	
	  var add= data.results[0].formatted_address.trim();	
	  var  value=add.split(" ");
	  count=value.length;
	  var result=value[count-1];
	  var Country=result;
	  var parameters = {
            "Keyword": Country
        };	
		 var latitude1 = latitude;
                var longitude1 = longitude;				
				$("#current_lat").remove();
				$("#current_long").remove();
                var x = document.createElement("INPUT");
                x.setAttribute("type", "hidden");
                x.setAttribute("value", latitude1);
                x.setAttribute("id", "current_lat");
				document.body.appendChild(x);
                var y = document.createElement("INPUT");
                y.setAttribute("type", "hidden");
                y.setAttribute("value", longitude1);
                y.setAttribute("id", "current_long");
                document.body.appendChild(y);
	  if(result=="Germany"){
		  
		  Country=value[0];
		  var latlng = new google.maps.LatLng(latitude, longitude);
		  geocoder.geocode({
                    'latLng': latlng
                }, function(results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            for (j = 0; j < results[0].address_components.length; j++) {
                                if (results[0].address_components[j].types[0] == 'postal_code')
                                    if (Number.isInteger(results[0].address_components[j].short_name)) {
                                        keys = results[0].address_components[j].short_name;
                                    }
                                else {
                                    keys = results[0].address_components[j].short_name;
                                    var parameters = {
                                        "Keyword": results[0].address_components[j].short_name
                                    };
                                    $.ajax({
                                        contentType: 'application/json; charset=utf-8',
                                        url: "/oneweb/LocateDistributor/GetLocateDistributor",
                                        data: JSON.stringify({
                                            'parameters': parameters
                                        }),
                                        cache: false,
                                        async: false,
                                        dataType: 'json',
                                        type: "POST",
                                        success: function(response) {
                                             if (response != undefined) {
                                                if (owCommon.isValidJson(response.Events)) {
													if(response.Events.length>0){
													document.getElementById("noresult").style.display = "none";
                                                    PopulateDataLocate(response.Events);
													}
													else{
														document.getElementById("noresult").style.display = "block";
															$('#locatedata').empty();            
	$('#locatedistance').empty();
													}
													
                                                }
												else{
													 document.getElementById("noresult").style.display = "block";
													 	$('#locatedata').empty();            
	$('#locatedistance').empty();
													
												}
                                            }
                                        },
                                        error: function(xhr) {
                                            console.log(xhr);
                                        }
                                    });

                                }
                            }
                        }
                    } else {

                    }
                });
	  }else{
	 
  
               
				var latlng = new google.maps.LatLng(latitude, longitude);
				$.ajax({
                                        contentType: 'application/json; charset=utf-8',
                                        url: "/oneweb/LocateDistributor/GetLocateDistributor",
                                        data: JSON.stringify({
                                            'parameters': parameters
                                        }),
                                        cache: false,
                                        async: false,
                                        dataType: 'json',
                                        type: "POST",
                                        success: function(response) {
                                            if (response != undefined) {
                                                if (owCommon.isValidJson(response.Events)) {
													if(response.Events.length>0){
													document.getElementById("noresult").style.display = "none";
                                                    PopulateDataLocate(response.Events);
													}
													else{
														document.getElementById("noresult").style.display = "block";
															$('#locatedata').empty();            
	$('#locatedistance').empty();
													}
													
                                                }
												else{
													 document.getElementById("noresult").style.display = "block";
													 	$('#locatedata').empty();            
	$('#locatedistance').empty();
													
												}
                                            }
                                        },
                                        error: function(xhr) {
                                            console.log(xhr);
                                        }
                                    });
					  
	  }

                    
     
				             
        })   
    }
}


function initialize(lat, longi) {
    geocoder = new google.maps.Geocoder();
	debugger;
    var latlng = new google.maps.LatLng(lat, longi);
    var mapOptions = {
        zoom: 5,
        center: latlng
    }
    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    TestMarker(lat, longi);
    google.maps.event.addListener(map, 'click', function(event) {
        //alert(event.latLng);          
        geocoder.geocode({
            'latLng': event.latLng
        }, function(results, status) {
            if (status ==
                google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    alert(results[1].formatted_address);
                } else {
                    alert('No results found');
                }
            } else {
                alert('Geocoder failed due to: ' + status);
            }
        });
    });
}
function addMarker(location) {
    marker = new google.maps.Marker({
        position: location,
		title: 'new marker',
        map: map
    });
}
function initMap() {
    const directionsService = new google.maps.DirectionsService();
    const directionsRenderer = new google.maps.DirectionsRenderer();
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 7,
       
    });
distance();
    directionsRenderer.setMap(map);
    calculateAndDisplayRoute(directionsService, directionsRenderer);

}


    function distance()
    {
	var current_lat= document.getElementById('current_lat').value;
	var current_long= document.getElementById('current_long').value;
	var destination_lat= document.getElementById('destination_lat').value;
	var destination_long= document.getElementById('destination_long').value;
   
        // The math module contains a function
        // named toRadians which converts from
        // degrees to radians.
        lon1 =  current_long * Math.PI / 180;
        lon2 = destination_long * Math.PI / 180;
        lat1 = current_lat * Math.PI / 180;
        lat2 = destination_lat * Math.PI / 180;
   
        // Haversine formula
        let dlon = lon2 - lon1;
        let dlat = lat2 - lat1;
        let a = Math.pow(Math.sin(dlat / 2), 2)
                 + Math.cos(lat1) * Math.cos(lat2)
                 * Math.pow(Math.sin(dlon / 2),2);               
        let c = 2 * Math.asin(Math.sqrt(a));
        // Radius of earth in kilometers. Use 3956
        // for miles 6371
        let r = 3956;   
		let dist=c * r;
        // calculate the result
		 $('#locatedistance').text('Distance : '+Math.ceil(dist)+' Km');
    }

function calculateAndDisplayRoute(directionsService, directionsRenderer) {
    debugger;
	var current_lat= document.getElementById('current_lat').value;
	var current_long= document.getElementById('current_long').value;
	var destination_lat= document.getElementById('destination_lat').value;
	var destination_long= document.getElementById('destination_long').value;
    var start = current_lat+ ","+current_long;
    var end = destination_lat+","+destination_long ;

    directionsService
        .route({
            origin: start,
            destination: end,
            travelMode: google.maps.TravelMode.DRIVING,
        })
        .then((response) => {
            directionsRenderer.setDirections(response);
        })
        .catch((e) => window.alert("Directions request failed due to " + status));
}

 $(document).on('click', '.clear-textLocate', function (e) {	
            clearText = true;
            callEvent12(this)
	document.getElementById("displaynone").style.display = "none";
	document.getElementById("noresult").style.display = "none";
	$('#locatedata').empty();            
	$('#locatedistance').empty();
        });
 $(document).on('keyup', '#locatedistributortextbox', function (event) {
		 if($(this).val().length > 0){
                            $('.clear-text').addClass('d-block');							
                        }
                        if($(this).val().length == 0){
                            $('.clear-text').removeClass('d-block');
							 document.getElementById("displaynone").style.display = "none";
							 document.getElementById("noresult").style.display = "none";
							 $('#locatedata').empty();
							 $('#locatedistance').empty();
                        }
 });
  function callEvent12(key){
            $('#locatedistributortextbox').val('');
            $('#locatedistributortextbox').focus();           
            $(key).removeClass('d-block');
           
           }
// Testing the addMarker function
function TestMarker(lat, longi) {
    CentralPark = new google.maps.LatLng(lat, longi);
    addMarker(CentralPark);
}
$(document).ready(function () {
	if($('#noresult').length > 0){
		document.getElementById("noresult").style.display = "none";
	}
 var script = document.createElement('script');
script.type = 'text/javascript';

script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAIHG5nPId68uwnbUIMxqZbsPMxWGtwkOI&amp;callback=initMap';
document.body.appendChild(script);   

})

function stringContainsNumber(_string) {
    return /\d/.test(_string);
  }