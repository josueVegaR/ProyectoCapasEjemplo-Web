<%@ Page Title="" Language="C#" MasterPageFile="~/Plantilla.Master" AutoEventWireup="true" CodeBehind="frmReserva.aspx.cs" Inherits="InterfazWeb.frmReserva" %>

<%-- Content PlaceHolder del Encabezado --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%-- Script de jQuery --%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" 
        crossorigin="anonymous"></script>
    <script src="JS/Funciones.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <!--Empezamos a dibujar el código del form-->

    <div class="container">
        <div class="card-header text-center">
            <h1>Reservaciones</h1>
        </div>
        <!--Se muestra una alerta al cliente si el session storage tiene algo-->
        <!--Se inserta un código C#-->
        <%if (Session["_mensaje"] != null)
          { %>
            
            <!--Acá se mezcla código HTML, dentro de código C#.-->
            <div class="alert alert-warning alert-dismissible fade show mt-3" role="alert">
                <%=Session["_mensaje"] %> <!--Aquí se evalúa la expresión de la Sesión.-->
                <!--Lleva un igual y evalúa expresiones-->

                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
            
        <%
                //Session["_mensaje"] = null;
          } %>
        <!--El If abre y cierra llaves en tags diferentes.-->
        <br />

        <!--Fila-->
        <div class="row mt-3">
            
            <!--Columnas-->
            <div class="col-2">
                <asp:Label ID="lblnumreserva" runat="server" Text="# Reserva" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtnumreserva" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-2">
                <asp:Label ID="lblfecha" runat="server" Text="Fecha" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtfechaActual" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
            </div>
            <!--Se debe agregar un control de validación al cliente-->
            <div class="col-8">
                <asp:Label ID="lblcliente" runat="server" Text="Cliente">
                    <%-- Field Validator para el campo del nombre del cliente --%>
                    <asp:RequiredFieldValidator 
                        ID="rfvtxtcliente" 
                        runat="server" 
                        ErrorMessage="Debe seleccionar un cliente."
                        text="*"
                        ControlToValidate="txtcliente"
                        validationGroup="1">
                    </asp:RequiredFieldValidator>
                </asp:Label>                
                <div class="input-group">
                    <%-- Cuadro que no se ve para cargar el ID del cliente --%>
                    <asp:TextBox ID="txtidSeleccionado" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtcliente" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>

                    <%-- Se necesita utilizar un control de HTML en vez de un botón de servidor.
                        Esto es debido a que el control de servidor hace PostBack y no se puede en este caso, se perdería el modal que queremos crear.--%>
                    <input type="button" id="btnBuscarCliente" value="Buscar Cliente" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#clienteModal"/>
                </div>
                                                  
            </div>

        </div><!--Cierre Fila-->

        <div class="mt-3">
            <asp:Label ID="Label1" runat="server" Text="Fecha Ingreso" CssClass="form-label">
                
                <%-- Validaciones --%>
                <asp:RequiredFieldValidator ID="rfvtxtfechaI" 
                    runat="server" 
                    ErrorMessage="Debe seleccionar una Fecha Inicio."
                    controltoValidate="txtfechaI"
                    ValidationGroup="1"
                    Text="*">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvtxtfechaI" 
                    runat="server" 
                    ErrorMessage="La fecha de ingreso debe ser mayor o igual a la Fecha de Hoy."
                    controltoValidate="txtfechaI"
                    controltoCompare="txtfechaActual"
                    Display="Dynamic"
                    ValidationGroup="1"
                    Operator="GreaterThanEqual"
                    Text="*">
                </asp:CompareValidator>

            </asp:Label>
            <asp:TextBox ID="txtfechaI" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>
        
        <div class="mt-3">
            <asp:Label ID="Label2" runat="server" Text="Fecha Salida" CssClass="form-label">
                
                <%-- Validaciones --%>
                <asp:RequiredFieldValidator ID="rfvtxtfechaS" 
                    runat="server" 
                    ErrorMessage="Debe seleccionar una Fecha Fin."
                    controltoValidate="txtfechaS"
                    ValidationGroup="1"
                    Text="*">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvtxtfechaS" 
                    runat="server" 
                    ErrorMessage="La fecha de salida debe ser mayor a la Fecha Inicio."
                    controltoValidate="txtfechaS"
                    controltoCompare="txtfechaI"
                    Display="Dynamic"
                    ValidationGroup="1"
                    Operator="GreaterThan"
                    Text="*">
                </asp:CompareValidator>

            </asp:Label>
            <asp:TextBox ID="txtfechaS" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
        </div>

        <div class="mt-3">
            <asp:Label ID="Label3" runat="server" Text="Cantidad de Personas" CssClass="form-label">
                <%-- Validación --%>
                <asp:RequiredFieldValidator ID="rfvtxtpersonas" 
                    runat="server" 
                    ErrorMessage="Debe ingresar la cantidad de personas."
                    controltoValidate="txtpersonas"
                    validationGroup="1"
                    Text="*">
                </asp:RequiredFieldValidator>
            </asp:Label>
            <asp:TextBox ID="txtpersonas" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
        </div>
        
        <div class="mt-3">
            <asp:Label ID="Label4" runat="server" Text="Tipo de Habitación" CssClass="form-label">
                <%-- Validación --%>
            </asp:Label>
            <asp:DropDownList ID="cbotipo" runat="server" CssClass="form-select">
                <%-- Items del DropDown list --%>
                <asp:ListItem selected="True" value="Standard">Standard -> $80</asp:ListItem>
                <asp:ListItem value="Junior">Junior -> $120</asp:ListItem>
                <asp:ListItem value="Deluxe">Deluxe -> $180</asp:ListItem>
            </asp:DropDownList>
            <%-- No se valida el drop down ya que siempre va a tener un valor en la propiedad Value --%>
        </div>
        <%-- Botón de Reservar --%>
        <asp:Button ID="btnReservar" runat="server" Text="Reservar" CssClass="btn btn-primary" validationGroup="1"/>
        <asp:ValidationSummary ID="vsResumen" runat="server" ValidationGroup="1" class="mt-3" Font-Italic="True" ForeColor="#CC0000" />
    </div> <!--Cierre del contenedor-->

    <%--buscar cliente
        Este es el modal de Bootstrap con el código dentro del frmClientes, ya que es un form bastante similar--%>
    <div class="modal fade" id="clienteModal" tabindex="-1" aria-labelledby="ClienteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ClienteModalLabel">Buscar Cliente</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <%-- Fila --%>
                    <div class="row ">
                        <%-- Columna --%>
                        <div class="col">

                            <%-- Fila --%>
                            <div class="row mt-3">
                                <div class="col-auto">
                                    <asp:Label ID="Label7" runat="server" Text="Label" class="col-form-label">Nombre</asp:Label>
                                </div>
                                <div class="col-auto">
                                    <asp:TextBox ID="txtnombrecliente" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-auto">
                                    <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" />
                                </div>
                            </div>
                            <br />

                            <br />
                            <asp:GridView ID="GrdLista" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="No existen registros" ForeColor="#333333" GridLines="None" Width="100%" PagerSettings-PageButtonCount="4">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandArgument='<%# Eval("ID_CLIENTE").ToString() %>' CommandName="Seleccionar" ToolTip="Seleccionar" OnCommand="lnkSeleccionar_Command">Seleccionar</asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                <PagerSettings PageButtonCount="4"></PagerSettings>

                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
