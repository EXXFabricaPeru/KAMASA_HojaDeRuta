import mapboxgl from 'mapbox-gl';
import * as turf from "@turf/turf";
import {useEffect, useRef} from "react";
import axios from "axios";
import {DOMElement, MapboxHelper} from "../utilities";

mapboxgl.accessToken = 'pk.eyJ1IjoiZGFya21ha3UiLCJhIjoiY2tvYjJobHZ5MDcxaDJ2b2syb3g1ajhjNiJ9.tqMZPZvYv3rzh0fLs1y7Hg';

function SectorLocalization() {
    const sectorId = 'sector-box';

    const calculationBoxStyle = {
        width: '20%',
        position: 'absolute',
        bottom: '40px',
        left: '10px',
        backgroundColor: 'rgba(255, 255, 255, 0.9)',
        padding: '15px',
        textAlign: 'center',
        top: '10px'
    };

    const makeSectorBox =
        () => {
            const answer = document.getElementById(sectorId);
            const header = document.createElement('b');
            header.appendChild(document.createTextNode('Sector'));
            const sectorName = DOMElement.makeElementWith('div', {class: 'div-container'});
            sectorName.appendChild(DOMElement.makeParagraph(null, 'paragraph-20', 'Nombre'));
            sectorName.appendChild(DOMElement.makeTextInput('sector-name', 'text-box-80'));
            const sectorArea = DOMElement.makeElementWith('div', {class: 'div-container'});
            sectorArea.appendChild(DOMElement.makeParagraph(null, 'paragraph-20', 'Area'));
            sectorArea.appendChild(DOMElement.makeParagraph('sector-area', 'paragraph-80', '0 km2'));
            const sectorButton = DOMElement.makeElementWith('div', {class: 'div-container'});
            const button = DOMElement.makeButton('append-button', 'apply-button', 'Aplicar');
            button.disabled = true;
            sectorButton.appendChild(button);
            answer.appendChild(header);
            answer.appendChild(sectorName);
            answer.appendChild(sectorArea);
            answer.appendChild(sectorButton)
        }

    const mapContainer = useRef();

    useEffect(() => {
        makeSectorBox();
        const builder = MapboxHelper.MakeMapBuilder(mapContainer)
            .appendPolygonDrawControl()
            .appendFunction('draw.create', updateArea)
            .appendFunction('draw.delete', updateArea)
            .appendFunction('draw.update', updateArea);

        function updateArea() {
            const data = builder.drawControls[0].getAll();
            const answer = document.getElementById('sector-area');
            DOMElement.removeAllChild(answer);
            if (data.features.length > 0) {
                document.getElementById('append-button').disabled = false;
                const area = turf.area(data);
                const rounded_area = Math.round(area * 100) / 100;
                answer.appendChild(document.createTextNode(`${rounded_area} km2`))
            } else {
                document.getElementById('append-button').disabled = true;
                answer.appendChild(document.createTextNode('0 km2'))
            }
        }

        axios.get('http://localhost:5000/general/central-coordinate')
            .then((response) => {
                const longitude = response.data.find((item) => item.code === 'PNT_LG');
                const latitude = response.data.find((item) => item.code === 'PNT_LT');
                builder.setCenter(longitude.value, latitude.value);
            });

        return () => builder.mapboxReference().remove();
    });

    return (
        <div>
            <div className='map-container' ref={mapContainer}/>
            <div className="calculation-box">
                <p>Draw a polygon using the draw tools.</p>
                <div id={sectorId} style={calculationBoxStyle}/>
            </div>
        </div>)
}

export default SectorLocalization;