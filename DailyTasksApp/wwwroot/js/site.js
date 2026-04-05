document.addEventListener("DOMContentLoaded", function(){
    document.querySelectorAll('.edit-single-tasks-link').forEach(function(link){
        link.addEventListener("click", function(){
            let id = this.getAttribute("data-daily-tasks-id");
            let overlay = this.closest('.partial-container').querySelector('.modal-overlay');
            showEditTasksModal(id, overlay);
        });
    });
    
    function showEditTasksModal(dailyTaskId, overlay){
        $.ajax({
            url: "/DailyTasks/ShowEditTasksModal", // server endpoint to call
            data: {currentDailyTaskId: dailyTaskId }, // what data to send?
            dataType: 'html', // what do we expect in return from the server
            beforeSend: function(){
                // optional:  can show a loader here if needed
            },
            success: function(data){ // run when server responds successfully
                $(overlay).find('.edit-modal-body').html(data);
                $(overlay).css('display', 'flex'); // show the modal

                // at this moment in time, the button exists on the screen click handler can be attached
                $(overlay).find("#cancel-edit-btn").on('click', function(){
                    $(overlay).css('display', 'none');
                });

                // close modal when clicking the dark backdrop
                $(overlay).on('click', function(e){
                    if(e.target === this){
                        $(overlay).css('display', 'none');
                    }
                });
            }
        });
    }
    
});
