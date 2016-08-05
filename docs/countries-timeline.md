<div id="container" style="position: relative; width: 100%; height: 300px;"></div>
<script>
/*
var bombMap = new Datamap({
    element: document.getElementById('container'),
    scope: 'world',
    geographyConfig: {
        popupOnHover: false,
        highlightOnHover: false
    },
    fills: {
        'USA': '#1f77b4',
        'RUS': '#9467bd',
        'PRK': '#ff7f0e',
        'PRC': '#2ca02c',
        'IND': '#e377c2',
        'GBR': '#8c564b',
        'FRA': '#d62728',
        'PAK': '#7f7f7f',
        defaultFill: '#EDDC4E'
    },
    data: {
        'RUS': {fillKey: 'RUS'},
        'PRK': {fillKey: 'PRK'},
        'PRC': {fillKey: 'PRC'},
        'IND': {fillKey: 'IND'},
        'GBR': {fillKey: 'GBR'},
        'FRA': {fillKey: 'FRA'},
        'PAK': {fillKey: 'PAK'},
        'USA': {fillKey: 'USA'}
    }
});

     function bombs() { return [{
        name: 'Joe 4',
        radius: Math.random() * 100,
        yield: 400,
        country: 'USSR',
        fillKey: 'RUS',
        significance: 'First fusion weapon test by the USSR (not "staged")',
        date: '1953-08-12',
        latitude: 50.07,
        longitude: 78.43
      },{
        name: 'RDS-37',
        radius: Math.random() * 100,
        yield: 1600,
        country: 'USSR',
        fillKey: 'RUS',
        significance: 'First "staged" thermonuclear weapon test by the USSR (deployable)',
        date: '1955-11-22',
        latitude: 50.07,
        longitude: 78.43

      },{
        name: 'Tsar Bomba',
        radius: Math.random() * 100,
        yield: 50000,
        country: 'USSR',
        fillKey: 'RUS',
        significance: 'Largest thermonuclear weapon ever tested—scaled down from its initial 100 Mt design by 50%',
        date: '1961-10-31',
        latitude: 73.482,
        longitude: 54.5854
      }
    ]; }

//draw bubbles for bombs
bombMap.bubbles(bombs(), {
    popupTemplate: function (geo, data) {
            return ['<div class="hoverinfo">' +  data.name,
            '<br/>Payload: ' +  data.yield + ' kilotons',
            '<br/>Country: ' +  data.country + '',
            '<br/>Date: ' +  data.date + '',
            '</div>'].join('');
    }
});


setInterval(function() {
	bombMap.bubbles(bombs(), {
	    popupTemplate: function (geo, data) {
	            return ['<div class="hoverinfo">' +  data.name,
	            '<br/>Payload: ' +  data.yield + ' kilotons',
	            '<br/>Country: ' +  data.country + '',
	            '<br/>Date: ' +  data.date + '',
	            '</div>'].join('');
	    }
	});
}, 5000)
*/
</script>
