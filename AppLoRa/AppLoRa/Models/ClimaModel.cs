using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLoRa
{
    public class ClimaModel : RxModel
    {
        public float temperatura { get; set; }
        public float umidade { get; set; }
        public int pressao { get; set; }
        public DateTime horario { get; set; }
    }
}
