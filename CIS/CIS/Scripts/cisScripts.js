

// sets value to the hidden form getFaceID.
// this will be used to store 

$("#faceDropdown li").click(function () {

    $("#getFaceID").val(this.id);
    $("#menu1").html(this.id + " <span class='caret'></span>");

});



$("#BodyBuiltDropdown li").click(function () {

    $("#getBodyBuilt").val(this.id);
    $("#menu2").html(this.id + " <span class='caret'></span>");

});


