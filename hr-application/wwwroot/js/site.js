function fetchData() {
    var $loading = "<div class='loading'>Please wait...</div>";
    console.log("loading");
    $('#jobOffers').prepend($loading);
    $.ajax({
        url: '/api/JobOfferApi',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var $table = $('<table/>').addClass('table borderless');
            var $header = $('<thead/>').html('<tr><th>Job title</th><th>Salary</th><th>Location</th><th></th></tr>');
            $table.append($header);
            $.each(data, function (i, offer) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(offer.jobTitle));
                $row.append($('<td/>').html(offer.salary));
                $row.append($('<td/>').html(offer.location));
                $button = $('<a href="/JobOffer/Details/' + offer.id + '">Details</a>').addClass('btn btn-primary');
                $row.append($('<td/>').html($button));
                $table.append($row);
            });

            var $footer = $('<tr/>');
            $table.append($footer);

            $('#jobOffers').html($table);
        },
        error: function () {
            alert('Error! Please try again.');
        }
    }).done(function () {
        $('#jobOffers').remove('#loading');
    });
}
