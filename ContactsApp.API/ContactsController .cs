using ContactsApp.Models;
using ContactsApp.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;

namespace ContactsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactsController(ContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public ActionResult<List<Contact>> Get() => _contactService.Get();

    [HttpGet("{id:length(24)}", Name = "GetContact")]
    public ActionResult<Contact> Get(string id)
    {
        var contact = _contactService.Get(id);

        if (contact == null)
        {
            return NotFound();
        }

        return contact;
    }

    [HttpPost]
    public ActionResult<Contact> Create(Contact contact)
    {
        if (_contactService.IsDuplicateContact(contact.FirstName, contact.LastName))
        {
            return Conflict(new { message = "A contact with the same first name and last name already exists." });
        }

        if (string.IsNullOrEmpty(contact.Id))
        {
            contact.Id = ObjectId.GenerateNewId().ToString();
        }

        _contactService.Create(contact);
        return CreatedAtRoute("GetContact", new { id = contact.Id.ToString() }, contact);
    }

    [HttpPut("{id:length(24)}")]
    public IActionResult Update(string id, Contact contactIn)
    {
        var contact = _contactService.Get(id);

        if (contact == null)
        {
            return NotFound();
        }

        if (_contactService.IsDuplicateContact(contactIn.FirstName, contactIn.LastName, id))
        {
            return Conflict(new { message = "A contact with the same first name and last name already exists." });
        }

        _contactService.Update(id, contactIn);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var contact = _contactService.Get(id);

        if (contact == null)
        {
            return NotFound();
        }

        _contactService.Remove(contact.Id);

        return NoContent();
    }

    [HttpGet("search/name")]
    public ActionResult<List<Contact>> SearchByName(string name)
    {
        var contacts = _contactService.Get().Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name)).ToList();
        return contacts;
    }

    [HttpGet("search/phonenumber")]
    public ActionResult<List<Contact>> SearchByPhoneNumber(string phoneNumber)
    {
        var contacts = _contactService.Get().Where(c => c.PhoneNumbers.Contains(phoneNumber)).ToList();
        return contacts;
    }
}
