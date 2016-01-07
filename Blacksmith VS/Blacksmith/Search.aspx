<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Blacksmith.Search" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <link rel="stylesheet" href="Content/datatables.min.css"/>

<%--  <script type="text/javascript" src="https://cdn.datatables.net/s/bs-3.3.5/jq-2.1.4,dt-1.10.10,r-2.0.0/datatables.min.js"></script>--%>
  <script src="/Scripts/datatables.min.js"></script>
  <script src="/Scripts/moment.min.js"></script>
  <script src="/Scripts/ConvertDatesShort.min.js"></script>
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
      <asp:ListItem>Category</asp:ListItem>
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
      <table id="search-results" class="table table-hover">
        <thead>
          <%-- <th title="Id"><i class="fa fa-hashtag"></i></th> --%>
          <th title="Title"><i class="fa fa-link"></i></th>
          <th title="Address"><i class="fa fa-globe"></i></th>
          <th title="Description"><i class="fa fa-align-left"></i></th>
          <th title="Submitter"><i class="fa fa-user"></i></th>
          <th title="Comments"><i class="fa fa-comments"></i></th>
          <th title="Date submitted"><i class="fa fa-clock-o"></i></th>
          <th title="Favorites"><i class="fa fa-star"></i></th>
          <th title="Categories"><i class="fa fa-tags"></i></th>
        </thead>
          
          <tr id="itemPlaceholder" runat="server"></tr>
      </table>
    </LayoutTemplate>
    
    <ItemTemplate>
      <tr>
        <%-- <td><%# Item.Id %></td> --%>
        <td><a href="Link?addr=<%# Item.Address %>"><%# Item.Title %></a></td>
        <td><a href="http://<%# Item.Address %>"><%# Item.Address %></a></td>
        <td><%# Item.Description %></td>
        <td><a href="Profile?user=<%# Item.Submitter.UserName %>"><%# Item.Submitter.UserName %></a></td>
        <td><%# Item.Comments.Count %></td>
        <td class="date short-diff" abs-date="<%# Item.Date %>"><%# Item.Date %></td>
        <td><%# Item.Favorites.Count %> </td>
        <td> TODO </td>
      </tr>
    </ItemTemplate>

  </asp:ListView>
  
</asp:Content>
