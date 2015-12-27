<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Blacksmith.Search" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <link rel="stylesheet" href="Content/datatables.min.css"/>

<%--  <script type="text/javascript" src="https://cdn.datatables.net/s/bs-3.3.5/jq-2.1.4,dt-1.10.10,r-2.0.0/datatables.min.js"></script>--%>
  <script src="/Scripts/datatables.min.js"></script>
  <script src="/Scripts/Search.min.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <header>
    <h1>Search</h1>
  </header>

  <section id="box">
    <asp:TextBox ID="SearchBox" runat="server" placeholder="Enter serach term"></asp:TextBox>
    <asp:RadioButtonList ID="SearchCriteriaList" runat="server" RepeatDirection="Horizontal">
      <asp:ListItem>Title</asp:ListItem>
      <asp:ListItem>Address</asp:ListItem>
      <asp:ListItem>Description</asp:ListItem>
      <asp:ListItem>Tags</asp:ListItem>
    </asp:RadioButtonList>
  </section>

  <section id="result">
    <i class="fa fa-star"></i>
    <i class="fa fa-comments"></i>
    <i class="fa fa-clock-o"></i>
  </section>
<%--  https://www.asp.net/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/display_data_items_and_details --%>
<%--look here for an example of generated table--%>

<%--  <table id="mata"></table>--%>
<%--  <asp:SqlDataSource runat="server" ID="LinksDataSource"--%>
<%--    SelectCommand="Select title From Links"--%>
<%--    ConnectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-Blacksmith-20151126125322.mdf;Initial Catalog=aspnet-Blacksmith-20151126125322;Integrated Security=True">    --%>
<%--  </asp:SqlDataSource>--%>
<%----%>
<%--  <asp:GridView ID="resultsGrid" runat="server"--%>
<%--    DataSourceID="LinksDataSource" CssClass="results-grid">--%>
<%--    --%>
<%--  </asp:GridView>--%>

</asp:Content>
