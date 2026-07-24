using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Domain.Models
{
    public class ContactModel
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName{ get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Telephone { get; set; }
        public string? IdNumber { get; set; }
        public int? ContactType { get; set; }
        public string? GenderCode { get; set; }
        public string? RSACitizen { get; set; }
        public bool? IsBlack { get; set; }
        public Guid? CredentialsId { get; set; }
        public bool? CredentialsRequested { get; set; }
        public string? Initials { get; set; }
        public string? Designation { get; set; }
        public string? MobilePhone { get; set; }
        public string? FaxNumber { get; set; }
        public Guid? EmployerId { get; set; }
        public string? Employer { get; set; }
        public bool? AccountEnabled { get; set; }
        public Guid TenderId { get; set; }
        public string CollectionAddress { get; set; }
        public string? Role {  get; set; }
    }
}
