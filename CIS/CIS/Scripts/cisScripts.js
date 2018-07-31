

// sets value to the hidden form getFaceID.
// this will be used to store 

$("#faceDropdown li").click(function () {
    alert(this.id);
    $("#getFaceID").val(this.id);
    $("#menu1").html(this.id + " <span class='caret'></span>");

});

$("#hairDropDown li").click(function () {

    alert(this.id);
    $("#getHairId").val(this.id);
    $("#menu2").html(this.id + " <span class='caret'></span>");

});



