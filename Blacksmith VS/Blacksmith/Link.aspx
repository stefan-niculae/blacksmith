<%@ Page Title="Link" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="Blacksmith.Link" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <script src="/Scripts/moment.min.js"></script>
  <script src="/Scripts/UpdateDeleteLink.min.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  
<%--  <asp:Label runat="server" ID="loglabel">label</asp:Label>--%>
<%--  address literal <asp:Literal --%>
<%--      Text="<%$RouteValue:address%>" --%>
<%--      runat="server"></asp:Literal>,--%>
  
  <%-- TODO remove this (but keep the script and styling still apply--%>
  <%
    if(CurrentLink == null)
    {
  %>
    <%--  TODO make this prettier --%>
    no link address in url!
  <% } 
    else 
    {
  %>
  <section id="submitted">
  <article class="big-link row" db-id=<%: CurrentLink.Id %>>

    <%-- For each editable field, register it as the current when selected --%>
    <%-- so on deselect, we know which db entity to update --%>
    <h3 title="Title" class="title" 
      <% if (canEdit) { %> allows-edit <% } %>
      onfocus="registerFocus('<%: CurrentLink.Id %>>', 'title')" 
      onkeyup="updateFocused()" onblur="updateFocused()">
      <a href="Link?addr=<%: CurrentLink.Address %>>">
        <%: CurrentLink.Title %>
      </a>
    </h3>
    <span class="date difference" abs-date="<%: CurrentLink.Date %>" title="Date submitted: <%: CurrentLink.Date %>"><%: CurrentLink.Date %></span>
    <br/>
            
    <img class="favicon" alt="Favicon" src="<%: CurrentLink.Favicon %>" title="Favicon"/>
    <span class="address" title="Address" 
      <% if (canEdit) { %> allows-edit <% } %>
      onfocus="registerFocus('<%: CurrentLink.Id %>', 'address')" 
      onkeyup="updateFocused()" onblur="updateFocused()">
    <a href="http://<%: CurrentLink.Address %>">
      <%: CurrentLink.Address %>
    </a>
    </span>
    <span class="submitter-name"><a href="Profile?user=<%: CurrentLink.Submitter.UserName %>"><%: CurrentLink.Submitter.UserName %></a></span>
    <br/>
    
    <p class="description" title="Description" 
      <% if (canEdit) { %> allows-edit <% } %>
      onfocus="registerFocus('<%: CurrentLink.Id %>', 'description')" 
      onkeyup="updateFocused()" onblur="updateFocused()">
      <%: CurrentLink.Description %>
    </p>
            
    <% if (canEdit) { %>
    <div class="updating-wrapper">
      <span class="working-indicator"><i class="fa fa-circle-o-notch fa-spin"></i> Update pending</span>
    </div>
    <button onclick="toggleEditable(); return false;" class="btn btn-info edit-button"><i class="fa fa-pencil"></i></button>
    <% } %>
            
    <% if (canDelete) { %>
    <button onclick="sendDelete(); return false;" class="btn btn-danger delete-button"><i class="fa fa-trash"></i></button>
    <% } %>
  </article>
  </section>
  
  <section id="comments">
    <h2>Comments</h2>

    <asp:ListView runat="server" ID="CommentsList"
      ItemType="Blacksmith.Models.Comment">
      <ItemTemplate>
        <span class="submitter"><a href="Profile?user=<%# Item.Submitter.UserName %>"><%# Item.Submitter.UserName %></a></span>: <span class="content"><%# Item.Content %></span>
        <br/>
      </ItemTemplate>
      <EmptyDataTemplate>
        <span class="empty-data-text">This link has no comments</span>
      </EmptyDataTemplate>
    </asp:ListView>
    
    <div id="new-comment-form">
      <asp:TextBox ID="NewCommentBox" runat="server" Placeholder="New Comment"></asp:TextBox>
      <asp:Button ID="NewCommentButton" runat="server" Text="Add" CssClass="btn btn-success"/>
    </div>

  </section>
  
  <section id="categories">
    <h2>Categories</h2>
    
    <asp:ListView runat="server" ID="CategoriesList"
      ItemType="Blacksmith.Models.Favorite">
      <ItemTemplate>
        <span class="category"><%# Item.Category %></span>
        <br/>
      </ItemTemplate>
      <EmptyDataTemplate>
        <span class="empty-data-text">This link is not saved in any category</span>
      </EmptyDataTemplate>
    </asp:ListView>
  </section>
  
  <section id="similars">
    <h2>Similar Links</h2>
    
    <asp:ListView runat="server" ID="SimilarsList"
      ItemType="Blacksmith.Models.Link">
      <ItemTemplate>
        <span class="similar"><%# Item.Address %></span>
        <br/>
      </ItemTemplate>
      <EmptyDataTemplate>
        <span class="empty-data-text">This link has no similar pages submitted</span>
      </EmptyDataTemplate>
    </asp:ListView>

  </section>

  <% } %>
  
<%--
  <asp:ListView runat="server"
    ItemType="Blacksmith.Models.Link"
    SelectMethod="GetLink">
    
    <ItemTemplate>
      title: <%# Item.Title %>
      <br/>
      address: <%# Item.Address %>
      <br/>
      description: <%# Item.Description %>
      <br/>
      link submitter: <%# Item.Submitter.UserName %>
      <br/>
      comments: <%# Item.Comments.Count %>
      <br/>
      <asp:Repeater runat="server"
        DataSource="<%# Item.Comments %>"
        ItemType="Blacksmith.Models.Comment">
        <ItemTemplate>
          <%# Item.Content %> by <%# Item.Submitter.UserName %>
          <br/>
        </ItemTemplate>
      </asp:Repeater>
      <br/>

    </ItemTemplate>
    
  </asp:ListView>
  --%>

</asp:Content>