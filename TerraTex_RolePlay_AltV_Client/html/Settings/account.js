function setCurrentEMail(email) {
    $("#staticEmail").val(email);
}

function resetAccount() {
    $("#pw-error").toggleClass("hidden", true);
    $("#pw-success").toggleClass("hidden", true);
    $("#mail-error").toggleClass("hidden", true);
    $("#mail-success").toggleClass("hidden", true);
}

function setMailStatus(success) {
    resetAccount();
    $(success ? "#mail-success" : "#mail-error").toggleClass("hidden", false);
}

function setPWStatus(success) {
    resetAccount();
    $(success ? "#pw-success" : "#pw-error").toggleClass("hidden", false);
}

function saveNewPassword() {
    resetAccount();
    const oldPw = $("#password").val();
    const newPw = $("#newPassword").val();
    const newPwWdh = $("#newPasswordWdh").val();

    if (newPw !== newPwWdh || newPw.length < 8) {
        setPWStatus(false);
        return;
    }

    resourceCall("saveNewPW", oldPw, newPw);
}


function saveNewEMail() {
    resetAccount();
    const newMail = $("#newMail").val();
    const newMailWdh = $("#newMailWdh").val();

    if (newMail !== newMailWdh) {
        setMailStatus(false);
        return;
    }

    setMailStatus(true);
    resourceCall("saveNewMail", newMail);
}