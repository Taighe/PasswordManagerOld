using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace PasswordManager.Models
{
    [Table("Passwords")]
    public class PasswordItem
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        [OneToMany]
        public List<PasswordKey> Keys { get; set; }
    }
}