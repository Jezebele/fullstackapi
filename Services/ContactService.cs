using FullStackAPI.Data;

public interface IContactService
{
    void DeleteContact(int contactId);
}

public class ContactService : IContactService
{
    private readonly DataContext _context;

    public ContactService(DataContext context)
    {
        _context = context;
    }

    public void DeleteContact(int contactId)
    {
        var contact = _context.Contact.Find(contactId);
        if (contact != null)
        {
            _context.Contact.Remove(contact);
            _context.SaveChanges();
        }
    }
}
