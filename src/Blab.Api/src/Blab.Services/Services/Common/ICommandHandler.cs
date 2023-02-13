using Audacia.Commands;

namespace Blab.Services.Services.Common;

public interface ICommandHandler<in T, TOutput> : ICommandHandler where T : ICommand
{
    Task<Models.Validation.CommandResult<TOutput>> HandleAsync(
        T command,
        CancellationToken cancellationToken = default(CancellationToken));
}
