﻿using CinderellaCore.Model.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model.Models
{
    public class Wish
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string UserID { get; set; }

        [DisplayName("Api ID")]
        public string ApiID { get; set; }

        public DateTime DateAdded { get; set; } = Convert.ToDateTime("1/1/1900");
        public DateTime DateModified { get; set; } = Convert.ToDateTime("1/1/1900");

        [Required]
        [DisplayName("Item Type")]
        public ItemType ItemType { get; set; }

        public string Notes { get; set; }

        public string Category { get; set; }

        [DisplayName("Owned?")]
        public bool Owned { get; set; }

        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }
    }
}