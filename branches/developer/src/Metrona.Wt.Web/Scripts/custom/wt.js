var dialogSelector = '#loadDialog';
$(function () {

    $.scrollToTop();

    $(dialogSelector)
        .removeClass("hidden")
        .dialog(
        {
            //autoOpen: false,
            dialogClass: "loading",
            modal: true,
            draggable: false,
            closeOnEscape: false,
            resizable: false,
            width: "auto",
            //height: 100
        }).parent().css(
        {
            position: 'fixed'
        });
    $(dialogSelector).parent().find('.ui-dialog-titlebar').hide();
    $(dialogSelector).dialog('close');

   
    $("#cmbRequestType").change(function () {
        ChangeRequestType(this);
    });
     
});

window.onload = function () {
    //var bundesland = $('#Bundesland');
    //bundesland.hide();
    $("#cmbRequestType").trigger("change");
};

function addErrorMessage(message) {
    var container = $("#FormValidationSummary"); //find("[data-valsummary=true]");
    var list = container.find("ul");
    if (list && list.length == 0) {
        list = $("<ul />").appendTo(container);
    }
    $("<li />").html(message).appendTo(list);
    container.show();
}

var pageManager = Sys.WebForms.PageRequestManager.getInstance();

pageManager.add_initializeRequest(function (sender, args) {
    var sourceId =sender._postBackSettings.sourceElement.id;
    if (sourceId == "btnPrintPDF" || sourceId == "btnExportExecl") {
        var iframe = document.createElement("iframe");
        iframe.src = "Report/GenerateFile?type=" + sourceId;
        iframe.style.display = "none";
        // Add the IFRAME to the page.  This will trigger a request to GenerateFile now.
        document.body.appendChild(iframe);
    }
});

pageManager.add_beginRequest(function () {
    jQuery(dialogSelector).dialog();
});

pageManager.add_endRequest(function (sender, args) {
    jQuery(dialogSelector).dialog('close');
    if (args._error != null) {
        addErrorMessage(args._error.message);
    }
});

function ChangeRequestType(oList) {
    var sValue = oList.options[oList.selectedIndex].value;
    var plz = $('#PLZ');
    var bundesland = $('#Bundesland');
    switch (sValue) {
        case "Plz":
            plz.show();
            bundesland.hide();
            break;
        case "Bundesland":
            plz.hide();
            bundesland.show();
            break;
        case "Deutschland":
            plz.hide();
            bundesland.hide();
        default:
            break;
    }
}