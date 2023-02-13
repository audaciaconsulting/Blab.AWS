using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blab.Domain.Entities.Security;

namespace Blab.Domain.Models;

/// <summary>
/// Class for following another user.
/// </summary>
public class Follow
{
    /// <summary>
    /// Gets or sets the id of the follower.
    /// </summary>
    public int FollowerId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property of the follower.
    /// </summary>
    public ApplicationUser Follower { get; set; }

    /// <summary>
    /// Gets or sets the id of the followee.
    /// </summary>
    public int FolloweeId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property of the followee.
    /// </summary>
    public ApplicationUser Followee { get; set; }
}