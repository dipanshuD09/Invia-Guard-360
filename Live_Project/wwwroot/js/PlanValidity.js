var id = $('#customer-id').val();

function fetchData() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/charts/PlanValidity/' + id,
            method: 'GET',
            success: resolve,
            error: reject
        });
    });
}

fetchData()
    .then((data) => {
        var startDate = new Date(data.Plan_Start);
        var endDate = new Date(data.Plan_End);
        var currentDate = new Date();
        var totalDays = Math.floor((endDate - startDate) / (1000 * 60 * 60 * 24));
        var daysPassed = Math.floor((currentDate - startDate) / (1000 * 60 * 60 * 24));
        var daysLeft = totalDays - daysPassed;

        var $myContainer = $('#PlanValidity');

        $myContainer.html(`Days Left: <span class='tomato-red'> ${daysLeft}</span>`);

        $myContainer.css({
            'font-size': '2rem',
            'font-weight': 'bolder',
        });

        $myContainer.find('.tomato-red').css({
            'color': 'tomato',
        });

    })
    .catch((error) => {
        console.error("Error fetching data:", error);
    });
