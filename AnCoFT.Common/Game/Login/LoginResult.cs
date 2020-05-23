namespace AnCoFT.Game.Login
{
    public enum LoginResult : short
    {
        Success = 0,
        PasswordIncorrect = -1,
        AlreadyLoggedIn = -2,
        AccountExpired = -3,
        AccountInvalid = -4
    }
}
