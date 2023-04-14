using HospitalManagementSystem.Api.Models.Appointments;
using MediatR;

namespace HospitalManagementSystem.Api.Events.Appointments;

public class AppointmentCreatedEvent : INotification
{
    public Appointment Appointment { get; }

    public AppointmentCreatedEvent(Appointment appointment)
    {
        Appointment = appointment;
    }
}
