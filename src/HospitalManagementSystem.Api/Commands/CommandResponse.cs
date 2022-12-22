using MongoDB.Bson;

namespace HospitalManagementSystem.Api.Commands
{
    public class CommandResponse
    {
        private CommandResponse(ObjectId id, string message)
        {
            Id = id;
            Message = message;
        }

        public ObjectId Id { get; }
        public string Message { get; }

        public static CommandResponse From(string id, string message)
            => new CommandResponse(ObjectId.Parse(id), message);
    }
}