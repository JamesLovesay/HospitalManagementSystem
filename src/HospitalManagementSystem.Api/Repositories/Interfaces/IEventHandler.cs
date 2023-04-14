namespace HospitalManagementSystem.Api.Repositories.Interfaces;

public interface IEventHandler<T>
{
    Task Handle(T @event, CancellationToken cancellationToken);
}
