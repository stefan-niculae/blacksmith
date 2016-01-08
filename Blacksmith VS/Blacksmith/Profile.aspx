<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Blacksmith.Profile" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <script src="/Scripts/moment.min.js"></script>
  <script src="/Scripts/ConvertDates.min.js"></script>
  <script src="/Scripts/UpdateDeleteLink.min.js"></script>
  <script src="/Scripts/AddLink.min.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  
  <%
    if(Username == null)
    {
  %>
    <%--  TODO make this prettier --%>
    no username in url!
  <% } 
    else 
    {
  %>
  <h1><%: Username %>
    <% if (isModerator) { %>
      <span id="moderator-indicator" title="User is moderator"><i class="fa fa-shield"></i></span>
    <% } %>
  </h1>
  
  
  <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
    <p class="text-success"><%: SuccessMessage %></p>
  </asp:PlaceHolder>

  <div class="row col-md-12" id="administration" runat="server">
    <a class="col-md-offset-10 col-md-1" href="~/Account/ManagePassword" runat="server" title="Change password"><i class="fa fa-key"></i> Change password</a>
    <asp:LoginStatus CssClass="col-md-1" runat="server" LogoutAction="Redirect" LogoutText="<i class='fa fa-sign-out'></i> Log out" LogoutPageUrl="~/" OnLoggingOut="Unnamed_LoggingOut" />
  </div>

  <section id="creation" runat="server" ClientIDMode="Static">
    <h2><i class="fa fa-plus"></i> New Link</h2>

    <article id="inserting-link" class="big-link row">
      <h3 title="Title" class="title" contenteditable>
       Title
      </h3>
      <span class="requiredness"><i>optional</i></span>
      <br/>
            
<%--      <img class="favicon" alt="Favicon" src="" title="Favicon"/>--%>
      <span class="address" title="Address" contenteditable>
        Address
      </span> 
      <span class="requiredness"><i>required</i></span>
<%--      <a class="visit-link" href="http://<%# Item.Address %>" title="Visit <%# Item.Address %>"><i class="fa fa-external-link"></i></a>--%>
      <br/>

      <p class="description" title="Description" contenteditable>
        Description
      </p>
      <span class="requiredness"><i>optional</i></span>

      <div class="add-wrapper">
          <%-- on client click returns false because we don't want to page to be refreshed --%>
        <button onclick="sendInsert(); return false;" class="btn btn-success">Add</button>
      </div>
    </article>
  </section>
  

  <section id="submitted" class="col-md-offset-3 col-md-6">
    <h2><span class="glyphicon glyphicon-link" aria-hidden="true"></span> Submitted Links</h2>
    
    <asp:ListView ID="submissions" runat="server"
      SelectMethod="SubmittedLinks"
      ItemType="Blacksmith.Models.Link">
        <EmptyDataTemplate>
          No submitted links: try adding one using the form above this
        </EmptyDataTemplate>

        <ItemTemplate>
          <article class="big-link row" db-id=<%# Item.Id %>>
          <hr/>

            <%-- For each editable field, register it as the current when selected --%>
            <%-- so on deselect, we know which db entity to update --%>
            <h3 title="Title" class="title" 
              <% if (canEdit) { %> allows-edit <% } %>
              onfocus="registerFocus('<%# Item.Id %>', 'title')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
              <a href="Link?addr=<%# Item.Address %>">
                <%# Item.Title %>
              </a>
            </h3>
            <span class="date difference" abs-date="<%# Item.Date %>" title="Date submitted: <%# Item.Date %>"><%# Item.Date %></span>
            <br/>
            
            <img class="favicon" alt="Favicon" src="<%# Item.Favicon %>" title="Favicon"/>
            <span class="address" title="Address" 
              <% if (canEdit) { %> allows-edit <% } %>
              onfocus="registerFocus('<%# Item.Id %>', 'address')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
            <a href="http://<%# Item.Address %>">
              <%# Item.Address %>
            </a>
            </span> 
            <br/>

            <p class="description" title="Description" 
              <% if (canEdit) { %> allows-edit <% } %>
              onfocus="registerFocus('<%# Item.Id %>', 'description')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
              <%# Item.Description %>
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

        </ItemTemplate>
    </asp:ListView>
    
  </section>
  
  <% } %>
</asp:Content>
