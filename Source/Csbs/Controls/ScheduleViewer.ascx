<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleViewer.ascx.cs" Inherits="Csbs.Controls.ScheduleViewer" %>

<div>
    
    <h1>CSBS Event Calendar</h1>
    <telerik:RadScheduler runat="server" ID="Scheduler" Skin="Sitefinity" SelectedView="WeekView" Height="730" WorkDayStartTime="8:00" WorkDayEndTime="20:00">
        <ResourceStyles>
            <telerik:ResourceStyleMapping Type="Calendar" Text="Development" ApplyCssClass="rsCategoryGreen" />
            <telerik:ResourceStyleMapping Type="Calendar" Text="Marketing" ApplyCssClass="rsCategoryRed" />
            <telerik:ResourceStyleMapping Type="Calendar" Text="ABC" ApplyCssClass="rsCategoryOrange" />
        </ResourceStyles>
        <DayView UserSelectable="False"></DayView>
        <WeekView WorkDayStartTime="8:00" WorkDayEndTime="20:00" DayStartTime="8:00" DayEndTime="20:00" HeaderDateFormat="MMM d, yyyy" ></WeekView>
        <MonthView HeaderDateFormat="MMMM yyyy"></MonthView>
        <TimelineView UserSelectable="False"></TimelineView>
        <AgendaView UserSelectable="True" NumberOfDays="364" HeaderDateFormat="MMM d, yyyy" ></AgendaView>
        <Localization HeaderAgendaAppointment="Event" AdvancedEditAppointment="Edit Event">
        </Localization>
    </telerik:RadScheduler>

</div>