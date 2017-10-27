function IsFirstNameEmpty()
{
    if (document.getElementById('txtFName').value == "") {
        return 'First Name should not be empty';
    }
    else
        return "";
}

function IsFirstNameInValid() {
    if (document.getElementById('txtFName').value.indexOf("@") != -1) {
        return 'First Name should not contain @';
    }
    else
        return "";
}

function IsLastNameInValid() {
    if (document.getElementById('txtLName').value.length >=12) {
        return 'Last Name should not contain more than 12 chars';
    }
    else
        return "";
}
function IsAgeEmpty() {
    if (document.getElementById('txtAge').value == "") {
        return 'Age should not be empty';
    }
    else
        return "";
}
function IsAgeInValid() {
    if (isNaN(document.getElementById('txtAge').value)) {
        return 'Enter valid Age';
    }
    else
        return "";
}
function IsEnrollDateEmpty() {
    if (document.getElementById('txtEnrollDate').value == "") {
        return 'Enrollment Date should not be empty';
    }
    else
        return "";
}

function IsValid()
{
    var FirstNameEmptyMessage = IsFirstNameEmpty();
    var FirstNameInValidMessage = IsFirstNameInValid();
    var LastNameInValidMessage = IsLastNameInValid();
    var AgeEmptyMessage = IsAgeEmpty();
    var AgeInValidMessage = IsAgeInValid();
    var EnrollDateMessage = IsEnrollDateEmpty();

    var FinalErrorMessage = "Errors:";
    if (FirstNameEmptyMessage != "")
        FinalErrorMessage += "\n" + FirstNameEmptyMessage;
    if (FirstNameInValidMessage != "")
        FinalErrorMessage += "\n" + FirstNameInValidMessage;
    if (LastNameInValidMessage != "")
        FinalErrorMessage += "\n" + LastNameInValidMessage;
    if (AgeEmptyMessage != "")
        FinalErrorMessage += "\n" + AgeEmptyMessage;
    if (AgeInValidMessage != "")
        FinalErrorMessage += "\n" + AgeInValidMessage;
    if (EnrollDateMessage != "")
        FinalErrorMessage += "\n" + EnrollDateMessage;

    if (FinalErrorMessage != "Errors:") {
        alert(FinalErrorMessage);
        return false;
    }
    else
        return true;



















}