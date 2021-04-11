$(() => {
   
    let questionId = $("#question-id").val();
   
    setInterval(() => {

        $.get(`/questions/getlikesforquestion?questionId=${questionId}`, function (likes) {
         
            $("#likes-count").text(likes);
        })
    }, 1000)

    $("#like-question").on('click', function () {
        $.post(`/questions/likequestion?questionId=${questionId}`, function(){
            $("#like-question").removeClass('oi oi-heart');
            $("#like-question").addClass('oi oi-heart text-danger');
            $("#like-question").prop('disabled', true);
        })
    })
})