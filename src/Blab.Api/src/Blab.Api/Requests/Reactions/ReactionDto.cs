using System.Linq.Expressions;
using Blab.Domain.Models.Reactions;

namespace Blab.Api.Requests.Reactions;

/// <summary>
/// Reaction request model.
/// </summary>
public class ReactionDto 
{
    /// <summary>
    /// Gets or sets reaction type. 
    /// </summary>
    public ReactionType Type { get; set; }

    /// <summary>
    ///  Gets or sets Counting the reaction types. 
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Gets the count of the reactions on a blab. 
    /// </summary>
    public static Expression<Func<IGrouping<ReactionType, Reaction>, ReactionDto>> FromReact { get; } = reaction => new ReactionDto()
    {
        Type = reaction.First().ReactionType,
        Count = reaction.Count()
    };
}
