namespace UserMgr.Domain;

public enum CheckCodeResult
{
    OK,
    PhoneNumberNotFound,
    Lockout,
    CodeError,
}