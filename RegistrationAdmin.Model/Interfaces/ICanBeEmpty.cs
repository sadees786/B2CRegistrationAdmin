namespace RegistrationAdmin.Models.Interfaces
{
    /// <summary>
    /// Objects that implement this interface can be checked to see if any of their properties have been filled in.
    /// </summary>
    public interface IEmptyCheck
    {
        bool IsEmpty();
    }
}
