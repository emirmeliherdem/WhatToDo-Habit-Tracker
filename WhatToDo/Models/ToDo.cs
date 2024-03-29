﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhatToDo.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}