// Write your Javascript code.


function servicesDateAndTimePickers() {
    $('#datepicker').datepicker({ language: 'pt' });
    $('#datepicker').datepicker('setDate', '@Model.Date.Date.ToString("O")');
    $('#datepicker').on("changeDate", function () {
        $('#date_hidden').val(
            $('#datepicker').datepicker('getFormattedDate')
        );
    });

    function setTimePickers(timePicker, inputHolder) {
        $(timePicker).on('change.bfhtimepicker', function (e) {
            var time = $(timePicker).val();
            console.log(time);
            $(inputHolder).val(time);
        });
    }

    setTimePickers('#startTimePicker', '#startTime');
    setTimePickers('#endTimePicker', '#endTime');
}