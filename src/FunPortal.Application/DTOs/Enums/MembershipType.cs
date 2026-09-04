using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Application.DTOs.Enums;

[SwaggerSchema(
    nameof(MembershipType),
    Description = "The type of membership a user can have. Possible values are BookClub = 1, VideoClub = 2, and Premium = 3.")]
public enum MembershipType
{
    BookClub = 1,

    VideoClub = 2,

    Premium = 3
}
