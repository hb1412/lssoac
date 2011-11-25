using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace NetworkInfrastructure
{
    public class MaskCode{
    
    public  const int Mask_Humidify=64;//加湿位掩码
	public  const int Mask_Dehumidify=32;//除湿位掩码
    public  const int Mask_Discharge=16;//排气位掩码
    public  const int Mask_Charge=8; //加气位掩码
    public  const int Mask_Heat=4;//加热位掩码
    public  const int Mask_AirCool=2;//风冷位掩码
    public  const int Mask_WaterCool=1;//水冷位掩码
    }
    
    public  class Data
    {    
        protected int dataLength;
        protected int idNumber;
        protected const int STARTINDEX_LENGTH = 0;//the start index of the field 'dataLength' int the byte sequence 
        protected const int STARTINDEX_IDNUMBER = sizeof(int);
        public const int IDNUMBER_NULL = -1;
        public const int IDNUMBER_BROADCAST = 0;
        public const int IDNUMBER_MINNORMAL = 16;
        public Data()
        {
            dataLength = sizeof(int);
            idNumber =IDNUMBER_NULL;
        }
        public Data(byte[] data)
        {
            dataLength = BitConverter.ToInt32(data, STARTINDEX_LENGTH);
            idNumber = BitConverter.ToInt32(data, STARTINDEX_IDNUMBER);
        }
        public virtual byte[] ToByte()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(dataLength));
            result.AddRange(BitConverter.GetBytes(idNumber));
           
            return result.ToArray();
        }
        public int getIdNumber()
        {
            return idNumber;
        }

    }

    public class Command : Data
    {
        int command;
        double inputO2Density;
        const int STARTINDEX_COMMAND=sizeof(int)*2;           //the start index of the field 'command' int the byte sequence 
        const int STARTINDEX_INPUTO2DENSITY = sizeof(int)*3; //the start index of the field 'inputO2Density' int the byte sequence 


        public Command()
        {
            dataLength = sizeof (int)*2+sizeof (double );
            command = 0;
            inputO2Density = 0.0;
        }
        public Command(byte[] data)
        {
            dataLength = BitConverter.ToInt32(data, STARTINDEX_LENGTH);
            command = BitConverter.ToInt32(data, STARTINDEX_IDNUMBER);
            command = BitConverter.ToInt32(data, STARTINDEX_COMMAND);
            inputO2Density = BitConverter.ToInt32(data, STARTINDEX_INPUTO2DENSITY);

        }
        public override byte[] ToByte()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(dataLength ));
            result.AddRange(BitConverter.GetBytes(idNumber));
            result.AddRange(BitConverter.GetBytes(command));
            result.AddRange(BitConverter.GetBytes(inputO2Density));
            return result.ToArray() ;
        }
    }

    public class StatusSignal : Data
    {
        double temperature;//record the current temperature.
        double humidity;//record the current humidity.
        double pressure;//record the current pressure .
        double oxygenDensity;//record the current density of oxygen.
        double carbonDioxideDensity;//record the currrent density of CO2

        const int STARTINDEX_TEMPERATURE=sizeof(int)*2;
        const int STARTINDEX_HUMIDITY = sizeof(double ) +sizeof (int)*2;
        const int STARTINDEX_PRESSURE = sizeof(double )*2+sizeof(int)*2 ;
        const int STARTINDEX_OXYGENDENSITY = sizeof(double ) * 3 + sizeof(int)*2 ;
        const int STARTINDEX_CARBONDIOXIDEDENSITY = sizeof(double ) * 4 + sizeof(int)*2 ;
        public StatusSignal()
        {
            dataLength =sizeof(double) * 5+sizeof(int);
            temperature = 0.0;
            humidity = 0.0;
            pressure = 0.0;
            oxygenDensity = 0.0;
            carbonDioxideDensity = 0.0;

        }
        public StatusSignal(double t, double h, double p, double o, double c)
        {
            dataLength = sizeof(double) * 5+sizeof(int);
            temperature = t;
            humidity = h;
            pressure = p;
            oxygenDensity = o;
            carbonDioxideDensity = c;
        }
        public StatusSignal(byte[] data)
        {
            dataLength =BitConverter.ToInt32(data,STARTINDEX_LENGTH);
            temperature = BitConverter.ToInt32(data, STARTINDEX_TEMPERATURE);
            humidity = BitConverter.ToInt32(data, STARTINDEX_HUMIDITY);
            pressure = BitConverter.ToInt32(data, STARTINDEX_PRESSURE);
            oxygenDensity = BitConverter.ToInt32(data, STARTINDEX_OXYGENDENSITY);
            carbonDioxideDensity = BitConverter.ToInt32(data, STARTINDEX_CARBONDIOXIDEDENSITY);
        }
        public override byte[] ToByte()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter .GetBytes(dataLength));
            result.AddRange(BitConverter.GetBytes(idNumber));
            result.AddRange(BitConverter.GetBytes(temperature));
            result.AddRange(BitConverter.GetBytes(humidity));
            result.AddRange(BitConverter.GetBytes(pressure));
            result.AddRange(BitConverter.GetBytes(oxygenDensity));
            result.AddRange(BitConverter.GetBytes(carbonDioxideDensity));
            return result.ToArray();
        }
    }
    
}