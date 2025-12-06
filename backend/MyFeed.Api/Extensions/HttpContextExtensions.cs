using System.Security.Claims;

namespace MyFeed.Api.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the current user ID from the JWT token claims.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The user ID as an integer, or null if not found.</returns>
        public static int? GetCurrentUserId(this HttpContext httpContext)
        {
            var subClaim = httpContext.User.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(subClaim) && int.TryParse(subClaim, out var userId))
            {
                return userId;
            }

            var userIdClaim = httpContext.User.FindFirst("user_id")?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out userId))
            {
                return userId;
            }

            var nameIdentifierClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(nameIdentifierClaim) && int.TryParse(nameIdentifierClaim, out userId))
            {
                return userId;
            }

            return null;
        }

        /// <summary>
        /// Gets the current user ID from the JWT token claims, throwing an exception if not found.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The user ID as an integer.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when user ID cannot be found in claims.</exception>
        public static int GetCurrentUserIdRequired(this HttpContext httpContext)
        {
            var userId = httpContext.GetCurrentUserId();
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token claims.");
            }
            return userId.Value;
        }
    }
}

