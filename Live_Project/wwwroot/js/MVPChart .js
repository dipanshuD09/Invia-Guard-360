const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun"];
const basicPlanData = [20, 25, 10, 30, 49, 35];
const standardPlanData = [15, 12, 8, 15, 2, 20];
const premiumPlanData = [20, 12, 14, 18, 6, 10];

const ctx3 = document.getElementById("myChart-3").getContext("2d");

const chart = new Chart(ctx3, {
    type: 'bar', // Change the chart type to 'bar'
    data: {
        labels: months,
        datasets: [
            {
                label: 'Basic Plan',
                data: basicPlanData,
                backgroundColor: 'blue',
            },
            {
                label: 'Standard Plan',
                data: standardPlanData,
                backgroundColor: 'green', 
            },
            {
                label: 'Premium Plan',
                data: premiumPlanData,
                backgroundColor: 'orange', 
            }
        ]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            },
        },
    }
});
