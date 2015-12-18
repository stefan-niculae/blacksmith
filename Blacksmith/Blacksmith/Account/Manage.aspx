<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="Blacksmith.Account.Manage" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
  <script src="/Scripts/moment.min.js"></script>
  <script src="/Scripts/AddUpdateDeleteLink.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <h1><%: User.Identity.GetUserName() %></h1>
<%--  <%: ((RolePrincipal)User).GetRoles()[0] %>--%>

  <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
    <p class="text-success"><%: SuccessMessage %></p>
  </asp:PlaceHolder>

  <div class="row col-md-12">
    <a class="col-md-offset-10 col-md-1" href="~/Account/ManagePassword" runat="server" title="Change password"><i class="fa fa-key"></i> Change password</a>
    <asp:LoginStatus CssClass="col-md-1" runat="server" LogoutAction="Redirect" LogoutText="<i class='fa fa-sign-out'></i> Log out" LogoutPageUrl="~/" OnLoggingOut="Unnamed_LoggingOut" />
  </div>
  
  <section id="creation">
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
        <ItemTemplate>
          <hr/>

          <article class="big-link row" db-id=<%# Item.Id %>>
            <%-- For each editable field, register it as the current when selected --%>
            <%-- so on deselect, we know which db entity to update --%>
            <h3 title="Title" class="title" 
              contenteditable onfocus="registerFocus('<%# Item.Id %>', 'title')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
              <%# Item.Title %>
            </h3>
            <span class="date difference" abs-date="<%# Item.Date %>" title="Date submitted: <%# Item.Date %>"><%# Item.Date %></span>
            <br/>
            
            <img class="favicon" alt="Favicon" src="<%# Item.Favicon %>" title="Favicon"/>
            <span class="address" title="Address" 
              contenteditable onfocus="registerFocus('<%# Item.Id %>', 'address')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
            <%# Item.Address %>
            </span> 
            <a class="visit-link" href="http://<%# Item.Address %>" title="Visit <%# Item.Address %>"><i class="fa fa-external-link"></i></a>
            <br/>

            <p class="description" title="Description" 
              contenteditable onfocus="registerFocus('<%# Item.Id %>', 'description')" 
              onkeyup="updateFocused()" onblur="updateFocused()">
              <%# Item.Description %>
            </p>
            
            <div class="updating-wrapper">
              <span class="working-indicator"><i class="fa fa-circle-o-notch fa-spin"></i> Update pending</span>
            </div>
            
            <button onclick="sendDelete(<%# Item.Id %>); return false;" class="btn btn-danger delete-button"><i class="fa fa-trash"></i></button>
          </article>

        </ItemTemplate>
    </asp:ListView>
    
  </section>
  

</asp:Content>
