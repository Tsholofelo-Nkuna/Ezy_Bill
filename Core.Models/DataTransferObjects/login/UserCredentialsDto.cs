using Core.Presentation.Models.DataTransferObjects.Base;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.DataTransferObjects.login
{
    public class UserCredentialsDto: BaseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
