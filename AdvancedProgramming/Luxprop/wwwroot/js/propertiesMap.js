window.initPropertiesMap = (properties) => {

    console.log(">>> propertiesMap.js LOADED at", new Date().toISOString());

    console.log("Properties received:", properties);

    let map = L.map('map').setView([9.94, -84.09], 8);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18
    }).addTo(map);

    properties.forEach(p => {
        if (p.lat && p.lng) {

            const marker = L.marker([p.lat, p.lng]).addTo(map);

            marker.bindPopup(`
                <div style="font-size:14px;">
                    <b>${p.titulo}</b><br/>
                    ${p.tipo}<br/>
                    <b>$${p.precio.toLocaleString()}</b><br/><br/>
                    <a href="/properties/details/${p.id}" style="color:#0d505a;font-weight:bold;">
                        View details →
                    </a>
                </div>
            `);
        } else {
            console.warn("Property missing coordinates:", p);
        }
    });

};


window.initEditMap = (dotNetHelper, lat, lng) => {

    let map = L.map('map').setView([lat || 9.9281, lng || -84.0907], 10);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18
    }).addTo(map);

    let marker = null;

    // Si la propiedad YA tiene coordenadas → mostrar marcador
    if (lat && lng) {
        marker = L.marker([lat, lng]).addTo(map);
    }

    map.on('click', function (e) {
        const newLat = e.latlng.lat;
        const newLng = e.latlng.lng;

        if (marker) map.removeLayer(marker);
        marker = L.marker([newLat, newLng]).addTo(map);

        dotNetHelper.invokeMethodAsync("SetCoordinates", newLat, newLng);
    });
};

