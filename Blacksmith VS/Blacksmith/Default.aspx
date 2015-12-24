<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Blacksmith._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  <header class="text-center">
    <h1>Blacksmith</h1>
    <p>Find and share links</p>
  </header>

  <section id="links" class="fluid-container row">
    <section id="popular" class="col-md-6">
      <h3>Popular Links</h3>
      TODO: ordered list of most popular links with star count (paginated)
    </section>
    <section id="recent" class="col-md-6">
      <h3>Recent Links</h3>
        <asp:ListView ID="RecentLinksView" runat="server"
          SelectMethod="GetRecentLinks2"
          ItemType="Blacksmith.Models.Link">

          <ItemTemplate>
            <img src="<%#: Item.Favicon %>" alt="Favicon"/>
            <%#: Item.Address %> by <%#: Item.Submitter.UserName %> (<%#: Item.Comments.Count %> comments)
            <%--<img src="<%#: Item.Thumbnail %>" alt="Thumbnail"/>--%>
            <br />
          </ItemTemplate>

        </asp:ListView>

    </section>
  </section>
</asp:Content>
