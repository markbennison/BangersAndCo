<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BangersAndCo._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Banger & Co.</h1>
        <p class="lead">Banger & Co are your one-stop vehicle hire company. Booking online or come into the store.</p>
        <p><a href="/RegisteredUser/BookingView" class="btn btn-primary btn-lg">Book now &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4 col-sm-6 col-xs-12">
            <h2>Small Town Car</h2>
            <p>
                We have hybrid and petrol small town cars, each from £40.00 per day.
            </p>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <h2>Small Family Hatchback</h2>
            <p>
                Our hatchbacks come in a diesel-automatic or a petrol-manual combination priced from £55.00 per day.
            </p>
        </div>
        <div class="col-md-4 col-sm-6 col-xs-12">
            <h2>Large Family Saloon</h2>
            <p>
                The large family saloon is perfect for family trips and vacations, available from £60.00 per day.
            </p>
        </div>
		<div class="col-md-4 col-sm-6 col-xs-12">
            <h2>Large Family Estate</h2>
            <p>
                The large family estate has the extra storage space you need for well-equipped traveller, available from £75.00 per day.
            </p>
        </div>
		<div class="col-md-4 col-sm-6 col-xs-12">
            <h2>Medium Van</h2>
            <p>
                Our medium vans pure and simple, available for £70.00 per day.
            </p>
        </div>
    </div>

</asp:Content>
