<%@ Page Title="" Language="C#" MasterPageFile="~/Plantilla.Master" AutoEventWireup="true" CodeBehind="frmClientes.aspx.cs" Inherits="InterfazWeb.frmClientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="container">
        <div class="card-header text-center">
            <h1>Mantenimiento de Clientes</h1>
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
        
        <div class="mb-3">
            <asp:Label ID="lblID" runat="server" Text="Código" CssClass="form-label">

                <asp:RequiredFieldValidator ID="rfvtxtnombre" runat="server" ErrorMessage="El nombre es obligatorio." ControlToValidate="txtnombre" Text="*" ValidationGroup="1" Font-Italic="true" ForeColor="#FF5050"></asp:RequiredFieldValidator>
            
            </asp:Label>
            <asp:TextBox ID="txtIDCliente" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtNombre" AutoCompleteType="Disabled" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblTelefono" runat="server" Text="Teléfono" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtTelefono" AutoCompleteType="Disabled" runat="server" CssClass="form-control" TextMode="Number" MaxLength="8"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblDireccion" runat="server" Text="Dirección" CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtDireccion" AutoCompleteType="Disabled" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
        </div>
        
        <!--Botones de validación-->
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-primary" ValidationGroup="1" OnClick="btnGuardar_Click"/>
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-secondary" OnClick="btnCancelar_Click" />
        <asp:ValidationSummary ID="vsResumen" runat="server" CssClass="mt-3" ValidationGroup="1" Font-Italic="true" ForeColor="#FF5050" />

    </div>

</asp:Content>      
