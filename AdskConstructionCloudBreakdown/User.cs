using System;
using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public class User
    {
        public string MailAddress { get; set; }
        public List<string> IndustryRoles { get; set; }

        public Company AssignedCompany { get; set; }



        //Constructor
        public User()
        {
            IndustryRoles = new List<string>();
        }

        public User(string mailAddress)
        {
            MailAddress = mailAddress;
        }

        public User(string mailAddress, string IndustryRoles, Company assignedCompany)
        {
            MailAddress = mailAddress;
            this.IndustryRoles = new List<string>
            {
                IndustryRoles
            };
            this.AssignedCompany = assignedCompany;
        }

        public User(string mailAddress, List<string> IndustryRoles, Company assignedCompany)
        {
            MailAddress = mailAddress;
            this.IndustryRoles = IndustryRoles;
            this.AssignedCompany = assignedCompany;
        }

        public User(string mailAddress, Company assignedCompany)
        {
            MailAddress = mailAddress;
            this.AssignedCompany = assignedCompany;
        }

        public User(string mailAddress, string assignedCompany)
        {
            MailAddress = mailAddress;
            this.AssignedCompany = new Company(assignedCompany);

        }
    }
}