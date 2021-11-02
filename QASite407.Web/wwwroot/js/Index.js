$(() => {

    $("#like-question").on('click', function () {
        const id = $("data-question-id").val();
        $.post('/home/like', { id }, function () {
            console.log("hi")
            updateLikes();
        });
    })
    function updateLikes() {
        const id = $("data-question-id").val();
        $.get('/home/getlikes', { id }, function (result) {
            $("#likes-count").text(result.likes);
        });

})