using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GSQLBOT.Core.DTOs
{
    public class SqlRequestDTOs
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }

        [JsonPropertyName("schema")]
        public string Schema { get; set; }

        [JsonPropertyName("db_type")]
        public string DbType { get; set; } = "SQL Server";
        public int? ChatId { get; set; }
    }
}
