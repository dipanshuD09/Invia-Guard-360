var id = $('#customer-id').val();

function fetchData() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/charts/RoundsLeft/' + id,
            method: 'GET',
            success: resolve,
            error: reject
        });
    });
}


function updateProgressChart(data) {
    if (data) {
      /*  var completedPercentage = Math.ceil((data.Current_Round / data.Max_Rounds) * 100);
        var remainingPercentage = 100 - completedPercentage;*/

        var $myContainer = $('#progressChart');

        $myContainer.html(`Round:  <span  class="tomato-red"> ${data.Current_Round}</span>/${data.Max_Rounds}`);

        $myContainer.css({
            'font-size': '2rem',
            'font-weight':'bolder',
        });

        $myContainer.find('.tomato-red').css({
            'color': 'tomato',
        });

        $myContainer.fadeIn(1000); 
    } else {
        console.error("Data is undefined or null");
    }
}

fetchData()
    .then(updateProgressChart)
    .catch(function (error) {
        console.error("Error fetching data:", error);
    });
