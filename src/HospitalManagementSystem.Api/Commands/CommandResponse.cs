using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Commands
{
    public class CommandResponse
    {
        private CommandResponse(ObjectId id)
        {
            Id = id;
        }

        public ObjectId Id { get; }

        public static CommandResponse From(ObjectId id)
            => new CommandResponse(id);
    }
}