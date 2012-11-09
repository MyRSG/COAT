(function($) {
    $.fn.MvcSubmit = function(targetAction, currrentAction, common, form) {
        var _form = form || $('form');
        var comm = common || $('#comment');
        var currAct = currrentAction || 'Details';
        var targAct = targetAction || '';

        $(this).click(function() {
            if ($('#assignToId option:selected').val() == '') {
                alert("You should select a person before submit !");
                $('#assignToId').focus();
                return;
            }

            if (comm.val() == '') {
                alert("Comment should not be empty!");
                comm.focus();
                return;
            }

            var act = _form.attr('action').replace(currAct, targAct);
            _form.attr('action', act).submit();
        });
    };

    $.fn.RejectSubmite = function(targetAction, currentAction, form) {
        var comm = $('#comment');
        var _form = form || $('form');
        var currAct = currentAction || 'Details';
        var targAct = targetAction || '';
        var dialogComment = $('#rejectComment');

        var option = {
            width: dialogComment.width() + 45,
            autoOpen: false,
            modal: true,
            buttons: [
                {
                    text: 'Save',
                    click: function() {
                        comm.val(getRejectComment());

                        if (comm.val() == '') {
                            alert("Comment should not be empty!");
                            dialogComment.focus();
                            return;
                        }

                        $(this).dialog('option', 'disable', true);

                        var act = _form.attr('action').replace(currAct, targAct);
                        _form.attr('action', act).submit();
                    }
                }, {
                    text: 'Cancel',
                    click: function() { $(this).dialog("close"); }
                }]
        };

        var dialog = $('#rejectDialog').dialog(option);


        var regUser = $('#rejectOption');
        regUser.change(function() {
            var val = $('#rejectOption option:selected').val();
            if (val == 0) {
                dialog.dialog('option', 'width', dialogComment.width() + 45);
                dialogComment.show('blind');
            } else {
                dialogComment.hide('blind');
                dialog.dialog('option', 'width', 300);
            }
        });
        $("#rejectOption option:first-child").attr("selected", "selected");

        $(this).click(function() {
            dialog.dialog('open');
        });
    };

    $.fn.RollBackSubmit = function(targetAction, currentAction, form) {
        var comm = $('#comment');
        var _form = form || $('form');
        var currAct = currentAction || 'Details';
        var targAct = targetAction || '';
        var dialogComment = $('#reAssignComment');

        var option = {
            width: dialogComment.width() + 45,
            autoOpen: false,
            modal: true,
            buttons: [
                {
                    text: 'Save',
                    click: function() {
                        comm.val(getReAssignComment());

                        if (comm.val() == '') {
                            alert("Comment should not be empty!");
                            dialogComment.focus();
                            return;
                        }

                        //We should set a assigner for assign wrong mark use.
                        if ($('#assignToId option:selected').val() == '') {
                            $("#assignToId option:nth-child(2)").attr("selected", "selected");
                        }

                        $(this).dialog('option', 'disable', true);

                        var act = _form.attr('action').replace(currAct, targAct);
                        _form.attr('action', act).submit();
                    }
                }, {
                    text: 'Cancel',
                    click: function() { $(this).dialog("close"); }
                }]
        };
        var dialog = $('#reAssign').dialog(option);


        var regUser = $('#reAssignUser');
        regUser.prepend($('<option value="0">Write Custom Comment</option>'));
        regUser.change(function() {
            var val = $('#reAssignUser option:selected').val();
            if (val == 0) {
                dialog.dialog('option', 'width', dialogComment.width() + 45);
                dialogComment.show('blind');
            } else {
                dialogComment.hide('blind');
                dialog.dialog('option', 'width', 300);
            }
        });
        $("#reAssignUser option:first-child").attr("selected", "selected");


        $(this).click(function() {
            dialog.dialog('open');
        });
    };

    function getRejectComment() {
        var val = $('#rejectOption option:selected').val();
        if (val == 0) {
            return $('#rejectComment').val();
        }

        var user = $('#rejectOption option:selected').text();
        return 'This Deal is rejeccted, because ' + user;
    }

    function getReAssignComment() {
        var val = $('#reAssignUser option:selected').val();
        if (val == 0) {
            return $('#reAssignComment').val();
        }

        var user = $('#reAssignUser option:selected').text();
        return 'Should Assign to ' + user;
    }

})(jQuery);