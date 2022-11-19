/*=========================================================================================
    File Name: kanban.js
    Description: kanban plugin
    ----------------------------------------------------------------------------------------
    Item Name: Modern KANABAN
    Author: AKRAM HAMDI
   
==========================================================================================*/

$(document).ready(function() {
    if($("#kanban-wrapper").length >0){
var kanban_curr_el, kanban_curr_item_id, kanban_item_title, kanban_data, kanban_item, kanban_users;

    // appel de la liste des taches

    $("#spinner").hide();
    var ajaxResult = null;
    ajaxResulta = [];
    // set tache non commencer array

    ajaxResultCommencer = [];
    // set tache en execution

    ajaxResultEnExecution = [];
    // set tache en pause

    ajaxResultEnPause = [];

    // set tache a valider

    ajaxResultAValider = [];

    // set tache a tester

    ajaxResultAtester = [];

    // set tache terminee

    ajaxResultTerminee = [];
    $("#btnFilter").click(
        function() {
            $("#kanban-wrapper").html("");
            $("#spinner").show();
            // set params for ajax
            dataReclamation = {
                CodeRespensable: $("#CodeRespensable").val(),
                CodeClient: $("#CodeClient").val(),
                NomPlanificateur: $("#NomPlanificateur").val(),
                UtilisateurCreateur: userName, // session connecter
            };

            // set ajax
            $.ajax({
                    method: "POST",
                    async: false,
                    contentType: 'application/json',
                    dataType: 'json',
                    url: "/Crm_TacheReclamation/listeTache",
                    data: JSON.stringify(dataReclamation)
                })
                .done(function(msg) {
                    // set tache non commencer array

                    ajaxResultCommencer = [];
                    // set tache en execution

                    ajaxResultEnExecution = [];
                    // set tache en pause

                    ajaxResultEnPause = [];

                    // set tache a valider

                    ajaxResultAValider = [];

                    // set tache a tester

                    ajaxResultAtester = [];

                    // set tache terminee

                    ajaxResultTerminee = [];
                    var array = JSON.parse(msg);
                    array.forEach(function(object) {

                        // get tache non commencer
                        if (object.Status === "E11") {
                            ajaxResultCommencer.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,
                            });
                        }
                        // get tach en execution
                        if (object.Status === "E07") {
                            ajaxResultEnExecution.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,

                            });
                        }

                        // get tach en pause
                        if (object.Status === "E06") {
                            ajaxResultEnPause.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,

                            });
                        }

                        // get tach A Valider
                        if (object.Status === "E04") {
                            ajaxResultAValider.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,

                            });
                        }

                        // get tach A Tester
                        if (object.Status === "E03") {
                            ajaxResultAtester.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,

                            });
                        }
                        // get tach terminee
                        if (object.Status === "E01") {
                            ajaxResultTerminee.push({
                                id: object.NumeroTache,
                                title: object.TacheTitre,
                                comment: object.DescriptionTache,
                                border: "success",
                                dueDate: object.DateCreation,
                                attachment: object.UtilisateurCreateur,

                            });
                        }

                    });
                    let nonCommencer = {};
                    ajaxResult = [];
            ajaxResult.push({
                    id: "kanban-board-1",
                    title: "Non Commencé",
                    dragTo: ['kanban-board-2'],
                    item: ajaxResultCommencer
                });
            if(userRole == "01" || userRole == "02"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-5'],
                item: ajaxResultAValider,
            }, {
                id: "kanban-board-5",
                title: "A tester",
                dragTo: ['kanban-board-4', 'kanban-board-6'],
                item: ajaxResultAtester,
            }, {
                id: "kanban-board-6",
                title: "Terminé",
                dragTo: ['kanban-board-7'],
                item: ajaxResultTerminee,
            });
            }
            if(userRole == "03"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-5'],
                item: ajaxResultAValider,
            });
            }


                    // Kanban Board and Item Data passed by json
                    var kanban_board_data = ajaxResult;
                    // Kanban Board

                    var KanbanExample = new jKanban({
                        element: "#kanban-wrapper", // selector of the kanban container
                        buttonContent: "", // text or html content of the board button
                        dragBoards       : false,
                        // click on current kanban-item
                        click: function(el) {
      
                            // kanban-overlay and sidebar display block on click of kanban-item
                            $(".kanban-overlay").addClass("show");
                            $(".kanban-sidebar").addClass("show");
                            // Set el to var kanban_curr_el, use this variable when updating title
                            kanban_curr_el = el;
                            // Extract  the kan ban item & id and set it to respective vars
                           
                           kanban_item_title = $(el).contents()[0].data;
                            kanban_item_description = $(el).contents()[1].data;
                            kanban_curr_item_id = $(el).attr("data-eid");
                            kanban_curr_item_comment = $(el).attr("data-comment");
                            kanban_curr_item_date = $(el).attr("data-duedate");
                            // set edit title
                    $(".edit-kanban-item .edit-kanban-item-title").html("<span class='title-kanban'>" + kanban_item_title + "</span>");
                    $(".edit-kanban-item .edit-kanban-item-date").val(kanban_curr_item_date);
                    $(".edit-kanban-item .edit-kanban-item-comment").html(kanban_curr_item_comment);

                        },
                        buttonClick: function(el, boardId) {},
                        addItemButton: false, // add a button to board for easy item creation
                        boards: kanban_board_data // data passed from defined variable
                    });
                    // Add html for Custom Data-attribute to Kanban item
                    var board_item_id, board_item_el;
                    // Kanban board loop
                    for (kanban_data in kanban_board_data) {
                        // Kanban board items loop
                        for (kanban_item in kanban_board_data[kanban_data].item) {
                            var board_item_details = kanban_board_data[kanban_data].item[kanban_item]; // set item details
                            board_item_id = $(board_item_details).attr("id"); // set 'id' attribute of kanban-item

                            (board_item_el = KanbanExample.findElement(board_item_id)), // find element of kanban-item by ID
                            (board_item_users = board_item_dueDate = board_item_comment = board_item_attachment = board_item_image = board_item_badge =
                                " ");

                            // check if users are defined or not and loop it for getting value from user's array
                            if (typeof $(board_item_el).attr("data-users") !== "undefined") {
                                for (kanban_users in kanban_board_data[kanban_data].item[kanban_item].users) {
                                    board_item_users +=
                                        '<li class="avatar pull-up my-0">' +
                                        '<img class="media-object" src=" ' +
                                        kanban_board_data[kanban_data].item[kanban_item].users[kanban_users] +
                                        '" alt="Avatar" height="18" width="18">' +
                                        "</li>";
                                }
                            }
                            // check if dueDate is defined or not
                            if (typeof $(board_item_el).attr("data-dueDate") !== "undefined") {
                                board_item_dueDate =
                                    '<div class="kanban-due-date mr-50">' +
                                    '<i class="ft-clock font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-dueDate") +
                                    "</span>" +
                                    "</div>";
                            }
                            // check if comment is defined or not
                            if (typeof $(board_item_el).attr("data-comment") !== "undefined") {
                                board_item_comment =
                                    '<div class="kanban-comment mr-50">' +
                                    '<i class="ft-message-square font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-comment") +
                                    "</span>" +
                                    "</div>";
                            }
                            // check if attachment is defined or not
                            if (typeof $(board_item_el).attr("data-attachment") !== "undefined") {
                                board_item_attachment =
                                    '<div class="kanban-attachment">' +
                                    '<i class="ft-link font-size-small mr-25"></i>' +
                                    '<span class="font-size-small">' +
                                    $(board_item_el).attr("data-attachment") +
                                    "</span>" +
                                    "</div>";
                            }
                            // check if Image is defined or not
                            if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                                board_item_image =
                                    '<div class="kanban-image mb-1">' +
                                    '<img class="img-fluid" src=" ' +
                                    kanban_board_data[kanban_data].item[kanban_item].image +
                                    '" alt="kanban-image">';
                                ("</div>");
                            }
                            // check if Badge is defined or not
                            if (typeof $(board_item_el).attr("data-badgeContent") !== "undefined") {
                                board_item_badge =
                                    '<div class="kanban-badge">' +
                                    '<div class="avatar bg-' +
                                    kanban_board_data[kanban_data].item[kanban_item].badgeColor +
                                    ' font-size-small font-weight-bold">' +
                                    kanban_board_data[kanban_data].item[kanban_item].badgeContent +
                                    "</div>";
                                ("</div>");
                            }
                            // add custom 'kanban-footer'
                            if (
                                typeof(
                                    $(board_item_el).attr("data-dueDate") ||
                                    $(board_item_el).attr("data-comment") ||
                                    $(board_item_el).attr("data-users") ||
                                    $(board_item_el).attr("data-attachment")
                                ) !== "undefined"
                            ) {
                                $(board_item_el).append(
                                    '<hr/>' + board_item_comment +'<hr><div class="kanban-footer d-flex justify-content-between mt-1">' +
                                    '<div class="kanban-footer-left d-flex">' +
                                    board_item_dueDate +
                                    
                                    board_item_attachment +
                                    "</div>" +
                                    '<div class="kanban-footer-right">' +
                                    '<div class="kanban-users">' +
                                    board_item_badge +
                                    '<ul class="list-unstyled users-list cursor-pointer m-0 d-flex align-items-center">' +
                                    board_item_users +
                                    "</ul>" +
                                    "</div>" +
                                    "</div>" +
                                    "</div>"
                                );
                            }
                            // add Image prepend to 'kanban-Item'
                            if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                                $(board_item_el).prepend(board_item_image);
                            }
                        }
                    }
                    Swal.fire({
                        position: 'top-center',
                        icon: 'success',
                        title: 'Filtrée',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    $("#spinner").hide();
                });

            /// END SET AJAX****/
        }
    );


dataReclamation = {
        CodeRespensable: "",
        NomPlanificateur: UserID,
        CodeClient: "",
        ValiderPar: "",
        UtilisateurCreateur: userName, // session connecter
        Type:userRole, // role connecter
    };


    
    $.ajax({
            method: "POST",
            async: false,
            contentType: 'application/json',
            dataType: 'json',
            url: "/Crm_TacheReclamation/listeTache",
            data: JSON.stringify(dataReclamation)
        })
        .done(function(msg) {
            var array = JSON.parse(msg);
            array.forEach(function(object) {

                // get tache non commencer
                if (object.Status === "E11" || object.Status === "E10") {
                    ajaxResultCommencer.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",

                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,
                    });
                }
                // get tach en execution
                if (object.Status === "E07") {
                    ajaxResultEnExecution.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",

                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,

                    });
                }

                // get tach en pause
                if (object.Status === "E06") {
                    ajaxResultEnPause.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,

                    });
                }

                // get tach A Valider
                if (object.Status === "E04") {
                    ajaxResultAValider.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,

                    });
                }

                // get tach A Tester
                if (object.Status === "E03") {
                    ajaxResultAtester.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,

                    });
                }
                // get tach terminee
                if (object.Status === "E01") {
                    ajaxResultTerminee.push({
                        id: object.NumeroTache,
                        title: object.TacheTitre,
                        comment: object.DescriptionTache,
                        border: "success",
                        dueDate: object.DateCreation,
                        attachment: object.UtilisateurCreateur,

                    });
                }

            });
            let nonCommencer = {};
            ajaxResult = [];
            ajaxResult.push({
                    id: "kanban-board-1",
                    title: "Non Commencé",
                    dragTo: ['kanban-board-2'],
                    item: ajaxResultCommencer
                });
            if(userRole == "01" || userRole == "02"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-5'],
                item: ajaxResultAValider,
            }, {
                id: "kanban-board-5",
                title: "A tester",
                dragTo: ['kanban-board-4', 'kanban-board-6'],
                item: ajaxResultAtester,
            }, {
                id: "kanban-board-6",
                title: "Terminé",
                dragTo: ['kanban-board-7'],
                item: ajaxResultTerminee,
            });
            }
            if(userRole == "03"){
            ajaxResult.push({
                id: "kanban-board-2",
                title: "En Exécution",
                dragTo: ['kanban-board-3'],
                item: ajaxResultEnExecution
            }, {
                id: "kanban-board-3",
                title: "En Pause",
                dragTo: ['kanban-board-2', 'kanban-board-4'],
                item: ajaxResultEnPause,
            }, {
                id: "kanban-board-4",
                title: "A valider",
                dragTo: ['kanban-board-5'],
                item: ajaxResultAValider,
            });
            }
            


            // Kanban Board and Item Data passed by json
            var kanban_board_data = ajaxResult;

            // Kanban Board
            var KanbanExample = new jKanban({
                element: "#kanban-wrapper", // selector of the kanban container
                buttonContent: "", // text or html content of the board button
                dragBoards       : false,
                itemHandleOptions: {
                    enabled: false, // if board item handle is enabled or not
                    handleClass: "item_handle", // css class for your custom item handle
                    customCssHandler: "drag_handler", // when customHandler is undefined, jKanban will use this property to set main handler class
                    customCssIconHandler: "drag_handler_icon", // when customHandler is undefined, jKanban will use this property to set main icon handler class. If you want, you can use font icon libraries here
                    customHandler: "<span class='item_handle'>+</span> %key% " // your entirely customized handler. Use %title% to position item title 
                }, // any key's value included in item collection can be replaced with %key%

                // click on current kanban-item
                click: function(el) {
      
                            // kanban-overlay and sidebar display block on click of kanban-item
                            $(".kanban-overlay").addClass("show");
                            $(".kanban-sidebar").addClass("show");
                            // Set el to var kanban_curr_el, use this variable when updating title
                            kanban_curr_el = el;
                            // Extract  the kan ban item & id and set it to respective vars
                    
                            kanban_item_title = $(el).contents()[0].data;
                            kanban_item_description = $(el).contents()[1].data;
                            kanban_curr_item_id = $(el).attr("data-eid");
                            kanban_curr_item_comment = $(el).attr("data-comment");
                            kanban_curr_item_date = $(el).attr("data-duedate");
                            // set edit title
                    $(".edit-kanban-item .edit-kanban-item-title").html("<span class='title-kanban'>" + kanban_item_title + "</span>");
                    $(".edit-kanban-item .edit-kanban-item-date").val(kanban_curr_item_date);
                    $(".edit-kanban-item .edit-kanban-item-comment").html(kanban_curr_item_comment);

                        },
                dropEl: function(el, target, source, sibling) {
                    console.log(target);
                    //KanbanExample.replaceElement(target.getAttribute("data-eid"), source)
                    // KanbanExample.replaceElement(source, target)
                    // get element fraged
                    // console.log(el.getAttribute("data-duedate"))
                    console.log(el.getAttribute("data-eid"))
                        // from etat
                        //console.log(source.parentNode.dataset.id);
                    console.log(source.parentNode.dataset.order);

                    // to etat
                    //console.log(target.parentNode.dataset.id);
                    console.log(target.parentNode.dataset.order);

                    dataReclamation = {
                        Status: target.parentNode.dataset.order,
                        NumeroTache: el.getAttribute("data-eid"),
                        CodeRespensable: "",
                        NomPlanificateur: UserID,
                        CodeClient: "",
                        ValiderPar: "",
                        UtilisateurCreateur: userName, // session connecter
                        Type:userRole, // role connecter
    };
                    $.ajax({
                            method: "POST",
                            async: false,
                            contentType: 'application/json',
                            dataType: 'json',
                            url: "/Crm_TacheReclamation/UpdateStateTache",
                            data: JSON.stringify(dataReclamation)
                        })
                        .done(function(msg) {});
                },
                addItemButton: false, // add a button to board for easy item creation
                boards: kanban_board_data // data passed from defined variable
            });

            // Add html for Custom Data-attribute to Kanban item
            var board_item_id, board_item_el;
            // Kanban board loop
           

            for (kanban_data in kanban_board_data) {
                // Kanban board items loop
                for (kanban_item in kanban_board_data[kanban_data].item) {
                  
                    var board_item_details = kanban_board_data[kanban_data].item[kanban_item]; // set item details
                    board_item_id = $(board_item_details).attr("id"); // set 'id' attribute of kanban-item

                    (board_item_el = KanbanExample.findElement(board_item_id)), // find element of kanban-item by ID
                    (board_item_users = board_item_dueDate = board_item_comment = board_item_attachment = board_item_image = board_item_badge =
                        " ");

                    // check if users are defined or not and loop it for getting value from user's array
                    if (typeof $(board_item_el).attr("data-users") !== "undefined") {
                        for (kanban_users in kanban_board_data[kanban_data].item[kanban_item].users) {
                            board_item_users +=
                                '<li class="avatar pull-up my-0">' +
                                '<img class="media-object" src=" ' +
                                kanban_board_data[kanban_data].item[kanban_item].users[kanban_users] +
                                '" alt="Avatar" height="18" width="18">' +
                                "</li>";
                        }
                    }
                    // check if dueDate is defined or not
                    if (typeof $(board_item_el).attr("data-dueDate") !== "undefined") {
                        board_item_dueDate =
                            '<div class="kanban-due-date mr-50">' +
                            '<i class="ft-clock font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-dueDate") +
                            "</span>" +
                            "</div>";
                    }
                    // check if comment is defined or not
                    if (typeof $(board_item_el).attr("data-comment") !== "undefined") {
                        board_item_comment =
                            '<div class="kanban-comment mr-50">' +
                            '<i class="ft-message-square font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-comment") +
                            "</span>" +
                            "</div>";
                    }
                    // check if attachment is defined or not
                    if (typeof $(board_item_el).attr("data-attachment") !== "undefined") {
                        board_item_attachment =
                            '<div class="kanban-attachment">' +
                            '<i class="ft-link font-size-small mr-25"></i>' +
                            '<span class="font-size-small">' +
                            $(board_item_el).attr("data-attachment") +
                            "</span>" +
                            "</div>";
                    }
                    // check if Image is defined or not
                    if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                        board_item_image =
                            '<div class="kanban-image mb-1">' +
                            '<img class="img-fluid" src=" ' +
                            kanban_board_data[kanban_data].item[kanban_item].image +
                            '" alt="kanban-image">';
                        ("</div>");
                    }
                    // check if Badge is defined or not
                    if (typeof $(board_item_el).attr("data-badgeContent") !== "undefined") {
                        board_item_badge =
                            '<div class="kanban-badge">' +
                            '<div class="avatar bg-' +
                            kanban_board_data[kanban_data].item[kanban_item].badgeColor +
                            ' font-size-small font-weight-bold">' +
                            kanban_board_data[kanban_data].item[kanban_item].badgeContent +
                            "</div>";
                        ("</div>");
                    }
                    // add custom 'kanban-footer'
                    if (
                        typeof(
                            $(board_item_el).attr("data-dueDate") ||
                            $(board_item_el).attr("data-comment") ||
                            $(board_item_el).attr("data-users") ||
                            $(board_item_el).attr("data-attachment")
                        ) !== "undefined"
                    ) {
                        $(board_item_el).append(
                            '<hr/>' + board_item_comment +'<hr><div class="kanban-footer d-flex justify-content-between mt-1">' +
                            '<div class="kanban-footer-left d-flex">' +
                            board_item_dueDate +
                            
                            board_item_attachment +
                            "</div>" +
                            '<div class="kanban-footer-right">' +
                            '<div class="kanban-users">' +
                            board_item_badge +
                            '<ul class="list-unstyled users-list cursor-pointer m-0 d-flex align-items-center">' +
                            board_item_users +
                            "</ul>" +
                            "</div>" +
                            "</div>" +
                            "</div>"
                        );
                    }
                    // add Image prepend to 'kanban-Item'
                    if (typeof $(board_item_el).attr("data-image") !== "undefined") {
                        $(board_item_el).prepend(board_item_image);
                    }
                }
            }
            kanban_board_data = [];
        });



    // Add new kanban board
    //---------------------
   

    // Delete kanban board
    //---------------------


    // Kanban board dropdown
    // ---------------------


    // Kanban-overlay and sidebar hide
    // --------------------------------------------
    $(
        ".kanban-sidebar .delete-kanban-item, .kanban-sidebar .close-icon, .kanban-sidebar .update-kanban-item, .kanban-overlay"
    ).on("click", function() {
        $(".kanban-overlay").removeClass("show");
        $(".kanban-sidebar").removeClass("show");
    });

    // Updating Data Values to Fields
    // -------------------------------
    $(".update-kanban-item").on("click", function(e) {
        e.preventDefault();
    });

    // Delete Kanban Item
    // -------------------
    $(".delete-kanban-item").on("click", function() {
        $delete_item = kanban_curr_item_id;
        addEventListener("click", function() {
            KanbanExample.removeElement($delete_item);
        });
    });



    // Making Title of Board editable
    // ------------------------------


    // kanban Item - Pick-a-Date
    $(".edit-kanban-item-date").pickadate();

    // Perfect Scrollbar - card-content on kanban-sidebar
    if ($(".kanban-sidebar .edit-kanban-item .card-content").length > 0) {
        var kanbanSidebar = new PerfectScrollbar(".kanban-sidebar .edit-kanban-item .card-content", {
            wheelPropagation: false
        });
    }

    // select default bg color as selected option
    $("select").addClass($(":selected", this).attr("class"));

    // change bg color of select form-control
    $("select").change(function() {
        $(this)
            .removeClass($(this).attr("class"))
            .addClass($(":selected", this).attr("class") + " form-control text-white");
    });

}

});