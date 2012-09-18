$(document).ready(function () {
    var addBtn = $('#btnAdd');
    addBtn.click(function () {
        var line = $('<li></li>');
        var fileInput = $('<input type="file" name ="contract" />');
        var rmInput = $('<input type="button" id="btnRm" value="Remove" />');

        rmInput.click(function () {
            line.remove();
        });

        line.append(fileInput);
        line.append(rmInput);
        addBtn.before(line);
    });
});