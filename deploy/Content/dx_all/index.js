$(() => {
  
    $('#gridContainer').dxDataGrid({

        dataSource: {
            store: {
                type: 'odata',
                url: 'https://js.devexpress.com/Demos/SalesViewer/odata/DaySaleDtoes',
                key: 'Id',
                beforeSend(request) {
                    request.params.startDate = '2020-05-10';
                    request.params.endDate = '2020-05-15';
                },
            },
        },
        selection: {
            mode: 'multiple',
        },
        focusedRowEnabled: true,
        hoverStateEnabled: true,

        columnChooser: { enabled: true },
        scrolling: {
            mode: 'virtual',
        },
        allowColumnResizing: true,
        showBorders: true,
        columnResizingMode: 'nextColumn',
        columnMinWidth: 50,
        columnAutoWidth: true,
        headerFilter: {
            visible: true,
        },
        filterPanel: {
            visible: true,
        },
        filterRow: { visible: true, applyFilter: "onClick" },
        paging: {
            pageSize: 10,
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100],
        },
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true,
        },
        groupPanel: { visible: true },
        grouping: {
            autoExpandAll: false,
        },
        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [{
                dataField: 'Product',
                //groupIndex: 0,
            },
            {
                dataField: 'Amount',
                caption: 'Sale Amount',
                dataType: 'number',
                format: 'currency',
                alignment: 'right',
            },
            {
                dataField: 'Discount',
                caption: 'Discount %',
                dataType: 'number',
                format: 'percent',
                alignment: 'right',
                allowGrouping: false,
                cellTemplate: discountCellTemplate,
                cssClass: 'bullet',
            },
            {
                dataField: 'SaleDate',
                dataType: 'date',
            },
            {
                dataField: 'Region',
                dataType: 'string',
                hidingPriority: 0,
            },
            {
                dataField: 'Sector',
                dataType: 'string',
                hidingPriority: 1,
            },
            {
                dataField: 'Channel',
                dataType: 'string',
                hidingPriority: 2,
            },
            {
                dataField: 'Customer',
                dataType: 'string',
                width: 150,
                hidingPriority: 3,
            },
        ],
        onContentReady(e) {
            if (!collapsed) {
                collapsed = true;
                e.component.expandRow(['EnviroCare']);
            }
        },
    });
});

const discountCellTemplate = function(container, options) {
    $('<div/>').dxBullet({
        onIncidentOccurred: null,
        size: {
            width: 150,
            height: 35,
        },
        margin: {
            top: 5,
            bottom: 0,
            left: 5,
        },
        showTarget: false,
        showZeroLevel: true,
        value: options.value * 100,
        startScaleValue: 0,
        endScaleValue: 100,
        tooltip: {
            enabled: true,
            font: {
                size: 18,
            },
            paddingTopBottom: 2,
            customizeTooltip() {
                return { text: options.text };
            },
            zIndex: 5,
        },
    }).appendTo(container);
};

let collapsed = false;
$(function() {
    const types = ['error', 'info', 'success', 'warning'];

    $("#show-message").dxButton({
        text: "Show message",
        onClick: function() {
            DevExpress.ui.notify({
                    message: "You have a new message",
                    width: 230,
                    position: {
                        my: "bottom",
                        at: "bottom",
                        of: "#container"
                    }
                },
                types[Math.floor(Math.random() * 4)],
                500
            );
        },
    });
    //


    const data = [{
        ID: 1,
        Name: 'Banana',
        Category: 'Fruits'
    }, {
        ID: 2,
        Name: 'Cucumber',
        Category: 'Vegetables'
    }, {
        ID: 3,
        Name: 'Apple',
        Category: 'Fruits'
    }, {
        ID: 4,
        Name: 'Tomato',
        Category: 'Vegetables'
    }, {
        ID: 5,
        Name: 'Apricot',
        Category: 'Fruits'
    }]

    $("#selectBox").dxSelectBox({
        dataSource: data,
        searchEnabled: true,
        valueExpr: "ID",
        displayExpr: "Name",

    });
});