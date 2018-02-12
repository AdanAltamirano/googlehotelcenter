<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CategoriasCasas.aspx.vb" Inherits="RateManager.CategoriasCasas" %>

<%@ Import Namespace="RateManager" %>
<%@ Register Src="~/Modulos/CtrlIdioma.ascx" TagPrefix="uc2" TagName="CtrlIdioma" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>    
    <link href="../Includes/bootstrap.min.css" rel="stylesheet" />
    <link href="../StyleSheets/Styles.css" rel="stylesheet" />
    <link href="../Includes/sweetalert.css" rel="stylesheet" />
    <script src="../Includes/Script/jquery-3.1.1.min.js"></script>
    <script src="../Includes/Script/bootstrap.js"></script>
    <script src="../Includes/Script/sweetalert.min.js"></script>
       
    <script>
        var idCat = 0;
        var dataToUpdate = {};
        $(function () {

            $("#Hidden1").val(idCat);
            $("#btn_actualizar").hide();
            $('[data-toggle="tooltip"]').tooltip()

            $("#listado_categorias").change(function () {
                $("#Hidden1").val(this.value);
                idCat = this.value;
            });

            (function () {
                //console.log("agregar categorias de la bd");
                getAllCategories()
            })();

            
        });

        function deleteCategoryCasas(id) {

            swal({
                title: "¿Esta seguro de eliminar la categoría seleccionada ?",
                text: "Una vez eliminado no podrá ser recuperado!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Eliminar",
                closeOnConfirm: false
            },
            function () {

                jQuery.ajax({
                    type: "GET",
                    url: "Crud.ashx",
                    data: { "MethodName": "deleteCategoryCasas", "id": id },
                    success: function (response) {
                        //var data = JSON.parse(response);
                        console.log(response)

                        if (response.Error == "False") {
                            swal("Eliminado!", "La categoría a sido eliminada.", "success");
                            getAllCategories();
                        }
                    }
                });
            });
        }

        

        function getAllCategories() {
            jQuery.ajax({
                type: "GET",
                url: "Crud.ashx",
                data: { "MethodName": "GetCategoriesCasas" },
                success: function (data) {
                    //var data = JSON.parse(response);
                    console.log("Categorias ", data);

                    var trHTML = '';

                    if (data.results.length != 0) {

                        $.each(data.results, function (i, item) {
                            trHTML += '<tr id=' + item.name + '><td>' + item.name + '</td><td class="options">' +
                                        '<input type="button" id="btn_delete" class="btn btn-danger btn_crud" value="eliminar" onclick="deleteCategory(' + item.id + ')"  />' +
                                        '<!--input type="button" id="btn_update" class="btn btn-warning btn_crud" value="editar" onclick="updateCategory(' + item.id + ',' + item.id_dictionary + ',' + item.name + ')" /-->' +
                                        '</td></tr>';
                        });

                    } else {

                        swal("No hay categorías agregadas.", "Ingresa una categoría en caso de ser necesario.")
                    }

                    $('#tabla_categories').html(trHTML);

                    var options = "<option value='0'>Otros</option>";
                    for (var i = 0; i < data.results.length; i++) {
                        options += '<option value="' + data.results[i].id + '">' + data.results[i].name + '</option>';
                    }
                    $("#listado_categorias").html(options);

                }
            });
        }

        

        function updateCategory(id_cat, id_dic, id_cat_ame) {

            console.log("id_amenidad " + id_cat + " id_dic " + id_dic + " id_cat_came " + id_cat_ame);

            $('#' + id_dic).css({ "color": "#d9534f" });
            $('#' + id_dic).css({ "font-size": "15px" });

            $('#listado_categorias option[value="' + id_cat_ame + '"]').attr("selected", true);

            $("#agregar_amen").hide();
            $("#ver_amen").hide();

            $("#btn_actualizar").show();
            $("#btn_cancel").show();

            $(".btn_crud").attr("disabled", "disabled");
            $(".btn_crud_mainA").attr("disabled", "disabled");


            $("#id_amen_d").val(id_cat);
            $("#id_dic_d").val(id_dic);

            /*location.reload();
            window.close();*/

            dataToUpdate.idCat = id_cat

        }

        function cancelUpdateCat() {
            /*$("#agregar_amen").show();
            $("#ver_amen").show();*/
        }

        function errorEmptyValues() {
            swal("Valores vacíos", "Por favor ingrese el nombre en Ingles e Español.", "error");
        }

        function successInsertValues() {
            swal("Registro insertado!", "El registro se ha ingresado con éxito.", "success");
        }

        function errorDataBase() {
            swal("Error al insertar en la bd.", "", "error");
        }

        function updateSuccess() {
            swal("Registro actualizado!!.", "El registro se ha actualizado con éxito.", "success");
        }
    </script>
</head>
<body>  
    <form id="form1" class="form-inline" runat="server">
        <h2>Categorias</h2>
        <hr />
        <div class="row">
            <div class="col-md-8">
                
                <div>                
                    <h4>Agregar categorias</h4>

                    <div class="form-group" id="cat_name_content" >
                        <uc2:CtrlIdioma runat="server" ID="ctrlIdiomaCategoria" />
                    </div>
                    
                    <asp:Button id="agregar_cat" data-toggle="tooltip" data-placement="top" title="Recuerda ingresar el nombre en Inglés y Español." class="btn btn-primary btn_crud"  runat="server" Text="Agregar"/>
  
                </div>
                <hr />
                <div class="panel panel-info">
                    <div class="panel-heading">Categorías registradas</div>
                    <div class="panel-body">
                        <table class="table table-striped" id="tabla_categories">
                            
                        </table>
                    </div>
                </div> 
            </div>
        </div>
    </form>    
</body>
</html>
