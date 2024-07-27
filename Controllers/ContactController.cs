using FullStackAPI.Data;
using FullStackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class ContactController : ControllerBase
{
    private readonly DataContext _context;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public ContactController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Contact>>> GetAllContacts()
    {
        Logger.Info("GetAllContacts endpoint called");
        var contacts = await _context.Contact.ToListAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(long id)
    {
        Logger.Info($"GetContact endpoint called with id: {id}");
        var contact = await _context.Contact.FindAsync(id);

        if (contact == null)
        {
            Logger.Warn($"Contact with id: {id} not found");
            return NotFound("Contact not found.");
        }

        return Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<List<Contact>>> CreateContact(Contact contact)
    {
        Logger.Info("CreateContact endpoint called");

        if (contact == null || string.IsNullOrEmpty(contact.FirstName) || string.IsNullOrEmpty(contact.LastName))
        {
            return BadRequest("Invalid contact data.");
        }

        _context.Contact.Add(contact);
        await _context.SaveChangesAsync();

        Logger.Info($"Contact created with id: {contact.Id}");
        var contacts = await _context.Contact.ToListAsync();
        return Ok(contacts);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<List<Contact>>> UpdateContact(long id, Contact contact)
    {
        Logger.Info($"UpdateContact endpoint called with id: {id}");

        if (contact == null || id != contact.Id)
        {
            return BadRequest("Invalid contact data.");
        }

        var dbContact = await _context.Contact.FindAsync(id);

        if (dbContact == null)
        {
            Logger.Warn($"Contact with id: {id} not found");
            return NotFound("Contact not found.");
        }

        dbContact.FirstName = contact.FirstName;
        dbContact.LastName = contact.LastName;
        dbContact.Email = contact.Email;
        dbContact.Phone = contact.Phone;
        dbContact.City = contact.City;

        await _context.SaveChangesAsync();
        Logger.Info($"Contact with id: {id} updated");

        var contacts = await _context.Contact.ToListAsync();
        return Ok(contacts);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<Contact>>> DeleteContact(long id)
    {
        Logger.Info($"DeleteContact endpoint called with id: {id}");

        var dbContact = await _context.Contact.FindAsync(id);

        if (dbContact == null)
        {
            Logger.Warn($"Contact with id: {id} not found");
            return NotFound("Contact not found.");
        }

        _context.Contact.Remove(dbContact);
        await _context.SaveChangesAsync();
        Logger.Info($"Contact with id: {id} deleted");

        var contacts = await _context.Contact.ToListAsync();
        return Ok(contacts);
    }
}
