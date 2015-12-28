<%@ Page Title="Link" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="Blacksmith.Link" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  link details
  <br/>
  
  <asp:Label runat="server" ID="loglabel">label</asp:Label>
  address literal <asp:Literal 
      Text="<%$RouteValue:address%>" 
      runat="server"></asp:Literal>,
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

</asp:Content>