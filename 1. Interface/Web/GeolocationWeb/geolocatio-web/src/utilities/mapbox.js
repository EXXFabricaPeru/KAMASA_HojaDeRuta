import mapboxgl from "mapbox-gl";
import MapboxDraw from "@mapbox/mapbox-gl-draw";

export class MapboxHelper {
    static MakeMapBuilder(containerReference) {
        const builder = new MapBoxBuilder();
        return builder.makeDefaultMapBox(containerReference);
    }
}

export class MapBoxBuilder {
    #mapbox;
    drawControls = [];

    makeDefaultMapBox =
        (containerReference) => {
            this.#mapbox = new mapboxgl.Map({
                container: containerReference.current,
                attributionControl: false,
                style: 'mapbox://styles/mapbox/streets-v11',
                zoom: 15
            });

            this.#mapbox.addControl(new mapboxgl.NavigationControl(), 'top-right');
            this.#mapbox.addControl(new mapboxgl.ScaleControl({
                maxWidth: 80,
                unit: 'metric'
            }));
            return this;
        }

    appendPolygonDrawControl =
        () => {
            const draw = new MapboxDraw({
                displayControlsDefault: false,
                controls: {
                    polygon: true,
                    trash: true
                },
                defaultMode: 'draw_polygon'
            });
            this.drawControls.push(draw);
            this.#mapbox.addControl(draw);
            return this;
        }

    appendFunction =
        (functionName, onFunction) => {
            this.#mapbox.on(functionName, () => {
                onFunction(this.#mapbox);
            });
            return this;
        }

    mapboxReference =
        () => this.#mapbox;

    setCenter =
        (longitude, latitude) => {
            this.#mapbox.flyTo({
                center: [longitude, latitude]
            });
        }
}