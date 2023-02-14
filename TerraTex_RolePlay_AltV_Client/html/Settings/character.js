function setCurrentCharacterHistory(text) {
    $("#history").val(text);
}

function resetCharacter() {
    $("#history-success").toggleClass("hidden", true);
}

function setHistorySuccess() {
    resetAccount();
    $("#history-success").toggleClass("hidden", false);
}

function saveNewHistory() {
    setHistorySuccess();
    resourceCall("saveHistory", $("#history").val());
}