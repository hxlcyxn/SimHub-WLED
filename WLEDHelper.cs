using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using SimHub.Plugins.DataPlugins.PersistantTracker;
using SimHub.Plugins.OutputPlugins.ControlRemapper.UI;

namespace Halcyon.WLED
{
    public class WLEDHelper
    {
        private string host;
        private int port;
        public int LedAmount { get; set; }
        public int Offset { get; set; }

        public bool Mirror { get; set; }
        public bool Center { get; set; }

        private (int, int, int) RiseColor { get; set; }
        private (int, int, int) MaxColor { get; set; }

        private UdpClient LedClient { get; set; }

        public WLEDHelper(string host, int port)
        {
            this.host = host;
            this.port = port;
            LedClient = new UdpClient(host, port);
            RiseColor = (0, 0, 255);
            MaxColor = (255, 0, 0);
        }

        public void ShowRpm(double rpm, double maxRpm, bool shouldShift)
        {
            int scaledAmount = (int)rpm / ((int)maxRpm / LedAmount);

            int left = 0 + Offset;
            int right = LedAmount + Offset;

            (int, int, int) outerColor = Mirror ? RiseColor : (0, 0, 0);
            (int, int, int) innerColor = Mirror ? (0, 0, 0) : RiseColor;

            var data = new List<byte>();
            data.Add(0x01); // set WARLS mode
            data.Add(0x01); // wait seconds till return to normal mode

            if (shouldShift)
            {
                for (int i = left; i < right; i++)
                {
                    data.Add(BitConverter.GetBytes(i)[0]); // led index
                    data.AddRange(ColorToByteList(MaxColor));
                }
            }
            else
            {

                if (Center)
                {
                    var center = right / 2;
                    int leftBorder = center - scaledAmount / 2;
                    int rightBorder = center + scaledAmount / 2;

                    if (Mirror)
                    {
                        leftBorder = left + scaledAmount / 2;
                        rightBorder = right - scaledAmount / 2;
                    }

                    for (int i = left; i < leftBorder; i++)
                    {
                        data.Add(BitConverter.GetBytes(i)[0]);
                        data.AddRange(ColorToByteList(outerColor));
                    }
                    for (int i = leftBorder; i < rightBorder; i++)
                    {
                        data.Add(BitConverter.GetBytes(i)[0]);
                        data.AddRange(ColorToByteList(innerColor));
                    }
                    for (int i = rightBorder; i < right; i++)
                    {
                        data.Add(BitConverter.GetBytes(i)[0]);
                        data.AddRange(ColorToByteList(outerColor));
                    }
                }
                else
                {
                    var border = left + scaledAmount;
                    if (Mirror)
                    {
                        border = right - scaledAmount;
                    }
                    for (int i = left; i < border + Offset; i++)
                    {
                        data.Add(BitConverter.GetBytes(i)[0]);
                        data.AddRange(ColorToByteList(innerColor));
                    }

                    for (int i = border; i < right; i++)
                    {
                        data.Add(BitConverter.GetBytes(i)[0]);
                        data.AddRange(ColorToByteList(outerColor));
                    }
                }

            }

            var dataArray = data.ToArray();

            LedClient.Send(dataArray, dataArray.Length);
        }

        private List<Byte> ColorToByteList((int, int, int) color)
        {
            return new List<Byte> { BitConverter.GetBytes(color.Item1)[0], BitConverter.GetBytes(color.Item2)[0], BitConverter.GetBytes(color.Item3)[0] };
        }
    }
}
