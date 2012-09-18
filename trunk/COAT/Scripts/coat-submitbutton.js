(function ($) {
    $.fn.MvcSubmit = function (targetAction, currrentAction, common, form) {
        var _form = form || $('form');
        var _comm = common || $('#comment');
        var _currAct = currrentAction || 'Details';
        var _targAct = targetAction || '';

        $(this).click(function () {
            if ($('#assignToId option:selected').val() == '') {
                alert("You should select a person before submit !")
                $('#assignToId').focus();
                return;
            }

            if (_comm.val() == '') {
                alert("Comment should not be empty!")
                _comm.focus();
                return;
            }

            var _act = _form.attr('action').replace(_currAct, _targAct);
            _form.attr('action', _act).submit();
        });
    };

    $.fn.RejectSubmite = function (targetAction, currentAction, form) {
        var _comm = $('#comment');
        var _form = form || $('form');
        var _currAct = currentAction || 'Details';
        var _targAct = targetAction || '';
        var _dialogComment = $('#rejectComment');

        var option = {
            width: _dialogComment.width() + 45,
            autoOpen: false,
            modal: true,
            buttons: [
                {
                    text: 'Save',
                    click: function () {
                        _comm.val(GetRejectComment());

                        if (_comm.val() == '') {
                            alert("Comment should not be empty!")
                            _dialogComment.focus();
                            return;
                        }

                        $(this).dialog('option', 'disable', true);

                        var _act = _form.attr('action').replace(_currAct, _targAct);
                        _form.attr('action', _act).submit();
                    }
                }, {
                    text: 'Cancel',
                    click: function () { $(this).dialog("close"); }
                }]
        };

        var dialog = $('#rejectDialog').dialog(option);


        var regUser = $('#rejectOption');
        regUser.change(function () {
            var val = $('#rejectOption option:selected').val();
            if (val == 0) {
                dialog.dialog('option', 'width', _dialogComment.width() + 45);
                _dialogComment.show('blind');
            } else {
                _dialogComment.hide('blind');
                dialog.dialog('option', 'width', 300);
            }
        });
        $("#rejectOption option:first-child").attr("selected", "selected");

        $(this).click(function () {
            dialog.dialog('open');
        });
    };

    $.fn.RollBackSubmit = function (targetAction, currentAction, form) {
        var _comm = $('#comment');
        var _form = form || $('form');
        var _currAct = currentAction || 'Details';
        var _targAct = targetAction || '';
        var _dialogComment = $('#reAssignComment');

        var option = {
            width: _dialogComment.width() + 45,
            autoOpen: false,
            modal: true,
            buttons: [
                {
                    text: 'Save',
                    click: function () {
                        _comm.val(GetReAssignComment());

                        if (_comm.val() == '') {
                            alert("Comment should not be empty!")
                            _dialogComment.focus();
                            return;
                        }

                        //We should set a assigner for assign wrong mark use.
                        if ($('#assignToId option:selected').val() == '') {
                            $("#assignToId option:nth-child(2)").attr("selected", "selected");
                        }

                        $(this).dialog('option', 'disable', true);

                        var _act = _form.attr('action').replace(_currAct, _targAct);
                       _form.attr('action', _act).submit();
                    }
                }, {
                    text: 'Cancel',
                    click: function () { $(this).dialog("close"); }
                }]
        };
        var dialog = $('#reAssign').dialog(option);


        var regUser = $('#reAssignUser');
        regUser.prepend($('<option value="0">Write Custom Comment</option>'))
        regUser.change(function () {
            var val = $('#reAssignUser option:selected').val();
            if (val == 0) {
                dialog.dialog('option', 'width', _dialogComment.width() + 45);
                _dialogComment.show('blind');
            } else {
                _dialogComment.hide('blind');
                dialog.dialog('option', 'width', 300);
            }
        });
        $("#reAssignUser option:first-child").attr("selected", "selected");


        $(this).click(function () {
            dialog.dialog('open');
        });
    };

    function GetRejectComment() {
        var val = $('#rejectOption option:selected').val();
        if (val == 0) {
            return $('#rejectComment').val();
        }

        var user = $('#rejectOption option:selected').text();
        return 'This Deal is rejeccted, because ' + user;
    }

    function GetReAssignComment() {
        var val = $('#reAssignUser option:selected').val();
        if (val == 0) {
            return $('#reAssignComment').val();
        }

        var user = $('#reAssignUser option:selected').text();
        return 'Should Assign to ' + user;
    }

})(jQuery);