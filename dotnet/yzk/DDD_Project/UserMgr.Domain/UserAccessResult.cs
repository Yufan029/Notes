namespace UserMgr.Domain;

public enum UserAccessResult
{
    OK,
    PhoneNumberNotFound,
    Lockout,
    NoPassword,
    PasswordError
}
