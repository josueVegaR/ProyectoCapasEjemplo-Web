<%@ Page Title="" Language="C#" MasterPageFile="~/Plantilla.Master" AutoEventWireup="true" CodeBehind="frmListadoReservas.aspx.cs" Inherits="InterfazWeb.frmListadoReservas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="card-header text-center">
            <h1>Listado de Reservaciones Pendientes</h1>
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
                Session["_mensaje"] = null;
            } %>
        <!--El If abre y cierra llaves en tags diferentes.-->

        <!--Divs para filas y columnas-->
        <div class="row mt-3"> <!--Fila-->

            <div class="col-auto"><!--Columna-->
                <asp:Label ID="lblcliente" 
                           runat="server" 
                           Text="Nombre del Cliente"
                           CssClass="col-form-label"></asp:Label>
            </div>
            <div class="col-auto"> <!--Columna-->
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            
            <div class="col-auto"><!--Columna-->
                <asp:Label ID="lblnumreserva" 
                           runat="server" 
                           Text="# Reserva"
                           CssClass="col-form-label"></asp:Label>
            </div>
            <div class="col-auto"> <!--Columna-->
                <asp:TextBox ID="txtnumreserva" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-auto"> <!--Columna-->
                <asp:Button ID="btnBuscar" 
                    runat="server" 
                    Text="Buscar"
                    CssClass="btn btn-primary"/>
            </div>
            <div class="col-auto"> <!--Columna-->
                <asp:Button ID="btnAgregar"
                    runat="server"
                    Text="Nueva Reservación"
                    CssClass="btn btn-secondary" />
            </div>
        
        </div> <!--Cierra Div para filas-->
        <br />
        <!--Grid View para cargar los clientes-->
        <asp:GridView ID="GrdLista" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="No se Registran Reservaciones." ForeColor="#333333" GridLines="None" Width="100%">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkModificar" runat="server" CommandArgument='<%# Eval("NUMRESERVACION").ToString() %>' >Modificar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEliminar" runat="server" CommandArgument='<%# Eval("NUMRESERVACION").ToString() %>' >Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NUMRESERVACION" HeaderText="# Reserva" />
                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" />
                <asp:BoundField DataField="FECHAINGRESO" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" />
                <asp:BoundField DataField="FECHASALIDA" HeaderText="Fecha Salida" DataFormatString="{0:d}" />
                <asp:BoundField DataField="CANTIDADPERSONAS" HeaderText="Ctd. Personas" />
                <asp:BoundField DataField="TIPOHABITACION" HeaderText="Tipo Habitación" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView><!--Se usa el mismo nombre que en windows forms para reutilizar código-->

    </div> <!--Cierra Div Container-->
</asp:Content>
