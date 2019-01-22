<%@ Page Title="Sorry" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sorry.aspx.cs" Inherits="BangersAndCo.Sorry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %>.</h2>
	<p>We are sorry, but due to missed vehicle collections you are no longer allowed to book online.</p>
	<p>Please visit your local branch to book a vehicle.</p>
	<a class="btn" runat="server" href="~/">OK</a>
</asp:Content>
