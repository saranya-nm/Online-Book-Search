function validateForm() {
    console.log("this method has been hit");
    let x = document.getElementById("SearchText").value;
    if (x == "") {
        alert("Search field cannot be empty");
        return false;
    }
}