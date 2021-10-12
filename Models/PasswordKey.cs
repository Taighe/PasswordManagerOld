using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager.Models
{
    [Table("Keys")]
    public class PasswordKey
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        [ForeignKey(typeof(PasswordItem))]
        public int PasswordItemId { get; set; }
    }
}
