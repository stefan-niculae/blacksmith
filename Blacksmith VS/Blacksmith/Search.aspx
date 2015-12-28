<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Blacksmith.Search" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <link rel="stylesheet" href="Content/datatables.min.css"/>

<%--  <script type="text/javascript" src="https://cdn.datatables.net/s/bs-3.3.5/jq-2.1.4,dt-1.10.10,r-2.0.0/datatables.min.js"></script>--%>
  <script src="/Scripts/datatables.min.js"></script>
  <script src="/Scripts/moment.min.js"></script>
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
<%--  https://www.asp.net/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/display_data_items_and_details --%>
<%--look here for an example of generated table--%>
  
  <asp:GridView runat="server"></asp:GridView>

  <asp:ListView 
    runat="server"
    ItemType="Blacksmith.Models.Link"
    SelectMethod="GetLinks">
    <EmptyDataTemplate>
      No results found
    </EmptyDataTemplate>

    
    <LayoutTemplate>
      <table id="search-results" class="table table-hover table-striped">
        <thead>
          <th>
            <td>Title</td>
            <td>Address</td>
            <td>Description</td>
            <td>Submitter</td>
            <td><i class="fa fa-comments"></i></td>
            <td><i class="fa fa-clock-o"></i></td>
            <td><i class="fa fa-star"></i></td>
            <td>Tags</td>
          </th>
          <tr id="itemPlaceholder" runat="server"></tr>
        </thead>
      </table>
    </LayoutTemplate>
    
    <ItemTemplate>
      <tr>
        <td><%# Item.Id %></td>
        <td><%# Item.Title %></td>
        <td><%# Item.Address %></td>
        <td><%# Item.Description %></td>
        <td><%# Item.Submitter.UserName %></td>
        <td><%# Item.Comments.Count %></td>
        <td class="date difference" abs-date="<%# Item.Date %>"><%# Item.Date %></td>
        <td> - </td>
        <td> - </td>
      </tr>
    </ItemTemplate>

  </asp:ListView>
  

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
