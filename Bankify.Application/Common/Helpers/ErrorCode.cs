namespace Bankify.Application.Common.Helpers
{
    public enum ErrorCode
    {
        UnAuthorized = 401,
        NotFound = 404,
        ServerError = 500,
        RecordFound = 409,
        NetworkError=503,

        //Validation errors should be in the range 100 - 199
        ValidationError = 101,

        //Infrastructure errors should be in the range 200-299
        IdentityCreationFailed = 202,

        //Application errors should be in the range 300 - 399
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301,
        InteractionRemovalNotAuthorized = 302,
        RecordExists = 303,
        IdentityUserDoesNotExist = 304,
        IncorrectPassword = 305,
        UnauthorizedAccountRemoval = 306,
        CommentRemovalNotAuthorized = 307,
        EmptyRquest = 308,
        EnrollmentFailed = 309,
        InvalidFileExtension = 310,


        UnknownError = 999


    }
}
