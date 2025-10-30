using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisticConcurrency
{
    public class House
    {
        public long Id { get; set; }
        public string Address { get; set; }
        public string? Owner { get; set; }

        // 定义为byte[]类型, 作为并发的token
        public byte[] RowVersion { get; set; }
    }
}
