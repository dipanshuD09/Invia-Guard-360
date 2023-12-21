var ctx = document.getElementById('myChart-2').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'pie',
    data: {
        labels: ['Buy the same Plan', 'Buy Another Plan', "Doesn't Buy"],
        datasets: [{
            label: 'Customer Retention Rate',
            data: [15, 10, 4],
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(75, 192, 192)',
                'rgb(255, 205, 86)',
            ]
        }]
    },
    options: {
        backdropPadding: {
            x: 10,
            y: 4
        },
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
});
