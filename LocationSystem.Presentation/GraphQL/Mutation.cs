using Elastic.Clients.Elasticsearch.Inference;

namespace LocationSystem.Presentation.GraphQL
{
    public class Mutation
    {
        public async Task<User> CreateMessageAsync(
           MessageInput messageInput,
           [Service] MessageRepository repository,
           CancellationToken cancellationToken)
    }
}
