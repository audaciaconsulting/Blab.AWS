using System.Linq.Expressions;
using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Blab.Domain.Models;
using Blab.Domain.Models.Photo;

namespace Blab.Api.Requests.Users;

/// <summary>
/// Record for PhotoDto
/// </summary>
/// <param name="Id"></param>
/// <param name="BlobName"></param>
/// <param name="Name"></param>
/// <param name="ImageType"></param>
public record PhotoDto(int Id, string BlobName, string Name, ImageType ImageType)
{
    /// <summary>
    /// Gets PhotoDto from Photo
    /// </summary>
    public static Expression<Func<ProfilePhoto, string, PhotoDto>> FromProfilePhoto { get; } = (photo, profileContainerUrl) => new PhotoDto(photo.Id, profileContainerUrl + photo.BlobName, photo.Name, photo.ImageType);

    /// <summary>
    /// Gets PhotoDto from Photo
    /// </summary>
    public static Expression<Func<BackgroundPhoto, string, PhotoDto>> FromBackgroundPhoto { get; } = (photo, backgroundContainerUrl) => new PhotoDto(photo.Id, backgroundContainerUrl + photo.BlobName, photo.Name, photo.ImageType);
}
