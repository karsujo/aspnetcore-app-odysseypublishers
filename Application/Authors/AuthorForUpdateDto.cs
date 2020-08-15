﻿using OdysseyPublishers.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Authors
{
    public class AuthorForUpdateDto : AuthorForManipulationDto
    {
        [Required(ErrorMessage = "Address is required for updation.")]
        public override string Address { get => base.Address; set => base.Address = value; }

        [Required(ErrorMessage = "City is required for updation.")]
        public override string City { get => base.City; set => base.City = value; }

        [Required(ErrorMessage = "State is required for updation.")]
        public override string State { get => base.State; set => base.State = value; }

        [Required(ErrorMessage = "ZipCode is required for updation.")]
        public override string Zip { get => base.Zip; set => base.Zip = value; }
    }
}