namespace Workers.Domain.Constants;

public static class ErrorCodes
{
    public const string NotFound = "NOT_FOUND";
    public const string BadRequest = "BAD_REQUEST";
    public const string Conflict = "CONFLICT";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string InternalError = "INTERNAL_ERROR";
    public const string ValidationError = "VALIDATION_ERROR";
    
    public static class Category
    {
        public const string HasChildren = "CATEGORY_HAS_CHILDREN";
        public const string InvalidSlug = "CATEGORY_INVALID_SLUG";
        public const string DuplicateSlug = "CATEGORY_DUPLICATE_SLUG";
    }
    
    public static class User
    {
        public const string EmailInUse = "EMAIL_IN_USE";
        public const string PhoneInUse = "PHONE_IN_USE";
        public const string AlreadyVerified = "ALREADY_VERIFIED";
        public const string InvalidToken = "INVALID_TOKEN";
        public const string AccountLocked = "ACCOUNT_LOCKED";
    }
    
    public static class Authentication
    {
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string TokenExpired = "TOKEN_EXPIRED";
        public const string RefreshTokenInvalid = "REFRESH_TOKEN_INVALID";
    }
    
    public static class Authorization
    {
        public const string InsufficientPermissions = "INSUFFICIENT_PERMISSIONS";
        public const string AdminRequired = "ADMIN_REQUIRED";
        public const string OwnerRequired = "OWNER_REQUIRED";
    }
}
