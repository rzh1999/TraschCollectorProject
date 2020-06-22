using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Models
{
    public class CustomersModel
    {
        [Key]
        public int CustomerId { get; set; }


        [Display(Name = "First Name:")]
        public string firstName { get; set; }

        [Display(Name = "Last Name:")]
        public string lastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        [Display(Name = "Telephone Number:")]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Collection Fee:")]
        public double CollectionFee { get; set; }

        [Display(Name = "Pick Up Day:")]
        public string PickUpDay { get; set; }

        [Display(Name = "One Time Pick Up:")]
        [Column(TypeName = "date")]
        public DateTime? OneTimeDate { get; set; }

        [Display(Name = "Suspend Start:")]
        [Column(TypeName = "date")]
        public DateTime? SuspendStart { get; set; }

        [Display(Name = "Suspend End:")]
        [Column(TypeName = "date")]
        public DateTime? SuspendEnd { get; set; }

        [Display(Name = "Suspend Service:")]
        public bool SuspendService { get; set; }

        [Display(Name = "Account Balance")]
        public double AccountBalance { get; set; }

        [Display(Name = "Confirm Pick Up:")]
        public bool ConfirmPickUp { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
       
       
    }
}
