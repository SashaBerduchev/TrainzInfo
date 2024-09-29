Api();

function Api() {
    $(document).ready(function () {
        $('#cities').autocomplete({
            source: '/api/ApiController/cities'
        });
        $('#oblasts').autocomplete({
            source: 'api/ApiController/oblasts'
        });
        $('#stations').autocomplete({
            source: 'api/ApiController/stations'
        });
        $('#locomotivesmodel').autocomplete({
            source: 'api/ApiController/locomotives'
        })
    });
}