namespace RegistrationAdmin.Models.Constants
{
    public static class TextPattern
    {
        public const string Email = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&’'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9A-Za-z][-0-9A-Za-z]*[0-9A-Za-z]*\.)+[a-zA-Z0-9][\-a-zA-Z0-9]{0,22}[a-zA-Z0-9]))$";
    }
}
