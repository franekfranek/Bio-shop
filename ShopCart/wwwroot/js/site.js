//$(function () {

//    if ($("a.confirmDeletion").length) {
//        $("a.confirmDeletion").click(() => {
//            if (!confirm("Confirm deletion")) return false;
//        })
//    }
//});

let deleteElements = document.getElementsByClassName("confirmDeletion");

for (var i = 0; i < deleteElements.length; i++) {
    deleteElements[i].addEventListener('click', displayConfirmation);
}

function displayConfirmation() {
    if (window.confirm("Do you really want to delete this element?")) {
        return true;
    } else return false;

}

