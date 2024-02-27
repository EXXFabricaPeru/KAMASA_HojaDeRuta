function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    let regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(window.location.href);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadRoutes(map) {
    const key = getParameterByName('key');
    fetch(`http://192.168.10.21:9095/api/map/${key}`, {
        mode: 'cors',
        headers: new Headers({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
        }),
    })
        .then(response => response.json())
        .then(data => insertRoutes(map, data))
        .then(data => {
            let table = document.getElementById('routes-table');
            for (let item of data) {
                let row = table.insertRow();
                let routeCell = row.insertCell();
                let colorCell = row.insertCell();
                let weightCell = row.insertCell();
                let colorSource = document.createElement('p');
                colorSource.setAttribute('style', `color: ${item.color}; text-align: center`);
                colorSource.appendChild(document.createTextNode('--------'));
                routeCell.appendChild(document.createTextNode(item.routeId));
                colorCell.appendChild(colorSource);
                weightCell.appendChild(document.createTextNode(`${item.weight.toFixed(2)} kg.`));
            }
        });
}

async function insertRoutes(map, routes) {
    let leyend = [];
    for (let i = 0; i < routes.length; i++) {
        const route = routes[i];
        const geojson = await buildGeoJson(route);

        const sourceId = `route_${i}`;
        const layerId = `layer_${i}`;
        const color = randomColor();

        map.addSource(sourceId, {
            type: 'geojson',
            lineMetrics: true,
            data: geojson[1]
        });

        const markers = geojson[0];
        for (let i1 = 0; i1 < markers.length; i1++) {
            let marker = markers[i1];
            let popupHtml = `<p style="text-decoration: underline;">Ruta: ${route.id}</p>`;
			popupHtml += `<p class="no-margin-b">Cliente: ${marker.cliente}</p>`;
            popupHtml += `<p class="no-margin-b">Dirección: ${marker.address}</p>`;
			popupHtml += `<p class="no-margin-b">Peso: ${marker.weight} kg</p>`;
            //popupHtml += `<p class="no-margin-t-b">Coordenadas: ${marker.latitude} ; ${marker.longitude} </p>`;
            if (marker.arriveHour !== null)
                popupHtml += `<p class="no-margin-t-b">Hora llegada: ${parseHour(marker.arriveHour.toString())}</p>`;
			else
				 popupHtml += `<p class="no-margin-t-b">Hora partida: ${parseHour(marker.startHour.toString())} </p>`;
            popupHtml += `<p class="no-margin-t-b">Posición: ${i1 === 0 ? 'Punto de partida' : `Punto entrega nro. ${i1}`}</p>`;
            new mapboxgl.Marker()
                .setLngLat([marker.longitude, marker.latitude])
                .setPopup(new mapboxgl.Popup().setHTML(popupHtml))
                .addTo(map);
        }

        leyend.push({routeId: route.id, color, weight: route.totalWeight});

        map.addLayer({
            type: 'line',
            source: sourceId,
            id: layerId,
            paint: {
                'line-color': color,
                'line-width': 6
            },
            layout: {
                'line-cap': 'round',
                'line-join': 'round'
            }
        });
    }
    return leyend;
}

async function buildGeoJson(route) {
    let buildCoordinates = getCoordinates(route);
    let coordinates = [];
    for (let i = 1; i < buildCoordinates.length; i++) {
        const previousCoordinate = buildCoordinates[i - 1];
        const currentCoordinate = buildCoordinates[i];
        const urlCoordinates = `${previousCoordinate.longitude},${previousCoordinate.latitude};${currentCoordinate.longitude},${currentCoordinate.latitude}`;
        const url = `https://api.mapbox.com/directions/v5/mapbox/driving/${urlCoordinates}?geometries=geojson&access_token=sk.eyJ1Ijoic29yYXlhZGlzdHJpYnVjaW9uIiwiYSI6ImNrcHE5M2VpdDAzczQycHBvZDRrMGo2Y3cifQ.BKjDPTRle0YQ31ElrBv0jQ`;
        let response = await fetch(url);
        let responseObject = await response.json();
        coordinates.push(...responseObject.routes[0].geometry.coordinates);
    }

    return [buildCoordinates, {
        type: 'FeatureCollection',
        features: [
            {
                type: 'Feature',
                properties: {},
                geometry: {
                    coordinates,
                    type: 'LineString'
                }
            }
        ]
    }];
}

function randomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function getCoordinates(route) {
    let coordinates = [];
	var start=route.deliveryPointsRelated[0];
    coordinates.push({latitude: route.latitude, longitude: route.longitude, arriveHour: null, address: route.address,weight: route.totalWeight,cliente: "Soraya",startHour:start.startHour});
    for (let pointRelated of route.deliveryPointsRelated)
        coordinates.push({
            latitude: pointRelated.latitude,
            longitude: pointRelated.longitude,
            arriveHour: pointRelated.arriveHour,
            address: pointRelated.address,
			weight: pointRelated.weight,
			cliente: pointRelated.cliente,
			startHour:null
        });
    return coordinates;
}

function parseHour(hour) {
    const splitHour = hour.split('').map(Number)
    if (splitHour.length < 4)
        return `'0${splitHour[0]}:${splitHour[1]}${splitHour[2]}'`;
    return `'${splitHour[0]}${splitHour[1]}:${splitHour[2]}${splitHour[3]}'`;
}