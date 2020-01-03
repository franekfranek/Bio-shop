$(function () {
    //WINDOW CONFIRMATION POPUP 
    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm deletion")) return false;
        });
    }
    // TEMP DATA NOTIFICATION HIDE
    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});

//WINDOW CONFIRMATION POPUP 
//function addListenersToDeleteElements() {
//    let deleteElements = document.getElementsByClassName("confirmDeletion");

//    for (var i = 0; i < deleteElements.length; i++) {
//        deleteElements[i].addEventListener('click', displayConfirmation);
//    }
//}

//function displayConfirmation() {
//  if (window.confirm("Do you really want to delete this element?")) {
//        return false;
//    }



//// TEMP DATA NOTIFICATION HIDE
//function hideTempData() {
//    let notification = document.getElementsByClassName("alert notification");
//    console.log(notification);
//    if (notification.length) {
//        setTimeout(function () {
//            notification[0].hidden = true
//        }, 2000);
//    }
//}

//hideTempData();
//addListenersToDeleteElements();


